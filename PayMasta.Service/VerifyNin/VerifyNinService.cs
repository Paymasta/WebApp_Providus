using PayMasta.Repository.Account;
using PayMasta.Repository.Employer.CommonEmployerRepository;
using PayMasta.Repository.VirtualAccountRepository;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.VirtualAccount;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayMasta.DBEntity.BankDetail;
using PayMasta.Utilities.Common;
using PayMasta.ViewModel;
using PayMasta.ViewModel.VerifyNinVM;
using Newtonsoft.Json;
using PayMasta.ViewModel.Common;
using System.Data.Common;
using Microsoft.Office.Interop.Word;
using System.IO;
using PayMasta.DBEntity.Account;
using PayMasta.ViewModel.Enums;
using NPOI.SS.Formula.Functions;
using System.Collections.ObjectModel;
using PayMasta.Repository.Extention;
using System.Web;
using PayMasta.ViewModel.OtherDetailVM;
using System.Text.RegularExpressions;
using System.Globalization;
using PayMasta.ViewModel.VoterIdVM;
using PayMasta.ViewModel.VNINVM;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.Account;

namespace PayMasta.Service.VerifyNin
{
    public class VerifyNinService : IVerifyNinService
    {
        private readonly IThirdParty _thirdParty;
        private readonly IAccountRepository _accountRepository;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IVirtualAccountService _virtualAccountService;
        private readonly IProvidusExpresssWalletService _providusExpresssWalletService;
        private readonly IAccountService _accountService;
        public VerifyNinService()
        {
            _accountRepository = new AccountRepository();
            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _virtualAccountService = new VirtualAccountService();
            _providusExpresssWalletService = new ProvidusExpresssWalletService();
            _accountService = new AccountService();
            //_commonEmployerRepository = new CommonEmployerRepository();
            //_virtualAccountRepository = new VirtualAccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        //public async Task<VerifyResponse> VerifyMe(VerifyRequest request)
        //{
        //    //  var user = await _accountRepository.GetUserByGuid(request.UserGuid);
        //    var result = new VerifyResponse();

        //    var req = new VninVerifyRequest
        //    {
        //        idNumber = request.VninNumber,
        //        dob = request.DOB,
        //        firstname = request.Firstname,
        //        gender = request.Gender,
        //        lastname = request.Lastname,
        //        phone = request.Phone,
        //    };
        //    var jsonData = JsonConvert.SerializeObject(req);
        //    string Url = AppSetting.vNinVerifyUrl + request.VninNumber;
        //    var response = await _thirdParty.VNinVerification(jsonData, Url);
        //    var data = JsonConvert.DeserializeObject<VninVerifyResponse>(response);
        //    if (data.status != null && data.status.ToLower() == "success".ToLower())
        //    {
        //        //  await _accountRepository.UpdateUser(user);

        //        result.vninVerifyResponses = data;
        //        result.RstKey = 1;
        //        result.IsSuccess = true;
        //        result.Message = ResponseMessages.SUCCESS;
        //    }
        //    else
        //    {
        //        var errorData = JsonConvert.DeserializeObject<ErrorResponse>(response);
        //        result.RstKey = 2;
        //        result.IsSuccess = false;
        //        result.Message = errorData.message;
        //    }
        //    return result;
        //}

        public async Task<QoreIdBvnNubanResult> VerifyNuban(RegisterByNubanRequest request)
        {
            var result = new QoreIdBvnNubanResult();
            try
            {
                //-------Check Email-----------
                if (_accountRepository.IsEmailExist(request.Email))
                {
                    result.RstKey = 7;
                    return result;
                }
                if (_accountRepository.IsAccountNumberExist(request.AccountNumber))
                {
                    result.RstKey = 9;
                    return result;
                }
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdBvnNubanUrl;
                    var req = new QoreIdBvnNubanRequest
                    {
                        accountNumber = request.AccountNumber,
                        bankCode = request.BankCode,
                        firstname = request.Firstname,
                        lastname = request.Lastname,
                    };
                    var jsonReq = JsonConvert.SerializeObject(req);
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<QoreIdBvnNubanResponse>(res);
                    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    {
                        result = await Signup(request, JsonRes);
                        if (result.IsSuccess == true)
                        {
                            //result.vninVerifyResponses = JsonRes;
                            result.IsSuccess = true;
                            // result.RstKey = 1;
                            result.Message = ResponseMessages.USER_REGISTERED;
                        }

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.Message = ResponseMessages.USER_NOT_REGISTERED;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                    result.Message = ResponseMessages.TRANSACTION_NULL_ERROR;
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.RstKey = 2;
                result.Message = ResponseMessages.USER_NOT_REGISTERED;
            }
            return result;
        }

        public async Task<BankListResult> BankListForRegister()
        {

            var result = new BankListResult();
            try
            {
                string path = AppSetting.BankListFile;
                string file;
                using (var reader = new StreamReader(path))
                {
                    file = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<BankListResponse>(file);
                if (data != null)
                {
                    result.bankListResponse = data;
                    result.IsSuccess = true;
                    result.RstKey = 1;
                    result.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                    result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<QoreIdAuthResponse> AuthTokenForQoreId()
        {

            var result = new QoreIdAuthResponse();
            try
            {
                var url = AppSetting.QoreIdAuthTokenUrl;
                string authToken = string.Empty;
                var req = new QoreIdAuthRequest
                {
                    clientId = AppSetting.QoreIdClientId,
                    secret = AppSetting.QoreIdSecretKey,
                };
                var jsonReq = JsonConvert.SerializeObject(req);
                var res = await _thirdParty.QoreIdAuthToken(jsonReq, url);
                var obj = JsonConvert.DeserializeObject<QoreIdAuthResponse>(res);
                if (obj.accessToken != null)
                {
                    result.accessToken = obj.accessToken;
                    result.expiresIn = obj.expiresIn;
                    result.tokenType = obj.tokenType;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        private async Task<QoreIdBvnNubanResult> Signup(RegisterByNubanRequest request, QoreIdBvnNubanResponse JsonRes)
        {
            string customer = string.Empty;
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            var result = new QoreIdBvnNubanResult();
            try
            {
                int userType = 0;
                if (request.RoleId == (int)EnumUserType.Customer)
                {
                    userType = (int)EnumUserType.Customer;
                }
                else if (request.RoleId == (int)EnumUserType.Employer)
                {
                    userType = (int)EnumUserType.Employer;
                }

                string customerNumber = request.PhoneNumber.Substring(0, 1);
                if (customerNumber != "0")
                {
                    customer = "0" + request.PhoneNumber;
                    //request.PhoneNumber = customer;
                }
                else
                {
                    customer = request.PhoneNumber; ;
                    // request.PhoneNumber = customer;
                }
                //-------Check Phone Number-----
                if (_accountRepository.IsPhoneNumberExist(customer))
                {
                    result.RstKey = 8;
                    return result;
                }
                //-------Check Phone Number-----
                if (_accountRepository.IsAccountNumberExist(request.AccountNumber))
                {
                    result.RstKey = 9;
                    return result;
                }
                using (var dbConnection = Connection)
                {
                    //dbConnection.Open();
                    //using (var transaction = dbConnection.BeginTransaction())
                    //{
                    var userMaste = new UserMaster
                    {
                        FirstName = JsonRes.bvn_nuban.firstname,
                        LastName = JsonRes.bvn_nuban.lastname,
                        MiddleName = JsonRes.bvn_nuban.middlename,
                        DateOfBirth = Convert.ToDateTime(JsonRes.bvn_nuban.birthdate),
                        Gender = JsonRes.bvn_nuban.gender,
                        PhoneNumber = customer,
                        Email = request.Email,
                        NinNo = string.Empty,
                        CountryCode = request.CountryCode,
                        IsActive = true,
                        IsDeleted = false,
                        IsEmailVerified = false,
                        IsPhoneVerified = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsPayMastaCardApplied = request.IsCardChecked,
                        IsProfileCompleted = false,
                        IsVerified = true,
                        IsvertualAccountCreated = false,
                        Password = request.Password,
                        Status = 1,
                        WalletBalance = 0,
                        UserType = userType,
                        CreatedBy = 0,
                        UpdatedBy = 0,
                        IsGuestUser = false,
                        IsverifiedByEmployer = false,
                    };
                    var userData = await _accountRepository.InsertUserFromNubanRegister(userMaste, dbConnection);
                    if (userData.Id > 0)
                    {
                        var bankDetail = new BankDetail
                        {
                            AccountNumber = request.AccountNumber,
                            BankAccountHolderName = JsonRes.bvn_nuban.firstname + " " + JsonRes.bvn_nuban.lastname,
                            BankCode = request.BankCode,
                            BankName = request.BankName,
                            BVN = JsonRes.bvn_nuban.bvn,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userData.Id,
                            IsSalaryAccount = false,
                            UserId = userData.Id,
                            IsActive = true,
                            IsDeleted = false,
                            CustomerId = string.Empty,
                        };
                        if (await _accountRepository.InsertBank(bankDetail, dbConnection) > 0)
                        {
                            // transaction.Commit();
                            result.RstKey = 111;
                            result.IsSuccess = true;
                            var otpReq = new OtpRequest
                            {
                                IsdCode = request.CountryCode,
                                MobileNo = customer,
                                Email = request.Email,
                            };
                            await SendOTP(otpReq, userData.Id, request.Email);

                            if (userData.Id > 0)
                            {
                                // await SendOTP(userEntity.Id, (int)EnumOtpType.SignUp, request.CountryCode, request.PhoneNumber);
                                result.UserGuid = userData.Guid;
                                // result.IsEmailVerified = userEntity.IsEmailVerified;
                                result.IsPhoneVerified = userData.IsPhoneVerified;
                                result.UserId = userData.Id;
                                result.CountryCode = userData.CountryCode;
                                result.MobileNumber = userData.PhoneNumber;
                                result.Email = userData.Email;
                                result.RoleId = userData.UserType;
                                result.RstKey = 11;
                                result.IsProfileCompleted = userData.IsProfileCompleted;
                                EmailUtils email = new EmailUtils();
                                try
                                {
                                    string filename = AppSetting.EmailVerificationTemplate;
                                    var body = email.ReadEmailformats(filename);
                                    string VerifyMailLink = AppSetting.VerifyMailLink + "/" + HttpUtility.UrlEncode(userData.Guid.ToString());
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



                                var req = new LoginResponse { UserId = userData.Id, FirstName = userData.FirstName, LastName = userData.LastName, UserGuid = userData.Guid, MobileNumber = userData.PhoneNumber };
                                result.Token = new JwtTokenUtils().GenerateToken(req);
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                                        await CreateSession(userData.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                                }
                                catch (Exception ex)
                                {
                                    //"Card Payment Success".ErrorLog("CardPaymentController.cs", "PaymentResponse", request);
                                    ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                                }
                            }
                        }
                        else
                        {
                            //transaction.Rollback();
                            result.RstKey = 112;
                            result.IsSuccess = false;
                        }
                    }
                    else
                    {
                        result.RstKey = 113;
                        result.IsSuccess = false;
                    }
                    //  }
                    //dbConnection.Close();
                }


            }
            catch (Exception ex)
            {
                result.RstKey = 114;
                result.IsSuccess = false;
            }
            return result;
        }

        private async Task<OtpResponse> SendOTP(OtpRequest request, long userId, string email)
        {
            var otpRes = new OtpResponse();
            string otp = string.Empty;
            var otpInfoEntity = new OtpInfo
            {
                OtpCode = CommonUtils.GenerateOtp(),
                CountryCode = request.IsdCode,
                PhoneNumber = request.MobileNo,
                Email = email,
                UserId = userId,
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

        public async Task<IsOtpVerifiedResponse> VerifyOTPWeb(VerifyOTPRequest request)
        {
            var result = new IsOtpVerifiedResponse();
            string customer = string.Empty;
            // var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            string customerNumber = request.MobileNumber.Substring(0, 1);
            if (customerNumber != "0")
            {
                customer = "0" + request.MobileNumber;
                //request.PhoneNumber = customer;
            }
            else
            {
                customer = request.MobileNumber; ;
                // request.PhoneNumber = customer;
            }
            if (request.OtpCode != null && request.MobileNumber != null)
            {
                var otpInfo = await _accountRepository.GetOtpInfoByUserId(customer, request.OtpCode);
                var user = await _accountRepository.GetUserByMobile(customer);
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
                        else
                        {
                            result.RstKey = 501;
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

        public async Task<AddOtherDetailResponse> AddUserOtherDetail(AddOtherDetailPRequest request)
        {
            var result = new AddOtherDetailResponse();
            var user = new UserMaster();
            bool isUserVerified = false;
            try
            {
                user = await _accountRepository.GetUserByGuid(request.UserGuid);
                var bankdetail = new BankDetail();
                var dlResponse = new DLVerificationResponse();
                var voterIdResponse = new DLVerificationResponse();
                var passportResponse = new DLVerificationResponse();
                var vNINResponse = new DLVerificationResponse();
                if (user != null && string.IsNullOrEmpty(user.Address))
                {
                    if (request.UserVerificationType.ToUpper() == "DL")
                    {
                        //request.VerificationId = "KJA47543AA01";
                        var dlReq = new DLVerificationRequest
                        {
                            firstname = user.FirstName,
                            lastname = user.LastName,
                        };
                        isUserVerified = await VerifyDL(dlReq, request.VerificationId);
                    }
                    if (request.UserVerificationType.ToUpper() == "VC")
                    {
                        IFormatProvider culture = new CultureInfo("en-US", true);
                        var dlReq = new VCVerificationRequest
                        {

                            firstname = user.FirstName,
                            lastname = user.LastName,
                            //dob = DateTime.ParseExact(user.DateOfBirth.ToString(), "yyyy-MM-dd", culture).ToString(),
                            dob = user.DateOfBirth.ToString("yyyy-MM-dd"),
                        };
                        isUserVerified = await VerifyVC(dlReq, request.VerificationId);
                    }
                    if (request.UserVerificationType.ToUpper() == "VNIN")
                    {
                        //var dlReq = new DLVerificationRequest
                        //{
                        //    firstname = user.FirstName,
                        //    lastname = user.LastName,
                        //};
                        //isUserVerified = await VerifyVNIN(dlReq, request.VerificationId);
                        //var reqNin = new NINNumberVerifyRequest
                        //{
                        //    dob = user.DateOfBirth.ToString("dd-MM-yyyy"),
                        //    firstname = user.FirstName,
                        //    lastname = user.LastName,
                        //    NINNumber = request.VerificationId,
                        //    phone = user.PhoneNumber,
                        //};
                        //var ninRes = await _accountService.NinVerify(reqNin);
                        //if (ninRes.RstKey == 1)
                        //{
                        //    isUserVerified = true;
                        //}
                        //isUserVerified = true;
                        var dlReq = new DLVerificationRequest
                        {
                            firstname = user.FirstName,
                            lastname = user.LastName,
                        };
                        isUserVerified = await VerifyNIN(dlReq, request.VerificationId);
                    }
                    if (isUserVerified)
                    {
                        user.Address = request.Address;
                        user.NinNo = request.VerificationId;
                        user.State = request.State;
                        user.CountryGuid = request.CountryGuid;
                        user.StateGuid = request.StateGuid;
                        user.CountryName = request.Countryname;
                        user.City = request.city;
                        user.PostalCode = request.PostalCode;
                        user.IsProfileCompleted = true;
                        user.VerificationType = request.UserVerificationType;
                        if (await _accountRepository.UpdateUserOtherDetail(user) > 0)
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.Message = ResponseMessages.PROFILE_UPDATED;

                            if (user != null)
                            {
                                bankdetail = await _accountRepository.GetBankDetailByUserId(user.Id);
                            }
                            user = await _accountRepository.GetUserByGuid(request.UserGuid);
                            if (user != null && user.IsvertualAccountCreated == false && bankdetail != null && user.IsProfileCompleted == true && !string.IsNullOrWhiteSpace(user.Address) && !string.IsNullOrWhiteSpace(user.State))
                            {
                                try
                                {
                                    if (await _providusExpresssWalletService.CreateVirtualAccount(user.Guid))
                                    {
                                        user.IsvertualAccountCreated = true;
                                        await _accountRepository.UpdateVirtualAccountStatus(user);
                                    }
                                    //var adminKeyPair = AES256.AdminKeyPair;
                                    //int _min = 0000;
                                    //int _max = 9999;
                                    //Random _rdm = new Random();
                                    //var pin = _rdm.Next(_min, _max);
                                    //var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, user.Password);
                                    //if (await _virtualAccountService.CreateVirtualAccount(user.FirstName, user.LastName,
                                    //                  bankdetail.BVN, user.Id, user.Address, user.DateOfBirth, user.Email,
                                    //                  user.Gender, user.CountryCode + user.PhoneNumber, "", user.Address, "", accountPassword, pin.ToString(),
                                    //                  user.State))
                                    //{
                                    //    user.IsvertualAccountCreated = true;
                                    //    await _accountRepository.UpdateVirtualAccountStatus(user);
                                    //    user.WalletPin = pin;
                                    //    await _accountRepository.UpdateVirtualAccountPin(user);
                                    //}

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.RstKey = 2;
                            result.Message = ResponseMessages.PROFILE_NOT_UPDATED;
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 3;
                        result.Message = ResponseMessages.OTHER_DETAIL_NOT_UPDATED;
                    }


                }
            }
            catch (Exception ex)
            {
                //result.RstKey = 10;
            }
            return result;
        }

        public async Task<QoreIdBvnNubanResult> VerifyVnin(RegisterByVNinRequest request)
        {
            var result = new QoreIdBvnNubanResult();
            try
            {
                //-------Check Email-----------
                if (_accountRepository.IsEmailExist(request.Email))
                {
                    result.RstKey = 7;
                    return result;
                }
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdVNninUrl + request.Vnin;
                    var req = new QoreIdVninRequest
                    {
                        firstname = request.Firstname,
                        lastname = request.Lastname,
                    };
                    var jsonReq = JsonConvert.SerializeObject(req);
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<QoreIdVninResponse>(res);
                    if (JsonRes.status.status.ToLower() == "verified".ToLower())
                    {
                        result = await Signup(request, JsonRes);
                        if (result.IsSuccess == true)
                        {
                            //result.vninVerifyResponses = JsonRes;
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.Message = ResponseMessages.USER_REGISTERED;
                        }

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.Message = ResponseMessages.USER_NOT_REGISTERED;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                    result.Message = ResponseMessages.TRANSACTION_NULL_ERROR;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<QoreIdBvnNubanResult> Signup(RegisterByVNinRequest request, QoreIdVninResponse JsonRes)
        {
            string customer = string.Empty;
            var adminKeyPair = AES256.AdminKeyPair;
            request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            var result = new QoreIdBvnNubanResult();
            try
            {
                int userType = 0;
                if (request.RoleId == (int)EnumUserType.Customer)
                {
                    userType = (int)EnumUserType.Customer;
                }
                else if (request.RoleId == (int)EnumUserType.Employer)
                {
                    userType = (int)EnumUserType.Employer;
                }

                string customerNumber = request.PhoneNumber.Substring(0, 1);
                if (customerNumber != "0")
                {
                    customer = "0" + request.PhoneNumber;
                    //request.PhoneNumber = customer;
                }
                else
                {
                    customer = request.PhoneNumber; ;
                    // request.PhoneNumber = customer;
                }
                //-------Check Phone Number-----
                if (_accountRepository.IsPhoneNumberExist(customer))
                {
                    result.RstKey = 8;
                    return result;
                }
                using (var dbConnection = Connection)
                {
                    //dbConnection.Open();
                    //using (var transaction = dbConnection.BeginTransaction())
                    //{
                    var userMaste = new UserMaster
                    {
                        FirstName = JsonRes.v_nin.firstname,
                        LastName = JsonRes.v_nin.lastname,
                        MiddleName = JsonRes.v_nin.middlename,
                        DateOfBirth = Convert.ToDateTime(JsonRes.v_nin.birthdate),
                        Gender = JsonRes.v_nin.gender,
                        PhoneNumber = customer,
                        Email = request.Email,
                        NinNo = string.Empty,
                        CountryCode = request.CountryCode,
                        IsActive = true,
                        IsDeleted = false,
                        IsEmailVerified = false,
                        IsPhoneVerified = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsPayMastaCardApplied = request.IsCardChecked,
                        IsProfileCompleted = true,
                        IsVerified = true,
                        IsvertualAccountCreated = false,
                        Password = request.Password,
                        Status = 1,
                        WalletBalance = 0,
                        UserType = userType,
                        CreatedBy = 0,
                        UpdatedBy = 0,
                        IsGuestUser = false,
                    };
                    var userData = await _accountRepository.InsertUserFromNubanRegister(userMaste, dbConnection);
                    if (userData.Id > 0)
                    {
                        var bankDetail = new BankDetail
                        {
                            AccountNumber = request.AccountNumber,
                            BankAccountHolderName = JsonRes.v_nin.firstname + " " + JsonRes.v_nin.lastname,
                            BankCode = request.BankCode,
                            BankName = request.BankName,
                            //  BVN = JsonRes.v_nin.bvn,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userData.Id,
                            IsSalaryAccount = false,
                            UserId = userData.Id,
                            IsActive = true,
                            IsDeleted = false,
                            CustomerId = string.Empty,
                        };
                        if (await _accountRepository.InsertBank(bankDetail, dbConnection) > 0)
                        {
                            // transaction.Commit();
                            result.RstKey = 111;
                            result.IsSuccess = true;
                            var otpReq = new OtpRequest
                            {
                                IsdCode = request.CountryCode,
                                MobileNo = customer,
                                Email = request.Email,
                            };
                            await SendOTP(otpReq, userData.Id, request.Email);

                            if (userData.Id > 0)
                            {
                                // await SendOTP(userEntity.Id, (int)EnumOtpType.SignUp, request.CountryCode, request.PhoneNumber);
                                result.UserGuid = userData.Guid;
                                // result.IsEmailVerified = userEntity.IsEmailVerified;
                                result.IsPhoneVerified = userData.IsPhoneVerified;
                                result.UserId = userData.Id;
                                result.CountryCode = userData.CountryCode;
                                result.MobileNumber = userData.PhoneNumber;
                                result.Email = userData.Email;
                                result.RoleId = userData.UserType;
                                result.RstKey = 11;
                                result.IsProfileCompleted = userData.IsProfileCompleted;
                                EmailUtils email = new EmailUtils();
                                try
                                {
                                    string filename = AppSetting.EmailVerificationTemplate;
                                    var body = email.ReadEmailformats(filename);
                                    string VerifyMailLink = AppSetting.VerifyMailLink + "/" + HttpUtility.UrlEncode(userData.Guid.ToString());
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



                                var req = new LoginResponse { UserId = userData.Id, FirstName = userData.FirstName, LastName = userData.LastName, UserGuid = userData.Guid, MobileNumber = userData.PhoneNumber };
                                result.Token = new JwtTokenUtils().GenerateToken(req);
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(request.DeviceId) && !string.IsNullOrWhiteSpace(request.DeviceToken))
                                        await CreateSession(userData.Id, request.DeviceId, request.DeviceType, request.DeviceToken, result.Token);
                                }
                                catch (Exception ex)
                                {
                                    //"Card Payment Success".ErrorLog("CardPaymentController.cs", "PaymentResponse", request);
                                    ex.Message.ErrorLog("AccountService.cs", "SignUpWithEmail", Connection);
                                }
                            }
                        }
                        else
                        {
                            //transaction.Rollback();
                            result.RstKey = 112;
                            result.IsSuccess = false;
                        }
                    }
                    else
                    {
                        result.RstKey = 113;
                        result.IsSuccess = false;
                    }
                    //  }
                    //dbConnection.Close();
                }


            }
            catch (Exception ex)
            {
                result.RstKey = 114;
                result.IsSuccess = false;
            }
            return result;
        }

        private async Task<bool> VerifyDL(DLVerificationRequest request, string VerificationId)
        {
            bool result = false;
            try
            {
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdDLVerificationURL + VerificationId;

                    var jsonReq = JsonConvert.SerializeObject(request);
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<DLVerificationResponse>(res);
                    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    {

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<bool> VerifyVC(VCVerificationRequest request, string VerificationId)
        {
            bool result = false;
            try
            {
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdVoterIdVerificationURL + VerificationId;

                    var jsonReq = JsonConvert.SerializeObject(request);
                    //below line is for response example
                    //"{\"id\":52693,\"applicant\":{\"firstname\":\"Olumide\",\"lastname\":\"Lawal\",\"dob\":\"1985-03-17\"},\"summary\":{\"voters_card_check\":{\"status\":\"EXACT_MATCH\",\"fieldMatches\":{\"firstname\":true,\"lastname\":true,\"dob\":false}}},\"status\":{\"state\":\"complete\",\"status\":\"verified\"},\"voters_card\":{\"fullname\":\"LawalOlumide\",\"vin\":\"90F5B1D80C528288265\",\"gender\":\"male\",\"occupation\":\"CivilServant\",\"pollingUnitCode\":\"24/05/05/002\",\"firstName\":\"Olumide\",\"lastName\":\"Lawal\"}}";
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<VoterIdResponse>(res);
                    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    {

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<bool> VerifyVNIN(DLVerificationRequest request, string VerificationId)
        {
            bool result = false;
            try
            {
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdVNninUrl + VerificationId;

                    var jsonReq = JsonConvert.SerializeObject(request);
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<VninResponse>(res);
                    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<bool> VerifyNIN(DLVerificationRequest request, string VerificationId)
        {
            bool result = false;
            try
            {
                var qoreidTokenResponse = await AuthTokenForQoreId();
                if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                {
                    var url = AppSetting.QoreIdNINVerificationURL + VerificationId;

                    var jsonReq = JsonConvert.SerializeObject(request);
                    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    var JsonRes = JsonConvert.DeserializeObject<NINResponse>(res);
                    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
