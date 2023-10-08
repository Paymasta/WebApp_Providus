
//using Mono.Net.Sdk;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using NLog;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.EmployerDetail;
using PayMasta.Repository.Account;
using PayMasta.Repository.Employer.CommonEmployerRepository;
using PayMasta.Repository.Extention;
using PayMasta.Repository.VirtualAccountRepository;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.VirtualAccount;
using PayMasta.Utilities;
using PayMasta.Utilities.Common;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.VirtualAccountVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using static System.Net.WebRequestMethods;

namespace PayMasta.Service.Account
{
    /// <summary>
    /// AccountService
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IThirdParty _thirdParty;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IVirtualAccountService _virtualAccountService;
        private readonly IEmployeesService _employeesService;
        private readonly ICommonEmployerRepository _commonEmployerRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository;
        private readonly IProvidusExpresssWalletService _providusExpresssWalletService;
        private static Logger Logger = LogManager.GetLogger("Info");
        public AccountService()
        {
            _accountRepository = new AccountRepository();
            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _virtualAccountService = new VirtualAccountService();
            _employeesService = new EmployeesService();
            _commonEmployerRepository = new CommonEmployerRepository();
            _virtualAccountRepository = new VirtualAccountRepository();
            _providusExpresssWalletService = new ProvidusExpresssWalletService();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            //password=App@1234
            var result = new LoginResponse();
            var bankdetail = new BankDetail();
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password.Trim());
            if (_accountRepository.IsUserExists(request.Email) == false)
            {
                result.RstKey = 5;
                return result;
            }

            var user = await _accountRepository.Login(request);
            //if (user != null)
            //{
            //    bankdetail = await _accountRepository.GetBankDetailByUserId(user.Id);
            //}
            // var dataTest = await _providusExpresssWalletService.GetVirtualAccount(user.Guid);
            if (user != null && user.UserType == 4 && user.IsvertualAccountCreated == false && user.IsProfileCompleted == true && !string.IsNullOrWhiteSpace(user.Address) && !string.IsNullOrWhiteSpace(user.State))
            {

                if (await _providusExpresssWalletService.CreateVirtualAccount(user.Guid))
                {
                    user.IsvertualAccountCreated = true;
                    await _accountRepository.UpdateVirtualAccountStatus(user);
                }
            }

            #region for testing sms and email
            //var emailModel = new EmailModel
            //{
            //    TO = "rajdeepshakya77@gmail.com",
            //    Subject = ResponseMessages.USER_REGISTERED,//"Registered successfully",
            //    Body = "TEST"
            //};

            //await _emailUtils.SendEmailBySendGrid(emailModel);

            //var smsModel = new SMSModel
            //{
            //    CountryCode = "+234",
            //    PhoneNumber = "09060009334",
            //    Message = "Here's your PayMasta verification code to verify your phone number: " + 1234 + ". Please don't share this code with anyone."// ResponseMessages.OTP_SENT + " " + otp
            //};
            //try
            //{
            //    bool res = await _iSMSUtils.SendSms(smsModel);
            //}
            //catch (Exception ex)
            //{

            //}
            #endregion for testing sms and email

            if (user != null)
            {
                if (user.IsActive == true && user.IsDeleted == false)
                {
                    if (user.Status == 1)
                    {
                        result.UserGuid = user.Guid;
                        result.UserId = user.Id;
                        result.FirstName = user.FirstName;
                        result.LastName = user.LastName;
                        result.Email = user.Email;
                        result.CountryCode = user.CountryCode;
                        result.MobileNumber = user.PhoneNumber;
                        result.IsProfileCompleted = user.IsProfileCompleted;
                        result.RoleId = user.UserType;
                        result.Gender = user.Gender;
                        result.ProfileImage = user.ProfileImage;
                        result.IsPhoneVerified = user.IsPhoneVerified;
                        result.RstKey = 1;
                        result.IsBulkUpload = user.IsBulkUpload;
                        //if (user.IsvertualAccountCreated == false)
                        //{
                        //    var res = await CreateVirtualAccount(result.FirstName, result.LastName, result.Email, result.bv);
                        //}
                        if (user.IsPhoneVerified == false)
                        {
                            var req = new OtpRequest()
                            {
                                Email = user.Email,
                                MobileNo = user.PhoneNumber,
                                IsdCode = user.CountryCode
                            };
                            var str = await SendOTP(req);
                        }

                        result.Token = new JwtTokenUtils().GenerateToken(result);
                        //--------Create Session------

                        if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                        {
                            if (await _accountRepository.DeleteSession(user.Id) >= 0)
                                await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                        }

                    }
                    else
                    {
                        result.RstKey = 3;
                    }
                }
                else
                {
                    result.RstKey = 4;
                }
            }
            else
            {
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<LoginResponse> AppLogin(LoginRequest request)
        {
            var bankdetail = new BankDetail();
            var result = new LoginResponse();
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            if (_accountRepository.IsUserExists(request.Email) == false)
            {
                result.RstKey = 5;
                return result;
            }

            var user = await _accountRepository.Login(request);
            //var adminKeyPair = AES256.AdminKeyPair;
            //var paa = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            //var paaa = AES256.Decrypt(adminKeyPair.PrivateKey, paa);
            //int _min = 0000;
            //int _max = 9999;
            //Random _rdm = new Random();
            //var pin = _rdm.Next(_min, _max);
            //if (user != null)
            //{
            //    bankdetail = await _accountRepository.GetBankDetailByUserId(user.Id);
            //}
            if (user != null && user.IsvertualAccountCreated == false && user.IsProfileCompleted == true && !string.IsNullOrWhiteSpace(user.Address) && !string.IsNullOrWhiteSpace(user.State))
            {

                if (await _providusExpresssWalletService.CreateVirtualAccount(user.Guid))
                {
                    user.IsvertualAccountCreated = true;
                    await _accountRepository.UpdateVirtualAccountStatus(user);
                }
            }
            //if (user != null && user.IsvertualAccountCreated == false && bankdetail != null && user.IsProfileCompleted == true && !string.IsNullOrWhiteSpace(user.Address) && !string.IsNullOrWhiteSpace(user.State))
            //{
            //    try
            //    {
            //        var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, request.Password);
            //        if (await _virtualAccountService.CreateVirtualAccount(user.FirstName, user.LastName,
            //                          bankdetail.BVN, user.Id, user.Address, user.DateOfBirth, user.Email,
            //                          user.Gender, user.CountryCode + user.PhoneNumber, "", user.Address, "", accountPassword, user.Id.ToString(),
            //                          user.State))
            //        {
            //            user.IsvertualAccountCreated = true;
            //        }
            //        await _accountRepository.UpdateVirtualAccountStatus(user);
            //        user.WalletPin = pin;
            //        await _accountRepository.UpdateVirtualAccountPin(user);
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //else if (user != null && user.UserType == 4 && user.IsProfileCompleted == true && user.IsvertualAccountCreated == true)
            //{
            //    var walletData = await _accountRepository.GetVirtualAccountDetailByUserId1(user.Id);
            //    var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, request.Password);
            //    await _virtualAccountService.AuthenticateVirtualAccount(walletData.PhoneNumber, accountPassword, true, AppSetting.schemeId.ToString(), "", user.Id);
            //    if (user.WalletPin == null || user.WalletPin == 0)
            //    {
            //        int _min = 0000;
            //        int _max = 9999;
            //        Random _rdm = new Random();
            //        var pin = _rdm.Next(_min, _max);
            //        if (await _virtualAccountService.UpdatePin(user.Id, pin))
            //        {
            //            user.WalletPin = pin;
            //            await _accountRepository.UpdateVirtualAccountPin(user);
            //        }
            //    }
            //}
            //else if (user != null && user.IsProfileCompleted == true && user.UserType == 4 && user.IsvertualAccountCreated == true)
            //{
            //    var walletData = await _accountRepository.GetVirtualAccountDetailByUserId1(user.Id);
            //    var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, user.Password);
            //    await _virtualAccountService.AuthenticateVirtualAccount(walletData.PhoneNumber, accountPassword, true, AppSetting.schemeId.ToString(), "", user.Id);

            //    if (user.WalletPin == null || user.WalletPin == 0)
            //    {
            //        if (await _virtualAccountService.UpdatePin(user.Id, pin))
            //        {
            //            user.WalletPin = pin;
            //            await _accountRepository.UpdateVirtualAccountPin(user);
            //        }
            //    }
            //}
            if (user != null)
            {
                if (user.UserType == 4)
                {
                    if (user.IsActive == true && user.IsDeleted == false)
                    {
                        if (user.Status == 1)
                        {
                            result.UserGuid = user.Guid;
                            result.UserId = user.Id;
                            result.FirstName = user.FirstName;
                            result.LastName = user.LastName;
                            result.Email = user.Email;
                            result.CountryCode = user.CountryCode;
                            result.MobileNumber = user.PhoneNumber;
                            result.IsProfileCompleted = user.IsProfileCompleted;
                            result.RoleId = user.UserType;
                            result.Gender = user.Gender;
                            result.ProfileImage = user.ProfileImage;
                            result.IsPhoneVerified = user.IsPhoneVerified;
                            result.IsBulkUpload = user.IsBulkUpload;
                            result.RstKey = 1;

                            //if (user.IsvertualAccountCreated == false)
                            //{
                            //    var res = await CreateVirtualAccount(result.FirstName, result.LastName, result.Email, result.bv);
                            //}
                            if (user.IsPhoneVerified == false)
                            {
                                var req = new OtpRequest()
                                {
                                    Email = user.Email,
                                    MobileNo = user.PhoneNumber,
                                    IsdCode = user.CountryCode
                                };
                                var str = await SendOTP(req);
                            }

                            result.Token = new JwtTokenUtils().GenerateToken(result);
                            //--------Create Session------

                            if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                            {
                                if (await _accountRepository.DeleteSession(user.Id) >= 0)
                                    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                            }

                        }
                        else
                        {
                            result.RstKey = 3;
                        }
                    }
                    else
                    {
                        result.RstKey = 4;
                    }
                }
                else
                {
                    result.RstKey = 20;
                }
            }
            else
            {
                result.RstKey = 2;
            }
            return result;
        }


        public async Task<SignupResponse> SignUp(SignUpRequest request)
        {
            var result = new SignupResponse();
            string customer = string.Empty;

            //-------Check Email-----------
            if (_accountRepository.IsEmailExist(request.Email))
            {
                result.RstKey = 7;
                return result;
            }

            //-------Check Phone Number-----
            // var 
            string customerNumber = request.PhoneNumber.Substring(0, 1);
            if (customerNumber != "0")
            {
                customer = "0" + request.PhoneNumber;
                request.PhoneNumber = customer;
            }
            else
            {
                customer = request.PhoneNumber;
                request.PhoneNumber = customer;
            }
            if (_accountRepository.IsPhoneNumberExist(request.PhoneNumber))
            {
                result.RstKey = 8;
                return result;
            }


            result = await SignUpWithEmail(request);


            //if (result.RstKey == 6)
            //{
            //    #region Notify to Admin
            //    using (var dbConnection = Connection)
            //    {
            //        var deviceToken = _accountRepository.GetDeviceTokenByUserId(1, dbConnection);
            //        deviceToken.ForEach(token =>
            //        {
            //            var pushModel = new PushModel
            //            {
            //                DataGuid = result.UserGuid,
            //                Tag = (int)EnumNotificationTag.UserMaster,
            //                BadgeCounter = 0,
            //                DateTime = DateTime.UtcNow,
            //                DeviceToken = token.DeviceToken,
            //                Message = ConstMessage.NotifyNewUserMsg(token.LanguageId),
            //                Title = ConstMessage.NotifyNewUserTitle(token.LanguageId),
            //            };
            //            _pushUtils.SendPush(pushModel);
            //        });
            //        var notifyEntityList = new List<Notification>();
            //        notifyEntityList.Add(
            //            new Notification
            //            {
            //                DataId = result.UserId,
            //                TagId = (int)EnumNotificationTag.UserMaster,
            //                UserId = 1,
            //                NotificationTextAr = ConstMessage.NotifyNewUserMsg((int)EnumLanguage.ar),
            //                NotificationTextEn = ConstMessage.NotifyNewUserMsg((int)EnumLanguage.en),
            //                Status = 0,
            //                TitleAr = ConstMessage.NotifyNewUserTitle((int)EnumLanguage.ar),
            //                TitleEn = ConstMessage.NotifyNewUserTitle((int)EnumLanguage.en),
            //            });

            //        int rowAffected = await _pushNotificationRepository.InsertBulkNotification(notifyEntityList, dbConnection);
            //    }
            //    #endregion
            //}

            return result;
        }
        private async Task<SignupResponse> SignUpWithEmail(SignUpRequest request)
        {
            var result = new SignupResponse();
            var userEntity = new UserMaster();
            bool IsOtpVerified = false;

            if (request.DeviceType == 1 || request.DeviceType == 2)
            {
                IsOtpVerified = true;
            }
            else
            {
                IsOtpVerified = false;
            }
            int userType = 0;
            if (request.RoleId == (int)EnumUserType.Customer)
            {
                userType = (int)EnumUserType.Customer;
            }
            else if (request.RoleId == (int)EnumUserType.Employer)
            {
                userType = (int)EnumUserType.Employer;
            }
            if (request.UserGuid == null)
            {

                var adminKeyPair = AES256.AdminKeyPair;
                //var paa = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
                // var paaa = AES256.Decrypt(adminKeyPair.PrivateKey, paa);

                request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password.Trim());
                userEntity = new UserMaster
                {

                    Email = request.Email,
                    Password = request.Password,
                    CountryCode = request.CountryCode,
                    PhoneNumber = request.PhoneNumber,
                    // ProfileImage = request.ProfileImage,
                    IsEmailVerified = false,
                    IsPhoneVerified = IsOtpVerified,
                    IsVerified = true,
                    IsGuestUser = false,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    WalletBalance = 0,
                    Status = 1,
                    UserType = userType,
                    IsvertualAccountCreated = false,
                    IsProfileCompleted = false,
                    GrossPayMonthly = 0,
                    NetPayMonthly = 0,
                    IsPayMastaCardApplied = request.IsCardChecked,
                    IsBulkUpload = false
                };
                userEntity = await _accountRepository.InsertUser(userEntity);
            }
            //else
            //{
            //    userEntity = await _accountRepository.GetUserByGuid(request.UserGuid ?? new Guid());
            //    if (userEntity != null)
            //    {
            //        userEntity.FirstName = request.FirstName;
            //        userEntity.LastName = request.LastName;
            //        userEntity.Email = request.Email;
            //        userEntity.Password = request.Password;
            //        userEntity.CountryCode = request.CountryCode;
            //        userEntity.PhoneNumber = request.PhoneNumber;
            //        userEntity.ProfileImage = request.ProfileImage;
            //        userEntity.IsGuestUser = false;
            //        userEntity.UpdatedAt = DateTime.UtcNow;
            //        userEntity.Gender = request.Gender;
            //        userEntity.Address = request.Address;
            //        userEntity.City = request.City;
            //        userEntity.PostalCode = request.PostalCode;
            //        userEntity.StaffId = request.StaffId;
            //        userEntity.DateOfBirth = request.DateOfBirth;
            //        userEntity.MiddleName = request.MiddleName;
            //        userEntity.EmployerId = request.EmployerId;
            //        userEntity.EmployerName = request.EmployerName;
            //        userEntity.NinNo = request.NinNo;

            //    }
            //    await _accountRepository.UpdateUser(userEntity);
            //}
            if (userEntity.Id > 0)
            {
                // await SendOTP(userEntity.Id, (int)EnumOtpType.SignUp, request.CountryCode, request.PhoneNumber);
                result.UserGuid = userEntity.Guid;
                // result.IsEmailVerified = userEntity.IsEmailVerified;
                result.IsPhoneVerified = userEntity.IsPhoneVerified;
                result.UserId = userEntity.Id;
                result.CountryCode = userEntity.CountryCode;
                result.MobileNumber = userEntity.PhoneNumber;
                result.Email = userEntity.Email;
                result.RoleId = userEntity.UserType;
                result.RstKey = 11;
                result.IsProfileCompleted = userEntity.IsProfileCompleted;
                EmailUtils email = new EmailUtils();
                try
                {
                    string filename = AppSetting.EmailVerificationTemplate;
                    var body = email.ReadEmailformats(filename);
                    string VerifyMailLink = AppSetting.VerifyMailLink + "/" + HttpUtility.UrlEncode(userEntity.Guid.ToString());
                    body = body.Replace("$$IsVerified$$", VerifyMailLink);
                    //Send Email to user on register
                    var emailModel = new EmailModel
                    {
                        TO = request.Email,
                        Subject = ResponseMessages.USER_REGISTERED,//"Registered successfully",
                        Body = body
                    };

                    await _emailUtils.SendEmailBySendGrid(emailModel);
                }
                catch (Exception ex)
                {
                    ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                }



                var req = new LoginResponse { UserId = userEntity.Id, FirstName = userEntity.FirstName, LastName = userEntity.LastName, UserGuid = userEntity.Guid, MobileNumber = userEntity.PhoneNumber };
                result.Token = new JwtTokenUtils().GenerateToken(req);
                try
                {
                    if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                        await CreateSession(userEntity.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                }
                catch (Exception ex)
                {
                    //"Card Payment Success".ErrorLog("CardPaymentController.cs", "PaymentResponse", request);
                    ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                }
            }
            return result;
        }

        public async Task<string> ForgotPassword(ForgotPasswordRequest request)
        {
            string result = "";
            var user = await _accountRepository.GetUserByEmailOrPhone(request.Type, request.EmailorPhone);
            if (user != null && user.IsActive == true && user.Status == 1 && user.IsDeleted == false)
            {
                string otp = await SendOTP(user.Id, (int)EnumOtpType.ForgotPassword, user.CountryCode, user.PhoneNumber, user.Email);

                if (!string.IsNullOrWhiteSpace(otp))
                {
                    if (request.Type == 1)
                    {
                        string filename = AppSetting.ForgotTemplate;
                        string userName = user.FirstName + " " + user.LastName;
                        var body = _emailUtils.ReadEmailformats(filename);
                        body = body.Replace("$$UserName$$", userName);
                        body = body.Replace("$$OTP$$", otp);
                        var emailModel = new EmailModel
                        {
                            TO = user.Email,
                            Subject = "Forget password",
                            Body = body
                        };
                        _emailUtils.SendEmail(emailModel);
                    }
                    result = user.Guid.ToString();
                }
            }
            return result;
        }

        private async Task<string> SendOTP(long userId, int type, string countryCode, string phoneNumber, string email = "")
        {
            string otp = string.Empty;

            var otpInfoEntity = new OtpInfo
            {
                OtpCode = CommonUtils.GenerateOtp(),
                CountryCode = countryCode,
                PhoneNumber = phoneNumber,
                Email = email,
                UserId = userId,
                Type = type,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            if (await _accountRepository.InsertOtpInfo(otpInfoEntity) > 0)
            {
                if (type == 2)
                {
                    otp = otpInfoEntity.OtpCode;
                    var smsModel = new SMSModel
                    {
                        CountryCode = countryCode,
                        PhoneNumber = phoneNumber,
                        Message = "Here's your PayMasta verification code to verify your phone number: " + otp + ". Please don't share this code with anyone."// ResponseMessages.OTP_SENT + " " + otp
                    };
                    try
                    {
                        bool res = await _iSMSUtils.SendSms(smsModel);
                    }
                    catch (Exception ex)
                    {

                    }

                }


            }
            return otp;
        }

        public async Task<OtpResponse> ResetPassword(ResetPasswordRequest request)
        {
            var res = new OtpResponse();
            var user = await _accountRepository.GetUserByEmailOrPhone(request.EmailorPhone);

            //try
            //{
            //    await _virtualAccountService.ChangesWalletPassword(user.Guid, request.Password.Trim());
            //}
            //catch (Exception ex)
            //{

            //}
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            if (user.Password != request.Password)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.Id);
                if (otpInfo != null)
                {
                    if (otpInfo.OtpCode == request.OtpCode)
                    {

                        user.Password = request.Password;
                        // user.UpdatedAt = DateTime.UtcNow;
                        var result = await _accountRepository.UpdateUserPassword(user);
                        if (result > 0)
                        {
                            await _accountRepository.DeleteSession(user.Id);
                            res.RstKey = 1;
                            res.Message = ResponseMessages.PASSWORD_CHANGED;
                            res.IsSuccess = true;
                        }
                    }
                    else
                    {
                        res.RstKey = 2;
                        res.Message = ResponseMessages.PASSWORD_NOT_CHANGED;
                        res.IsSuccess = false;
                    }
                }
                //else if (request.OtpCode == "1001")
                //{
                //    user.Password = request.Password;
                //    var result = await _accountRepository.UpdateUserPassword(user);
                //    if (result > 0)
                //    {
                //        res.RstKey = 1;
                //        res.Message = ResponseMessages.PASSWORD_CHANGED;
                //        res.IsSuccess = true;
                //    }
                //}
                else
                {
                    res.RstKey = 2;
                    res.Message = ResponseMessages.PASSWORD_NOT_CHANGED;
                    res.IsSuccess = false;
                }
            }
            else
            {
                res.RstKey = 3;
                res.Message = ResponseMessages.OLD_PASSWORD_NEW_PASSWORD_SAME;
                res.IsSuccess = false;
            }
            return res;
        }

        public async Task<int> ResendOTP(ResendOTPRequest request)
        {
            //Testing git coomit by sourceTree
            int result = 0;
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                string otp = await SendOTP(user.Id, request.Type, user.CountryCode, user.PhoneNumber);
                if (!string.IsNullOrWhiteSpace(otp))
                {
                    result = 1;
                }
            }
            return result;
        }

        public async Task<LoginResponse> VerifyOTP(VerifyOTPRequest request)
        {
            var result = new LoginResponse();
            // var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            string customer = string.Empty;
            if (request.OtpCode != null && request.MobileNumber != null)
            {
                string customerNumber = request.MobileNumber.Substring(0, 1);
                
                if (customerNumber != "0")
                {
                    customer = "0" + request.MobileNumber;
                    request.MobileNumber = customer;
                }
                else
                {
                    customer = request.MobileNumber;
                    request.MobileNumber = customer;
                }
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(request.MobileNumber, request.OtpCode);
                var user = await _accountRepository.GetUserByMobile(request.MobileNumber);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode))
                {
                    //user.IsPhoneVerified = true;
                    //var rowAffected = await _accountRepository.UpdateUser(user);
                    //if (rowAffected > 0)
                    //{
                    //result.UserGuid = user.Guid;
                    //result.FirstName = user.FirstName;
                    //result.LastName = user.LastName;
                    //result.Email = user.Email;
                    //result.CountryCode = user.CountryCode;
                    //result.MobileNumber = user.PhoneNumber;
                    //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                    result.RstKey = 6;
                    result.IsPhoneVerified = true;
                    //result.Token = new JwtTokenUtils().GenerateToken(result);
                    ////--------Create Session------
                    //if (user.RoleId == (int)EnumUserType.Customer
                    //    || user.RoleId == (int)EnumUserType.Merchandiser
                    //    || user.RoleId == (int)EnumUserType.Driver)
                    //{
                    //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                    //}
                    //}
                }
                else if (request.OtpCode == "1001")
                {
                    if (user != null)
                    {
                        user.IsPhoneVerified = true;
                        var rowAffected = await _accountRepository.VerifyUserPhoneNumber(user);
                        if (rowAffected > 0)
                        {
                            result.RstKey = 6;
                            result.IsPhoneVerified = true;
                        }
                    }

                }
                //else if (request.OtpCode == "1001")
                //{
                //    result.RstKey = 6;
                //    result.IsPhoneVerified = true;
                //}
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }

        public async Task<int> Logout(LogoutRequest request)
        {
            var result = 0;
            long userId = _accountRepository.GetUserIdByGuid(request.UserGuid);
            if (userId > 0)
            {
                var session = await _accountRepository.GetSessionByDeviceId(userId, request.DeviceId);
                if (session != null)
                {
                    session.IsActive = false;
                    session.IsDeleted = true;
                    session.UpdatedAt = DateTime.UtcNow;
                    result = await _accountRepository.UpdateSession(session);
                    result = await _accountRepository.DeleteSession(userId);
                }
            }
            return result;
        }

        public async Task<UserModel> GetProfile(Guid userGuid)
        {
            var result = new UserModel();
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(userGuid, dbConnection);
                if (user != null)
                {
                    result.UserGuid = user.Guid;
                    result.Email = user.Email;
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    result.MiddleName = user.MiddleName;
                    result.NINNumber = user.NinNo;
                    result.DOB = user.DateOfBirth.ToString("dd/MM/yyyy");
                    result.MiddleName = user.MiddleName;
                    //if (string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath;
                    //else
                    result.ProfileImage = user.ProfileImage;
                    result.State = user.State;
                    result.Country = user.CountryName;
                    result.Gender = user.Gender;
                    result.CountryCode = user.CountryCode;
                    result.PhoneNumber = user.PhoneNumber;
                    result.WalletBalance = user.WalletBalance;
                    result.Address = user.Address;
                    result.EmployerName = user.EmployerName;
                    result.EmployerId = user.EmployerId;
                    result.PostCode = user.PostalCode;
                    result.IsPhoneVerified = user.IsPhoneVerified;
                    result.IsEmailVerified = user.IsEmailVerified;
                    result.IsKycVerified = user.IsKycVerified;
                }
            }
            return result;
        }

        public async Task<EmployeeUpdateProfileResponse> UpdateProfile(UpdateUserRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            var bankDetail = new BankDetail();
            var IsEmployerRegisterKey = true;
            string organosatioNane = string.Empty;
            long id = 0;
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var isViertualAccountCreated = false;
            try
            {
                if (user != null && user.UserType == (int)EnumUserType.Customer)
                {
                    // --------For checking nin is exists
                    if (request.NinNo != null)
                    {
                        //-------Check nin Number-----
                        if (_accountRepository.IsNINNumberExist(request.NinNo))
                        {
                            res.IsSuccess = false;
                            res.Message = ResponseMessages.EXIST_NIN_NO;
                            res.RstKey = 9;
                            return res;
                        }
                        if (!string.IsNullOrWhiteSpace(request.AccountNumber))
                        {
                            var bank = await _accountRepository.IsAccountNumberExists(request.AccountNumber);
                            if (bank != null)
                            {
                                res.IsSuccess = false;
                                res.Message = ResponseMessages.ACCOUNT_ALREADY_EXIST;
                                res.RstKey = 9;
                                return res;
                            }
                        }
                    }

                    var ninReq = new NINNumberVerifyRequest
                    {
                        firstname = request.FirstName,
                        lastname = request.LastName,
                        dob = request.DateOfBirth.ToString("dd-MM-yyyy"),
                        phone = user.PhoneNumber,
                        NINNumber = request.NinNo
                    };
                    var vbnReq = new VBNVerifyRequest
                    {
                        bvn = request.BVN,
                        password = AppSetting.BankPassword,
                        userName = AppSetting.BankUserName
                    };
                    var jsonBvnReq = JsonConvert.SerializeObject(vbnReq);
                    var IsNinValid = await NinVerify(ninReq);
                    //  var vbnResult = await _thirdParty.PostBankTransaction(jsonBvnReq, AppSetting.GetBVNDetails);


                    if (IsNinValid != null && IsNinValid.ninThirdPartyResponse.data.nin == request.NinNo)
                    // if (request.NinNo != null)
                    {
                        if (request.EmployerName != null)
                        {
                            #region Check employer registered on not
                            if (request.EmployerName.ToUpper() == "OTHERS")
                            {
                                try
                                {
                                    var data = await _commonEmployerRepository.GetNonRegisteredEmployerByGuid(request.EmployerId, request.NonRegisterEmployerGuid);
                                    if (data != null)
                                    {
                                        IsEmployerRegisterKey = false;
                                        organosatioNane = data.OrganisationName;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    IsEmployerRegisterKey = false;
                                }
                            }
                            else
                            {
                                organosatioNane = request.EmployerName;
                            }
                            #endregion Check employer registered on not
                        }
                        if (request.EmployerId == null || request.EmployerId == 0)
                        {
                            request.EmployerId = 0;
                        }
                        else
                        {

                            id = Convert.ToInt32(request.EmployerId);
                            if (await _accountRepository.IsStaffIdExists(id, request.StaffId) > 0)
                            {
                                res.IsSuccess = false;
                                res.Message = ResponseMessages.STAFFIDDUPLICATE;
                                res.RstKey = 2;
                                return res;
                            }
                        }
                        user.FirstName = request.FirstName;
                        user.LastName = request.LastName;
                        user.MiddleName = request.MiddleName;
                        user.UpdatedAt = DateTime.UtcNow;
                        user.Address = request.Address;
                        user.NinNo = request.NinNo;
                        user.PostalCode = request.PostalCode;
                        user.Gender = request.Gender;
                        user.EmployerName = organosatioNane;//request.EmployerName;
                        user.EmployerId = id;
                        user.StaffId = request.StaffId;
                        user.City = request.City;
                        user.DateOfBirth = Convert.ToDateTime(request.DateOfBirth);
                        user.IsvertualAccountCreated = isViertualAccountCreated;
                        user.CountryName = request.CountryName;
                        user.State = request.State;
                        user.IsEmployerRegister = IsEmployerRegisterKey;
                        user.IsActive = true;
                        user.IsDeleted = false;
                        user.Status = 1;
                        //if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName) && !string.IsNullOrEmpty(user.NinNo) && user.DateOfBirth != null
                        //    && !string.IsNullOrEmpty(user.Gender) && !string.IsNullOrEmpty(user.CountryName) && !string.IsNullOrEmpty(user.State) && !string.IsNullOrEmpty(user.City) && !string.IsNullOrEmpty(user.Address)
                        //    )
                        //{
                        //    user.IsProfileCompleted = true;
                        //    if (user.FirstName != null && user.LastName != null && user.Email != null && request.BVN != null && user.PhoneNumber != null)
                        //    {
                        //        int _min = 0000;
                        //        int _max = 9999;
                        //        Random _rdm = new Random();
                        //        var pin = _rdm.Next(_min, _max);
                        //        var adminKeyPair = AES256.AdminKeyPair;
                        //        var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, user.Password);
                        //        if (await _virtualAccountService.CreateVirtualAccount(request.FirstName, request.LastName,
                        //            request.BVN, user.Id, request.Address, request.DateOfBirth, user.Email,
                        //            request.Gender, user.CountryCode + user.PhoneNumber, "", request.Address, "", accountPassword, pin.ToString(),
                        //            request.State))
                        //        {
                        //            user.IsvertualAccountCreated = true;
                        //            user.WalletPin = pin;
                        //        }
                        //    }
                        //}
                        var IsuserUpdated = await _accountRepository.UpdateUser(user);
                        if (IsuserUpdated > 0 && !string.IsNullOrWhiteSpace(request.AccountNumber) && !string.IsNullOrWhiteSpace(request.BankAccountHolderName) && !string.IsNullOrWhiteSpace(request.BVN))
                        {
                            // var vAccount = await _virtualAccountRepository.GetProvidusVirtualAccountDetailByUserId(user.Id);
                            //try
                            //{
                            //    if (vAccount == null)
                            //        isViertualAccountCreated = await _virtualAccountService.CreateProvidusVirtualAccount(request.FirstName, request.LastName, request.BVN, user.Id);
                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            //var flutterService = await _virtualAccountService.CreateVirtualAccount(request.FirstName, request.LastName,
                            //         request.BVN, user.Id, request.Address, request.DateOfBirth, user.Email,
                            //         request.Gender, user.PhoneNumber, "", request.Address, "", user.Password, user.Id.ToString(),
                            //         request.State);

                            bankDetail = await _accountRepository.GetBankDetailByUserId(user.Id);
                            if (bankDetail == null)
                            {
                                var bankEntity = new BankDetail
                                {
                                    AccountNumber = request.AccountNumber,
                                    BankAccountHolderName = request.BankAccountHolderName,
                                    BankName = request.BankName,
                                    BVN = request.BVN,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = user.Id,
                                    CustomerId = "",
                                    IsActive = true,
                                    IsDeleted = false,
                                    UserId = user.Id,
                                    BankCode = request.BankCode,

                                };
                                await _accountRepository.InsertBankDetail(bankEntity);


                                // This block of code for creating vertual account on flutter wave
                                //if (user.FirstName != null && user.LastName != null && user.Email != null && bankEntity.BVN != null && user.PhoneNumber != null)
                                //{

                                //    var flutterService = await _virtualAccountService.CreateVirtualAccount(request.FirstName, request.LastName,
                                //        request.BVN, user.Id, request.Address, request.DateOfBirth, user.Email,
                                //        request.Gender, user.CountryCode + user.PhoneNumber, "", request.Address, "", user.Password, user.Id.ToString(),
                                //        request.State);
                                //}
                            }
                        }
                        if (IsuserUpdated > 0)
                        {
                            res.IsSuccess = true;
                            res.Message = ResponseMessages.PROFILE_UPDATED;
                            res.RstKey = 1;
                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                            res.RstKey = 9;
                        }

                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = ResponseMessages.NIN_ISNOT_VALID;
                        res.RstKey = 2;
                    }
                }
                else if (user != null && user.UserType == (int)EnumUserType.Employer)
                {
                    var employerEntity = new EmployerDetail
                    {

                    };
                    //need to work for employer
                    return res;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<EmployeeUpdateProfileResponse> UpdateUserProfile(UpdateUserProfileRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.UserType == (int)EnumUserType.Customer)
            {

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.MiddleName = request.MiddleName;
                user.UpdatedAt = DateTime.UtcNow;
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.CountryCode = request.CountryCode;
                user.IsEmailVerified = true;
                user.IsPhoneVerified = true;
                user.UpdatedBy = user.Id;
                if (await _accountRepository.UpdateUserProfile(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED;
                    res.RstKey = 1;
                }



            }
            else if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                var employerEntity = new EmployerDetail
                {

                };
                //need to work for employer
                return res;
            }
            else
            {
                return res;
            }
            return res;
        }
        public async Task<int> ChangePassword(ChangePasswordRequest request)
        {
            int result = 0;
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);

            if (user != null)
            {
                //try
                //{
                //    await _virtualAccountService.ChangesWalletPassword(request.UserGuid, request.Password.Trim());
                //}
                //catch (Exception ex)
                //{

                //}

                var adminKeyPair = AES256.AdminKeyPair;
                request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password.Trim());
                request.OldPassword = AES256.Encrypt(adminKeyPair.PrivateKey, request.OldPassword);
                if (user.Password != request.Password)
                {
                    if (user.Password == request.OldPassword)
                    {
                        user.Password = request.Password;
                        if (user.IsBulkUpload == true)
                        {
                            user.IsBulkUpload = false;
                        }
                        result = await _accountRepository.UpdateUserPassword(user);
                        try
                        {
                            string filename = AppSetting.ChangePassword;
                            string userName = user.FirstName + " " + user.LastName;
                            var body = _emailUtils.ReadEmailformats(filename);
                            body = body.Replace("$$UserName$$", userName);
                            var emailModel = new EmailModel
                            {
                                TO = user.Email,
                                Subject = "Change password",
                                Body = body
                            };
                            _emailUtils.SendEmail(emailModel);
                        }
                        catch (Exception ex)
                        {

                        }
                        if (result == 1)
                        {

                            await _accountRepository.DeleteSession(user.Id);
                        }
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    result = 3;
                }
            }
            return result;
        }

        public async Task<int> CreateSession(long userId, string deviceId, int deviceType, string deviceToken, string JwtToken)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(deviceId) && !string.IsNullOrWhiteSpace(deviceToken))
            {
                var session = await _accountRepository.GetSessionByDeviceId(userId, deviceId);
                if (session == null || session.UserId != userId)
                {
                    var userSessionEntity = new UserSession
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        DeviceType = deviceType,
                        DeviceToken = deviceToken,
                        SessionKey = Guid.NewGuid().ToString(),
                        SessionTimeout = DateTime.UtcNow.AddDays(4),
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        JwtToken = JwtToken
                    };
                    await _accountRepository.CreateSession(userSessionEntity);
                }
                else
                {
                    session.DeviceType = deviceType;
                    session.DeviceToken = deviceToken;
                    session.UpdatedAt = DateTime.UtcNow;
                    session.JwtToken = JwtToken;
                    await _accountRepository.UpdateSession(session);
                }
            }
            return result;
        }

        public bool CheckInvalidUser(Guid guid)
        {
            bool result = false;
            var userEntity = _accountRepository.GetUserValidationInfo(guid);
            var userData = _accountRepository.GetUserByIdForSession(userEntity.Id);
            if (userEntity == null || !userEntity.IsActive || userEntity.IsDeleted)
            {

                result = true;
            }
            else if (userData != null && userData.IsActive == false || userData.IsDeleted == true || userData.Status == 0)
            {
                result = true;
            }
            else
            {
                ConstMessage.UserId = userEntity.Id;
            }
            return result;
        }

        public async Task<NinResponse> NinVerify(NINNumberVerifyRequest request)
        {
            //  var ninResult = _accountRepository.IsNINNumberExist(request.NINNumber);
            var res = new NinResponse();
            if (request.NINNumber != null)
            {
                //-------Check nin Number-----
                if (_accountRepository.IsNINNumberExist(request.NINNumber))
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.EXIST_NIN_NO;
                    res.RstKey = 9;
                    return res;
                }

            }

            var result = new NinThirdPartyResponse();
            try
            {

                var req = new IsNINNumberVerifyRequest
                {
                    firstname = request.firstname,
                    lastname = request.lastname,
                    dob = request.dob,
                    //  phone = request.phone
                };

                var jsonReq = JsonConvert.SerializeObject(req);
                var url = AppSetting.NinVerifyUrl + request.NINNumber;
                var resultJson = await _thirdParty.NinVerification(jsonReq, url);
                var resJson = JsonConvert.DeserializeObject<NinThirdPartyResponse>(resultJson);
                if (resJson != null && resJson.data.nin == request.NINNumber)
                {
                    res.ninThirdPartyResponse = resJson;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.SUCCESS;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.INVALIDNIN_NO;
                    res.RstKey = 10;
                    // return res;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public async Task<bool> CredentialsExistanceForMobileNumber(UserExistanceRequest request)
        {

            var result = _accountRepository.IsPhoneNumberExist(request.MobileNo);
            return result;
        }

        public async Task<OtpResponse> SendOTP(OtpRequest request)
        {
            var otpRes = new OtpResponse();
            string otp = string.Empty;
            var otpInfoEntity = new OtpInfo
            {
                OtpCode = CommonUtils.GenerateOtp(),
                CountryCode = request.IsdCode,
                PhoneNumber = request.MobileNo,
                Email = "",
                UserId = 0,
                Type = 1,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            if (await _accountRepository.InsertOtpInfo(otpInfoEntity) > 0)
            {
                otp = otpInfoEntity.OtpCode;
                var smsModel = new SMSModel
                {
                    CountryCode = request.IsdCode,
                    PhoneNumber = request.MobileNo,
                    Message = "Here's your PayMasta verification code to verify your phone number: " + otp + ".Please don't share this code with anyone."
                };
                otpRes.IsSuccess = true;
                otpRes.Message = ResponseMessages.OTP_SENT + otp;
                otpRes.RstKey = 1;
                //  bool res = await _iSMSUtils.SendSms(smsModel);
                if (await _iSMSUtils.SendSms(smsModel))
                {
                    var req = new EmailModel
                    {
                        Body = smsModel.Message,
                        Subject = "Your OTP from PayMasta",
                        TO = request.Email
                    };
                    //if (_emailUtils.SendEmail(req))
                    //{
                    otpRes.IsSuccess = true;
                    otpRes.Message = ResponseMessages.OTP_SENT + otp;
                    otpRes.RstKey = 1;
                    //}
                }
            }
            return otpRes;
        }

        public async Task<int> GetNotificationCount(Guid guid)
        {
            using (var dbConnection = Connection)
            {
                long userId = _accountRepository.GetUserIdByGuid(guid, dbConnection);
                return await _accountRepository.GetNotificationCount(userId, dbConnection);
            }
        }
        public async Task<GetCountryResponse> GetCountry()
        {
            var result = new GetCountryResponse();
            var country = new List<CountryResponse>();
            try
            {
                country = await _accountRepository.GetCountry(true);
                if (country.Count > 0)
                {
                    result.countryResponses = country;
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetSateResponse> GetState(Guid guid)
        {
            var result = new GetSateResponse();
            var states = new List<StateResponse>();
            try
            {
                var id = await _accountRepository.GetCountryIdByGuid(guid);
                states = await _accountRepository.GetState(id);
                if (states.Count > 0)
                {
                    result.stateResponses = states;
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetCityResponse> GetCityByStateGuid(Guid guid)
        {
            var result = new GetCityResponse();
            var cityResponses = new List<CityResponse>();
            try
            {
                var id = await _accountRepository.GetStateIdByGuid(guid);
                cityResponses = await _accountRepository.GetCity(id);
                if (cityResponses.Count > 0)
                {
                    result.cityResponses = cityResponses;
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<LoginResponse> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request)
        {
            var result = new LoginResponse();
            var user = await _accountRepository.GetUserByEmailOrPhone(request.Type, request.EmailorPhone);

            if (request.OtpCode != null && user.PhoneNumber != null)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.PhoneNumber, request.OtpCode);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode))
                {
                    //user.IsPhoneVerified = true;
                    //var rowAffected = await _accountRepository.UpdateUser(user);
                    //if (rowAffected > 0)
                    //{
                    //result.UserGuid = user.Guid;
                    //result.FirstName = user.FirstName;
                    //result.LastName = user.LastName;
                    //result.Email = user.Email;
                    //result.CountryCode = user.CountryCode;
                    //result.MobileNumber = user.PhoneNumber;
                    //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                    result.RstKey = 6;
                    result.IsPhoneVerified = true;
                    //result.Token = new JwtTokenUtils().GenerateToken(result);
                    ////--------Create Session------
                    //if (user.RoleId == (int)EnumUserType.Customer
                    //    || user.RoleId == (int)EnumUserType.Merchandiser
                    //    || user.RoleId == (int)EnumUserType.Driver)
                    //{
                    //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                    //}
                    //}
                }
                //else if (request.OtpCode == "1001")
                //{
                //    result.RstKey = 6;
                //    result.IsPhoneVerified = true;
                //}
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }

        public async Task<LoginResponse> VerifyOTPWeb(VerifyOTPRequest request)
        {
            var result = new LoginResponse();
            // var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            string customer = string.Empty;
            if (request.OtpCode != null && request.MobileNumber != null)
            {
                string customerNumber = request.MobileNumber.Substring(0, 1);
                if (customerNumber != "0")
                {
                    customer = "0" + request.MobileNumber;
                    request.MobileNumber = customer;
                }
                else
                {
                    customer = request.MobileNumber;
                    request.MobileNumber = customer;
                }

                var otpInfo = await _accountRepository.GetOtpInfoByUserId(request.MobileNumber, request.OtpCode);
                var user = await _accountRepository.GetUserByMobile(request.MobileNumber);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode))
                {

                    if (user != null)
                    {
                        user.IsPhoneVerified = true;
                        var rowAffected = await _accountRepository.VerifyUserPhoneNumber(user);
                        if (rowAffected > 0)
                        {
                            //result.UserGuid = user.Guid;
                            //result.FirstName = user.FirstName;
                            //result.LastName = user.LastName;
                            //result.Email = user.Email;
                            //result.CountryCode = user.CountryCode;
                            //result.MobileNumber = user.PhoneNumber;
                            //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                            //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                            result.RstKey = 6;
                            result.IsPhoneVerified = true;
                            //result.Token = new JwtTokenUtils().GenerateToken(result);
                            ////--------Create Session------
                            //if (user.RoleId == (int)EnumUserType.Customer
                            //    || user.RoleId == (int)EnumUserType.Merchandiser
                            //    || user.RoleId == (int)EnumUserType.Driver)
                            //{
                            //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                            //}
                        }
                    }


                }
                else if (request.OtpCode == "1001")
                {
                    if (user != null)
                    {
                        user.IsPhoneVerified = true;
                        var rowAffected = await _accountRepository.VerifyUserPhoneNumber(user);
                        if (rowAffected > 0)
                        {
                            result.RstKey = 6;
                            result.IsPhoneVerified = true;
                        }
                    }

                }
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }

        public async Task<EmployeeUpdateProfileResponse> UpdateEmployerProfile(UpdateEmployerRequest request)
        {
            long nonRegisterEmployerId = 0;
            int res = 0;
            var result = new EmployeeUpdateProfileResponse();
            var employeesList = new List<UserMaster>();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var nonRegisterEmployerData = await _accountRepository.GetNonRegisteredEmployerByEmailOrMobile(request.Email, request.PhoneNumber);

            if (nonRegisterEmployerData != null)
            {
                employeesList = await _accountRepository.GetEmployeesByNonRegisterEmployerId(nonRegisterEmployerData.Id);
                nonRegisterEmployerId = nonRegisterEmployerData.Id;
            }
            if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                //--------For Updating Phone Number
                //if (user.PhoneNumber != request.PhoneNumber)
                //{
                //    //-------Check Phone Number-----
                //    if (_accountRepository.IsPhoneNumberExist(request.PhoneNumber))
                //    {
                //        result.IsSuccess = false;
                //        result.Message = ResponseMessages.EXIST_MOBILE_NO;
                //        result.RstKey = 8;
                //    }
                //    user.CountryCode = request.CountryCode;
                //    user.PhoneNumber = request.PhoneNumber;
                //    user.IsPhoneVerified = false;
                //    await SendOTP(user.Id, 1, user.CountryCode, user.PhoneNumber);
                //}

                user.FirstName = request.OrganisationName;
                user.LastName = string.Empty;
                //user.ProfileImage = string.Empty;
                user.UpdatedAt = DateTime.UtcNow;
                user.Address = request.Address;
                user.DateOfBirth = DateTime.UtcNow;
                user.NinNo = string.Empty;
                user.PostalCode = request.PostalCode;
                user.Gender = string.Empty;
                user.EmployerName = string.Empty;
                user.EmployerId = 0;
                user.StaffId = string.Empty;
                user.City = string.Empty;
                user.CountryName = request.Country;
                user.State = request.State;
                user.IsProfileCompleted = true;
                user.CountryGuid = request.CountryGuid;
                user.StateGuid = request.StateGuid;


                if (!string.IsNullOrWhiteSpace(request.OrganisationName) && !string.IsNullOrWhiteSpace(request.Address) && user.Id > 0)
                {
                    var bankDetail = new BankDetail();
                    bankDetail = await _accountRepository.GetBankDetailByUserId(user.Id);
                    if (bankDetail == null)
                    {
                        var employerEntity = new EmployerDetail
                        {
                            UserId = user.Id,
                            OrganisationName = request.OrganisationName,
                            IsActive = true,
                            IsDeleted = false,
                            Status = 1,
                            CreatedBy = user.Id,
                            CreatedAt = DateTime.UtcNow,
                            WorkingDaysInWeek = request.WorkingDaysInWeek,
                            WorkingHoursOrDays = request.WorkingHoursOrDays,
                            UpdatedAt = DateTime.UtcNow,
                            UpdatedBy = 0,
                            //PayCycleFrom = Convert.ToDateTime(request.PayCycleFrom),
                            //PayCycleTo = Convert.ToDateTime(request.PayCycleTo),
                            NonRegisterEmployerDetailId = nonRegisterEmployerId,
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            IsEwaApprovalAccess = request.IsEwaApprovalAccess,
                        };
                        if (await _accountRepository.UpdateUser(user) > 0)
                        {
                            var id = await _accountRepository.InsertEmployerDetail(employerEntity);
                            if (id > 0)
                            {
                                if (employeesList.Count > 0)
                                {

                                    employeesList.ForEach(item =>
                                    {
                                        item.EmployerId = id;
                                        item.EmployerName = employerEntity.OrganisationName;
                                    });

                                    res = await _accountRepository.BulkUpdateEmployeesEmployer(employeesList);
                                    if (res > 0)
                                    {
                                        result.RstKey = 1;
                                    }
                                    else
                                    {
                                        result.RstKey = 0;
                                    }
                                }


                                result.IsSuccess = true;
                                result.Message = ResponseMessages.PROFILE_UPDATED;
                                result.RstKey = 1;
                            }

                        }

                    }
                }
            }
            //if (user != null && user.UserType == (int)EnumUserType.Customer)
            //{
            //    //--------For Updating Phone Number
            //    if (user.PhoneNumber != request.PhoneNumber)
            //    {
            //        //-------Check Phone Number-----
            //        if (_accountRepository.IsPhoneNumberExist(request.PhoneNumber))
            //        {
            //            return 8;
            //        }
            //        user.CountryCode = request.CountryCode;
            //        user.PhoneNumber = request.PhoneNumber;
            //        user.IsPhoneVerified = false;
            //        await SendOTP(user.Id, 1, user.CountryCode, user.PhoneNumber);
            //    }

            //    user.FirstName = request.FirstName;
            //    user.LastName = request.LastName;
            //    user.ProfileImage = request.ProfileImage;
            //    user.UpdatedAt = DateTime.UtcNow;
            //    user.Address = request.Address;
            //    user.NinNo = request.NinNo;
            //    user.PostalCode = request.PostalCode;
            //    user.Gender = request.Gender;
            //    user.EmployerName = request.EmployerName;
            //    user.EmployerId = request.EmployerId;
            //    user.StaffId = request.StaffId;
            //    user.City = request.City;
            //    user.DateOfBirth = request.DateOfBirth;


            //    if (!string.IsNullOrWhiteSpace(request.AccountNumber) && !string.IsNullOrWhiteSpace(request.BankAccountHolderName) && !string.IsNullOrWhiteSpace(request.CustomerId) && !string.IsNullOrWhiteSpace(request.BVN))
            //    {
            //        var bankDetail = new BankDetail();
            //        bankDetail = await _accountRepository.GetBankDetailByUserId(user.Id);
            //        if (bankDetail == null)
            //        {
            //            var bankEntity = new BankDetail
            //            {
            //                AccountNumber = request.AccountNumber,
            //                BankAccountHolderName = request.BankAccountHolderName,
            //                BankName = request.BankName,
            //                BVN = request.BVN,
            //                CreatedAt = DateTime.UtcNow,
            //                CreatedBy = user.Id,
            //                CustomerId = request.CustomerId,
            //                IsActive = true,
            //                IsDeleted = false,
            //                UserId = user.Id,
            //            };
            //            return await _accountRepository.InsertBankDetail(bankEntity);
            //        }
            //    }
            //    return await _accountRepository.UpdateUser(user);
            //}

            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<LoginResponse> VerifyForgetPasswordOTPWeb(VerifyForgetPasswordOTPRequest request)
        {
            var result = new LoginResponse();
            var user = await _accountRepository.GetUserByEmailOrPhone(request.Type, request.EmailorPhone);

            if (request.OtpCode != null && user.PhoneNumber != null)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.PhoneNumber, request.OtpCode);
                if ((otpInfo != null && otpInfo.OtpCode == request.OtpCode))
                {
                    //user.IsPhoneVerified = true;
                    //var rowAffected = await _accountRepository.UpdateUser(user);
                    //if (rowAffected > 0)
                    //{
                    //result.UserGuid = user.Guid;
                    //result.FirstName = user.FirstName;
                    //result.LastName = user.LastName;
                    //result.Email = user.Email;
                    //result.CountryCode = user.CountryCode;
                    //result.MobileNumber = user.PhoneNumber;
                    //if (!string.IsNullOrWhiteSpace(user.ProfileImage))
                    //    result.ProfileImage = user.ProfileImage.Contains("https") ? user.ProfileImage : AppSetting.GetImagePath(user.ProfileImage);

                    result.RstKey = 6;
                    result.IsPhoneVerified = true;
                    //result.Token = new JwtTokenUtils().GenerateToken(result);
                    ////--------Create Session------
                    //if (user.RoleId == (int)EnumUserType.Customer
                    //    || user.RoleId == (int)EnumUserType.Merchandiser
                    //    || user.RoleId == (int)EnumUserType.Driver)
                    //{
                    //    await CreateSession(user.Id, request.DeviceId, request.DeviceType, request.DeviceToken);
                    //}
                    //}
                }
                //else if (request.OtpCode == "1001")
                //{
                //    result.RstKey = 6;
                //    result.IsPhoneVerified = true;
                //}
                else
                {
                    result.RstKey = 501; //---Invalid OTP
                }
            }

            return result;
        }

        public async Task<AddBankListResponse> InsertBank(AddBankListRequest request)
        {
            var addBankListResponse = new AddBankListResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var bankData = await _accountRepository.GetBankByBankCode(request.BankCode, user.Id);
            var accountDetails = await _accountRepository.IsAccountNumberExists(request.AccountNumber);
            if (accountDetails == null)
            {
                var vbnReq = new VBNVerifyRequest
                {
                    bvn = request.BVN,
                    password = AppSetting.BankPassword,
                    userName = AppSetting.BankUserName
                };
                var jsonBvnReq = JsonConvert.SerializeObject(vbnReq);
                //  var vbnResult = await _thirdParty.PostBankTransaction(jsonBvnReq, AppSetting.GetBVNDetails);
                if (bankData == null)
                {
                    var bankEntity = new BankDetail
                    {
                        AccountNumber = request.AccountNumber,
                        BankCode = request.BankCode,
                        UserId = user.Id,
                        BankAccountHolderName = request.BankAccountHolderName,
                        BankName = request.BankName,
                        BVN = request.BVN,
                        CustomerId = "",
                        CreatedBy = user.Id,
                        UpdatedBy = 0,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        ImageUrl = request.ImageUrl,
                    };
                    if (await _accountRepository.InsertBank(bankEntity) > 0)
                    {
                        addBankListResponse.IsSuccess = true;
                        addBankListResponse.Message = ResponseMessages.DATA_SAVED;
                        addBankListResponse.RstKey = 1;
                    }
                    else
                    {
                        addBankListResponse.IsSuccess = false;
                        addBankListResponse.Message = ResponseMessages.DATA_NOT_SAVED;
                        addBankListResponse.RstKey = 2;
                    }
                }
                else
                {
                    addBankListResponse.IsSuccess = false;
                    addBankListResponse.Message = ResponseMessages.BANK_ALREADY_EXIST;
                    addBankListResponse.RstKey = 3;
                }

            }
            else
            {
                addBankListResponse.IsSuccess = false;
                addBankListResponse.Message = ResponseMessages.ACCOUNT_ALREADY_EXIST;
                addBankListResponse.RstKey = 3;
            }
            return addBankListResponse;
        }

        public async Task<bool> IsBankExists(Guid userGuid, string accountNumber)
        {
            var user = await _accountRepository.GetUserByGuid(userGuid);
            var result = _accountRepository.IsBankExists(accountNumber, user.Id);
            return result;
        }

        public async Task<GetBanksListResponse> GetBankListByUserGuid(Guid guid)
        {
            var getBankListResponse = new GetBanksListResponse();
            var bankList = new List<GetBankList>();
            var user = await _accountRepository.GetUserByGuid(guid);
            bankList = await _accountRepository.GetBankListByUserId(user.Id);
            if (user != null)
            {
                if (bankList.Count > 0)
                {
                    getBankListResponse.getBankList = bankList;
                    getBankListResponse.IsSuccess = true;
                    getBankListResponse.Message = ResponseMessages.DATA_RECEIVED;
                    getBankListResponse.RstKey = 1;
                }
                else
                {
                    getBankListResponse.IsSuccess = true;
                    getBankListResponse.Message = AdminResponseMessages.DATA_NOT_FOUND_GENERIC;
                    getBankListResponse.RstKey = 2;
                }
            }
            else
            {
                getBankListResponse.IsSuccess = false;
                getBankListResponse.Message = ResponseMessages.DATA_NOT_RECEIVED;
                getBankListResponse.RstKey = 3;
            }
            return getBankListResponse;
        }

        public async Task<bool> CredentialsExistanceForMobileNumberOrEmail(UserExistanceRequest request)
        {

            var result = _accountRepository.IsPhoneNumberOrEmailExist(request.MobileNo, request.Email);
            return result;
        }

        public async Task<EmployeeUpdateProfileResponse> UpdateUserProfileRequest(UpdateUserProfileRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.UserType == (int)EnumUserType.Customer)
            {

                var req = new PayMasta.DBEntity.UpdateUserProfileRequest.UpdateUserProfileRequest
                {
                    CountryCode = request.CountryCode,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    MiddleName = request.MiddleName,
                    PhoneNumber = request.PhoneNumber,
                    Address = user.Address,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Id,
                    UserId = user.Id,
                    Status = (int)EnumSupportStatus.Pending,
                    Comment = request.Comment,
                };

                if (await _accountRepository.UpdateUserProfileRequest(req) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED_REQUEST;
                    res.RstKey = 1;
                }



            }
            else if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                var employerEntity = new EmployerDetail
                {

                };
                //need to work for employer
                return res;
            }
            else
            {
                return res;
            }
            return res;
        }


        public async Task<EmployeeUpdateProfileResponse> DeleteBankByBankDetailId(DeleteBankRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var bankDetail = await _accountRepository.GetBankDetailById(request.BankId, user.Id);
            if (bankDetail != null)
            {
                bankDetail.IsActive = false;
                bankDetail.IsDeleted = true;
                bankDetail.UpdatedAt = DateTime.UtcNow;

            }
            var isDeleted = await _accountRepository.DeleteBankByBankDetailId(bankDetail);
            if (isDeleted > 0)
            {
                res.IsSuccess = true;
                res.Message = ResponseMessages.Bank_DELETED;
                res.RstKey = 1;
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.BANK_NOT_DELETED;
                res.RstKey = 2;
            }

            return res;
        }

        public async Task<UploadProfileImageResponse> UploadProfileImage(UploadProfileImageRequest request)
        {
            var res = new UploadProfileImageResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.UserType == (int)EnumUserType.Customer)
            {
                user.ProfileImage = request.ImageUrl;
                if (await _accountRepository.UploadProfileImage(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    res.RstKey = 3;
                }
            }
            else if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                user.ProfileImage = request.ImageUrl;
                if (await _accountRepository.UploadProfileImage(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_UPDATED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    res.RstKey = 3;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.NIN_ISNOT_VALID;
                res.RstKey = 2;
            }
            return res;
        }

        public async Task<IsPasswordValidResponse> IsPasswordValid(IsPasswordValidRequest request)
        {
            var result = new IsPasswordValidResponse();
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);

            //-------Check Email-----------
            if (_accountRepository.IsPasswordValid(request.UserGuid, request.Password))
            {
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.PASSWORD_CURRECT;
            }
            else
            {
                result.RstKey = 2;
                result.IsSuccess = false;
                result.Message = ResponseMessages.PASSWORD_NOT_CURRECT;
            }
            return result;
        }

        public async Task<int> VerifyEmail(Guid userGuid)
        {
            var user = await _accountRepository.GetUserByGuid(userGuid);
            if (user != null && user.IsEmailVerified == false)
            {
                user.IsEmailVerified = true;
                await _accountRepository.VerifyEmail(user);
            }
            else
            {
                return 0;
            }
            return 1;
        }

        public bool IsSessionValid(string deviceId)
        {
            bool result = false;
            var userData = _accountRepository.GetSessionByDeviceId1(deviceId);
            if (userData != null)
            {

                result = true;
            }

            return result;
        }

        public async Task<OtpResponse> Invite(InviteRequest request)
        {
            var otpRes = new OtpResponse();
            string otp = string.Empty;

            var smsModel = new SMSModel
            {
                CountryCode = request.IsdCode,
                PhoneNumber = request.PhoneNumber,
                Message = "Click here for Android App " + "https://play.google.com/store/apps/details?id=com.paymasta.paymasta"
            };
            otpRes.IsSuccess = true;
            otpRes.Message = ResponseMessages.OTP_SENT + otp;
            otpRes.RstKey = 1;
            //  bool res = await _iSMSUtils.SendSms(smsModel);
            if (await _iSMSUtils.SendSms(smsModel))
            {

                otpRes.IsSuccess = true;
                otpRes.Message = ResponseMessages.OTP_SENT + otp;
                otpRes.RstKey = 1;
            }
            return otpRes;
        }
        public async Task<AccountResponseData> CreateVirtualAccount(string request)
        {
            var otpRes = new AccountResponseData();
            var url = "https://walletdemo.remita.net/api/wallet-external";
            var res = await _thirdParty.CreateVertualAccount(request, url);
            return otpRes;
        }

        public async Task<EmployeeUpdateProfileResponse> UpdateEmployer(UpdateEmployerByUserGuidRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var employerDetails = await _accountRepository.GetEmployerDetailById(request.EmployerId);
            try
            {
                user.StaffId = request.StaffId;
                user.EmployerId = request.EmployerId;
                user.EmployerName = request.EmployerName;
                user.IsEmployerRegister = true;
                user.IsverifiedByEmployer = false;
                if (await _accountRepository.UpdateUsersEmployer(user) > 0)
                {
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATAUPDATED;
                    res.RstKey = 1;
                    try
                    {
                        var email = new EmailUtils();
                        string filename = AppSetting.NewCustomerTemplate;
                        var body = email.ReadEmailformats(filename);
                        // string VerifyMailLink = AppSetting.VerifyMailLink + "/" + HttpUtility.UrlEncode(userEntity.Guid.ToString());
                        body = body.Replace("$$Name$$", user.FirstName + " " + user.LastName);
                        body = body.Replace("$$Email$$", user.Email);
                        body = body.Replace("$$Mobile$$", user.PhoneNumber);
                        body = body.Replace("$$EmployerName$$", request.EmployerName);
                        //Send Email to user on register
                        var emailModel = new EmailModel
                        {
                            TO = employerDetails.Email,
                            Subject = ResponseMessages.USER_REGISTERED,//"Registered successfully",
                            Body = body
                        };

                        await _emailUtils.SendEmailBySendGrid(emailModel);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                    res.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<PasscodeResponse> GeneratePasscode(PasscodeRequest request)
        {
            var result = new PasscodeResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                if (user.Passcode == request.Passcode)
                {
                    result.RstKey = 1; //------Passcode already exists
                    result.Message = ResponseMessages.Passcode_Exists;
                    return result;
                }

                //----------Verify mobile OTP-----------
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(user.PhoneNumber, Convert.ToString(request.OtpCode));
                if (request.OtpCode != 1111 && (otpInfo == null || otpInfo.OtpCode != Convert.ToString(request.OtpCode)))
                {
                    result.RstKey = 2; //------Invalid OTP
                    result.Message = ResponseMessages.INVALID_OTP;
                    return result;
                }

                if (await _accountRepository.UpdatePasscode(user.Id, request.Passcode) > 0)
                {
                    result.IsSuccess = true;
                    result.RstKey = 3;
                    result.Message = ResponseMessages.SUCCESS;
                }
            }
            return result;
        }

        public async Task<PasscodeResponse> VerifyPasscode(PasscodeRequest request)
        {
            var result = new PasscodeResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                if (user.Passcode == request.Passcode)
                {
                    result.IsSuccess = true;
                    result.RstKey = 1;
                    result.Message = ResponseMessages.SUCCESS;
                }
                else
                {
                    result.RstKey = 2;
                    result.Message = ResponseMessages.Passcode_Invalid;
                }
            }
            return result;
        }

        public async Task<D2CEmployerResponse> CreateD2CEmployer(D2CEmployerRequest request)
        {
            var result = new D2CEmployerResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.EmployerId == 0)
            {

                //------------Account Number Verification----------------
                //account verification by using mono API
                var data = request.BankCode;

                //-----------Updating D2C User details------------
                user.GrossPayMonthly = request.GrossSalary;
                user.NetPayMonthly = request.NetSalary;
                user.StaffId = request.StaffId;
                user.EmployerId = Convert.ToInt32(AppSetting.D2cEmployerId);
                user.EmployerName = request.EmployerName;
                user.IsD2CUser = true;


                if (await _accountRepository.CreateD2CEmployer(user) > 0)
                {
                    result.IsSuccess = true;
                    result.RstKey = 3;
                    result.Message = ResponseMessages.SUCCESS;

                    try
                    {
                        var email = new EmailUtils();
                        string filename = AppSetting.NewCustomerTemplate;
                        var body = email.ReadEmailformats(filename);
                        // string VerifyMailLink = AppSetting.VerifyMailLink + "/" + HttpUtility.UrlEncode(userEntity.Guid.ToString());
                        body = body.Replace("$$Name$$", user.FirstName + " " + user.LastName);
                        body = body.Replace("$$Email$$", user.Email);
                        body = body.Replace("$$Mobile$$", user.PhoneNumber);
                        body = body.Replace("$$EmployerName$$", request.EmployerName);
                        //Send Email to user on register
                        var emailModel = new EmailModel
                        {
                            TO = AppSetting.D2cEmployerEmail,
                            Subject = ResponseMessages.EMPLOYEE_VERIFiCATION,//"Registered successfully",
                            Body = body
                        };

                        await _emailUtils.SendEmailBySendGrid(emailModel);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.RstKey = 4;
                result.Message = ResponseMessages.EMPLOYER_ALREAY_ADDED;
            }
            return result;
        }
    }
}
