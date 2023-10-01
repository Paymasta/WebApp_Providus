using Firebase.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.ProvidusVirtualAccountDetail;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.Repository.Account;
using PayMasta.Repository.VirtualAccountRepository;
using PayMasta.Service.FlutterWave;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.Common;
using PayMasta.ViewModel.VirtualAccountVM;
using PayMasta.ViewModel.WalletBalanceAndNuban;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.VirtualAccount
{
    public class VirtualAccountService : IVirtualAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository;
        private readonly IFlutterWaveService _flutterWaveService;
        private readonly IThirdParty _thirdParty;
        // private readonly IVirtualAccountService _virtualAccountService;
        public VirtualAccountService()
        {
            //   _virtualAccountService = new VirtualAccountService();
            _flutterWaveService = new FlutterWaveService();
            _accountRepository = new AccountRepository();
            _virtualAccountRepository = new VirtualAccountRepository();
            _thirdParty = new ThirdPartyService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        //public async Task<bool> CreateVirtualAccount(string FirstName, string LastName, string Email, string PhoneNumber, long userId)
        //{
        //    bool res = false;
        //    //This block of code for creating vertual account on flutter wave
        //    if (FirstName != null && LastName != null && Email != null && PhoneNumber != null)
        //    {
        //        var req = new CreateVertualAccountRequest
        //        {
        //            account_name = FirstName + " " + LastName,
        //            country = "NG",
        //            email = Email,
        //            mobilenumber = PhoneNumber
        //        };

        //        var flutterService = await _flutterWaveService.CreateVertualAccount(req);
        //        var JsonResult = JsonConvert.DeserializeObject<VirtualAccountNumberResponse>(flutterService);
        //        if (JsonResult != null && JsonResult.status == "success")
        //        {
        //            var virtualAccount = new VirtualAccountDetail
        //            {
        //                AccountName = JsonResult.data.account_name,
        //                Country = JsonResult.data.country,
        //                AccountReference = JsonResult.data.account_reference,
        //                BankCode = JsonResult.data.bank_code,
        //                BankName = JsonResult.data.bank_name,
        //                BarterId = JsonResult.data.barter_id,
        //                CreatedAt = JsonResult.data.created_at,
        //                email = JsonResult.data.email,
        //                MobileNumber = JsonResult.data.mobilenumber,
        //                IsActive = true,
        //                IsDeleted = false,
        //                VirtualAccountId = JsonResult.data.id.ToString(),
        //                Nuban = JsonResult.data.nuban,
        //                Status = JsonResult.data.status,
        //                UserId = userId,
        //                UpdatedAt = JsonResult.data.created_at,
        //            };
        //            if (await _virtualAccountRepository.InsertVirtualAccountDetail(virtualAccount) > 0)
        //            {
        //                res = true;
        //            }

        //        }
        //    }
        //    return res;
        //}

        //public async Task<GetVirtualAccountBalanceResponse> GetVirtualAccountBalance(Guid userId)
        //{
        //    bool res = false;
        //    var result = new GetVirtualAccountBalanceResponse();
        //    var user = await _accountRepository.GetUserByGuid(userId);
        //    var userData = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(user.Id);
        //    //This block of code for creating vertual account on flutter wave
        //    if (userData != null)
        //    {
        //        string url = AppSetting.FlutterWaveVertualAccountBalance + userData.AccountReference + "/balances";
        //        var flutterService = await _thirdParty.GetVertualAccountBalance(url);
        //        var JsonResult = JsonConvert.DeserializeObject<VirtualAccountNumberResponse>(flutterService);
        //        if (JsonResult != null && JsonResult.status == "success")
        //        {
        //            result.Currency = JsonResult.data.currency;
        //            result.AvailableBalance = JsonResult.data.available_balance;
        //            result.LedgerBalance = JsonResult.data.ledger_balance;
        //            result.RstKey = 1;
        //        }
        //        else
        //        {
        //            result.AvailableBalance = 0;
        //            result.RstKey = 2;
        //        }
        //    }
        //    else
        //    {
        //        result.AvailableBalance = 0;
        //        result.RstKey = 2;
        //    }
        //    return result;
        //}

        public async Task<bool> CreateVirtualAccount(string FirstName, string LastName, string Bvn, long userId, string addess, DateTime dateOfBirth, string email, string gender, string phoneNumber, string latitude
            , string localGovt, string longitude, string password, string pin, string state)
        {
            var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userId);
            bool res = false;
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start(); //started watcher
            GeoCoordinate coord = watcher.Position.Location;
            double lat = 0;
            double _long = 0;
            if (!watcher.Position.Location.IsUnknown)
            {
                lat = coord.Latitude; //latitude
                _long = coord.Longitude;  //logitude
            }
            else
            {
                lat = 28.572660; //latitude
                _long = 77.329590;  //logitude
            }
            if (virtualAccountDetail == null)
            {
                //This block of code for creating vertual account on flutter wave
                if (FirstName != null && LastName != null && Bvn != null)
                {
                    string url = AppSetting.WalletExternal;
                    var req = new CreateVirtualAccountForUserRequest
                    {
                        accountName = FirstName,
                        address = addess,
                        dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd"),
                        email = email,
                        firstName = FirstName,
                        lastName = LastName,
                        gender = gender,
                        phoneNumber = phoneNumber,
                        latitude = lat.ToString(),
                        localGovt = localGovt,
                        longitude = _long.ToString(),
                        password = password,
                        scheme = AppSetting.schemeId.ToString(),
                        photo = "",
                        pin = pin,
                        state = state
                    };
                    var json = JsonConvert.SerializeObject(req);

                    // var flutterService = "{\n  \"code\" : \"success\",\n  \"message\" : \"Wallet created successfully\",\n  \"data\" : [ {\n    \"id\" : 1783456,\n    \"accountName\" : \"Uche\",\n    \"accountNumber\" : \"3499787272\",\n    \"currentBalance\" : \"0.0\",\n    \"nubanAccountNo\" : null,\n    \"trackingRef\" : null,\n    \"dateOpened\" : \"2022-08-23\",\n    \"status\" : \"ACTIVE\",\n    \"actualBalance\" : \"0.0\",\n    \"walletLimit\" : \"0.0\",\n    \"scheme\" : {\n      \"id\" : 1788401,\n      \"schemeID\" : null,\n      \"scheme\" : null,\n      \"bankCode\" : null,\n      \"accountNumber\" : null,\n      \"apiKey\" : null,\n      \"secretKey\" : null,\n      \"schemeCategory\" : null,\n      \"callbackUrl\" : null\n    },\n    \"walletAccountType\" : {\n      \"id\" : 1,\n      \"accountypeID\" : null,\n      \"walletAccountType\" : null\n    },\n    \"accountOwner\" : {\n      \"createdDate\" : null,\n      \"lastModifiedDate\" : \"2022-08-23T16:15:58.548Z\",\n      \"id\" : 1783907,\n      \"profileID\" : null,\n      \"pin\" : null,\n      \"deviceNotificationToken\" : null,\n      \"phoneNumber\" : null,\n      \"gender\" : null,\n      \"dateOfBirth\" : null,\n      \"address\" : null,\n      \"secretQuestion\" : null,\n      \"secretAnswer\" : null,\n      \"photo\" : null,\n      \"photoContentType\" : null,\n      \"bvn\" : null,\n      \"validID\" : null,\n      \"validDocType\" : \"NIN\",\n      \"nin\" : null,\n      \"profilePicture\" : null,\n      \"totalBonus\" : 0.0,\n      \"myDevices\" : [ ],\n      \"paymentTransactions\" : [ ],\n      \"billerTransactions\" : [ ],\n      \"customersubscriptions\" : [ ],\n      \"user\" : null,\n      \"bonusPoints\" : [ ],\n      \"approvalGroup\" : null,\n      \"profileType\" : null,\n      \"kyc\" : null,\n      \"beneficiaries\" : [ ],\n      \"addresses\" : [ ],\n      \"fullName\" : \"\"\n    },\n    \"parent\" : null,\n    \"subWallets\" : [ ],\n    \"accountFullName\" : \"\"\n  } ],\n  \"metadata\" : null\n}";// await _thirdParty.CreateProvidusVirtualAccount(json, url);
                    var flutterService = await _thirdParty.CreateProvidusVirtualAccount(json, url);
                    var JsonResult = JsonConvert.DeserializeObject<AccountResponseData>(flutterService);
                    if (JsonResult != null && JsonResult.code == "success")
                    {
                        var virtualAccount = new VirtualAccountDetail
                        {
                            VirtualAccountId = JsonResult.data[0].id.ToString(),
                            DateOfBirth = JsonResult.data[0].accountOwner.dateOfBirth != null ? JsonResult.data[0].accountOwner.dateOfBirth.ToString() : DateTime.UtcNow.ToString(),
                            CurrentBalance = JsonResult.data[0].currentBalance != null ? JsonResult.data[0].currentBalance.ToString() : "0",
                            IsActive = true,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            AccountName = JsonResult.data[0].accountName != null ? JsonResult.data[0].accountName.ToString() : "",
                            AccountNumber = JsonResult.data[0].accountNumber != null ? JsonResult.data[0].accountNumber.ToString() : "",
                            Address = JsonResult.data[0].accountOwner.address != null ? JsonResult.data[0].accountOwner.address.ToString() : "",
                            PhoneNumber = phoneNumber,
                            Bvn = JsonResult.data[0].accountOwner.bvn != null ? JsonResult.data[0].accountOwner.bvn.ToString() : "",
                            CreatedBy = userId,
                            Gender = gender,
                            deviceNotificationToken = JsonResult.data[0].accountOwner.deviceNotificationToken != null ? JsonResult.data[0].accountOwner.deviceNotificationToken.ToString() : "",
                            Pin = JsonResult.data[0].accountOwner.pin != null ? JsonResult.data[0].accountOwner.pin.ToString() : "",
                            ProfileID = JsonResult.data[0].accountOwner.profileID != null ? JsonResult.data[0].accountOwner.profileID.ToString() : "",
                            JsonData = flutterService,
                            UpdatedBy = userId,
                            UserId = userId,
                            AuthJson = "",
                            AuthToken = "",
                        };
                        if (await _virtualAccountRepository.InsertVirtualAccountDetail(virtualAccount) > 0)
                        {
                            if (await AuthenticateVirtualAccount(phoneNumber, password, true, AppSetting.schemeId.ToString(), "", userId))
                                res = true;
                        }
                        else
                        {
                            res = false;
                        }
                    }
                    else
                    {
                       // await AuthenticateVirtualAccount(phoneNumber, password, true, AppSetting.schemeId.ToString(), "", userId);
                        res = false;
                    }
                }

            }
            else if (string.IsNullOrWhiteSpace(virtualAccountDetail.AuthToken))
            {
                if (await AuthenticateVirtualAccount(phoneNumber, password, true, AppSetting.schemeId.ToString(), "", userId))
                    res = true;
            }
            else
            {
                await AuthenticateVirtualAccount(phoneNumber, password, true, AppSetting.schemeId.ToString(), "", userId);
                res = true;
            }
            return res;
        }

        public async Task<bool> AuthenticateVirtualAccount(string username, string password, bool rememberMe, string schemeid, string deviceId, long userId)
        {
            bool res = false;
            try
            {
                var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userId);
                var req = new AuthenticateRequest
                {
                    deviceId = "64784844-hhhd748849-g7378382",
                    username = username,
                    password = password,
                    rememberMe = rememberMe,
                    scheme = schemeid,
                };
                var json = JsonConvert.SerializeObject(req);

                var authenticateResponse = await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());// "{\n  \"message\" : \"Login success\",\n  \"code\" : null,\n  \"token\" : \"eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0ODkzOTM5ODg4OSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjM5MTQwNTR9.k_NCa24vLx3MxQMVXnMXstos0tjQBfKfbG8K5Xpr8yUd9f7wQ5fJwxFiozYvKqluN1fFJSX5BYJhRDT9pfOKZQ\",\n  \"user\" : {\n    \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n    \"lastModifiedDate\" : \"2022-08-24T06:18:44.643Z\",\n    \"id\" : 1790651,\n    \"profileID\" : \"2\",\n    \"pin\" : \"$2a$10$I2LUjQ.bRdnTinvgvHdVbeOroo8v2nsRFIYhbSuKFgYWhHjLGKVdG\",\n    \"deviceNotificationToken\" : null,\n    \"phoneNumber\" : \"+2348939398889\",\n    \"gender\" : \"MALE\",\n    \"dateOfBirth\" : \"1974-02-17\",\n    \"address\" : \"B-1000, 4th Floor\",\n    \"secretQuestion\" : null,\n    \"secretAnswer\" : null,\n    \"photo\" : null,\n    \"photoContentType\" : null,\n    \"bvn\" : null,\n    \"validID\" : null,\n    \"validDocType\" : \"NIN\",\n    \"nin\" : null,\n    \"profilePicture\" : null,\n    \"totalBonus\" : 0.0,\n    \"walletAccounts\" : null,\n    \"myDevices\" : null,\n    \"paymentTransactions\" : null,\n    \"billerTransactions\" : null,\n    \"customersubscriptions\" : null,\n    \"user\" : {\n      \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"lastModifiedDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"id\" : 1790601,\n      \"login\" : \"+2348939398889\",\n      \"firstName\" : \"Festus\",\n      \"lastName\" : \"Oluwadurotimi\",\n      \"email\" : \"festus@gmail.com\",\n      \"activated\" : true,\n      \"langKey\" : null,\n      \"imageUrl\" : null,\n      \"resetDate\" : null,\n      \"status\" : \"OK\"\n    },\n    \"bonusPoints\" : null,\n    \"approvalGroup\" : null,\n    \"profileType\" : null,\n    \"kyc\" : {\n      \"id\" : 1,\n      \"kycID\" : 1,\n      \"kyc\" : \"1\",\n      \"description\" : \"KYC Level 1\",\n      \"kycLevel\" : 1,\n      \"phoneNumber\" : true,\n      \"emailAddress\" : true,\n      \"firstName\" : true,\n      \"lastName\" : true,\n      \"gender\" : true,\n      \"dateofBirth\" : true,\n      \"address\" : true,\n      \"photoUpload\" : true,\n      \"verifiedBVN\" : true,\n      \"verifiedValidID\" : true,\n      \"evidenceofAddress\" : true,\n      \"verificationofAddress\" : null,\n      \"employmentDetails\" : null,\n      \"dailyTransactionLimit\" : 50000.0,\n      \"cumulativeBalanceLimit\" : 300000.0,\n      \"paymentTransaction\" : null,\n      \"billerTransaction\" : null\n    },\n    \"beneficiaries\" : null,\n    \"addresses\" : null,\n    \"fullName\" : \"Oluwadurotimi Festus\"\n  },\n  \"userType\" : \"CUSTOMER\",\n  \"walletAccountList\" : [ {\n    \"id\" : 1790751,\n    \"accountNumber\" : \"3397367646\",\n    \"currentBalance\" : 0.0,\n    \"dateOpened\" : \"2022-08-24\",\n    \"schemeId\" : 1788401,\n    \"schemeName\" : \"PayMasta\",\n    \"walletAccountTypeId\" : 1,\n    \"accountOwnerId\" : 1790651,\n    \"accountOwnerName\" : \"Oluwadurotimi Festus\",\n    \"accountOwnerPhoneNumber\" : \"+2348939398889\",\n    \"accountName\" : \"Festus\",\n    \"status\" : \"ACTIVE\",\n    \"actualBalance\" : 0.0,\n    \"walletLimit\" : \"0.0\",\n    \"trackingRef\" : null,\n    \"nubanAccountNo\" : \"\",\n    \"accountFullName\" : \"Oluwadurotimi Festus\",\n    \"totalCustomerBalances\" : 0.0\n  } ]\n}";//await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());
                var JsonResult = JsonConvert.DeserializeObject<AuthenticateResponse>(authenticateResponse);

                if (JsonResult != null && JsonResult.code != "error" && JsonResult.code != "52" && virtualAccountDetail != null)
                {
                    try
                    {
                        if (Convert.ToInt32(JsonResult.code) != 52)
                        {
                            virtualAccountDetail.AuthToken = JsonResult.token;
                            virtualAccountDetail.AuthJson = authenticateResponse;
                            virtualAccountDetail.Pin = JsonResult.user.pin != null ? JsonResult.user.pin : "";
                            virtualAccountDetail.ProfileID = JsonResult.user.profileID != null ? JsonResult.user.profileID : "";
                            virtualAccountDetail.Address = JsonResult.user.address != null ? JsonResult.user.address : "";
                            virtualAccountDetail.Bvn = JsonResult.user.bvn != null ? JsonResult.user.bvn.ToString() : "";
                            if (await _virtualAccountRepository.UpdateVirtualAccountDetailByUserId(virtualAccountDetail) > 0)
                            {
                                res = true;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
                else if (JsonResult.message == "Login success" && JsonResult.token != null)
                {
                    var virtualAccount = new VirtualAccountDetail
                    {
                        VirtualAccountId = JsonResult.walletAccountList[0].id.ToString(),
                        DateOfBirth = JsonResult.user.dateOfBirth != null ? JsonResult.user.dateOfBirth.ToString() : DateTime.UtcNow.ToString(),
                        CurrentBalance = JsonResult.walletAccountList[0].currentBalance != null ? JsonResult.walletAccountList[0].currentBalance.ToString() : "0",
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        AccountName = JsonResult.walletAccountList[0].accountName != null ? JsonResult.walletAccountList[0].accountName.ToString() : "",
                        AccountNumber = JsonResult.walletAccountList[0].accountNumber != null ? JsonResult.walletAccountList[0].accountNumber.ToString() : "",
                        Address = JsonResult.user.address != null ? JsonResult.user.address.ToString() : "",
                        PhoneNumber = JsonResult.user.phoneNumber,
                        Bvn = JsonResult.user.bvn != null ? JsonResult.user.bvn.ToString() : "",
                        CreatedBy = userId,
                        Gender = JsonResult.user.gender,
                        deviceNotificationToken = JsonResult.user.deviceNotificationToken != null ? JsonResult.user.deviceNotificationToken.ToString() : "",
                        Pin = JsonResult.user.pin != null ? JsonResult.user.pin.ToString() : "",
                        ProfileID = JsonResult.user.profileID != null ? JsonResult.user.profileID.ToString() : "",
                        JsonData = authenticateResponse,
                        UpdatedBy = userId,
                        UserId = userId,
                        AuthJson = "",
                        AuthToken = "",
                    };
                    if (await _virtualAccountRepository.InsertVirtualAccountDetail(virtualAccount) > 0)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<CurrentBalanceResponse> GetVirtualAccountBalance(Guid guid)
        {
            var result = new CurrentBalanceResponse();
            var bankdetail = new BankDetail();
            // string token = string.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(guid);
                var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var adminKeyPair = AES256.AdminKeyPair;
                if (virtualAccountDetail == null)
                {
                    if (userData != null)
                    {
                        bankdetail = await _accountRepository.GetBankDetailByUserId(userData.Id);
                    }
                    if (virtualAccountDetail == null && bankdetail != null && !string.IsNullOrWhiteSpace(userData.Address) && !string.IsNullOrWhiteSpace(userData.State))
                    {
                        try
                        {
                            int _min = 0000;
                            int _max = 9999;
                            Random _rdm = new Random();
                            var pin = _rdm.Next(_min, _max);
                            var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, userData.Password);
                            if (await CreateVirtualAccount(userData.FirstName, userData.LastName,
                                              bankdetail.BVN, userData.Id, userData.Address, userData.DateOfBirth, userData.Email,
                                              userData.Gender, userData.CountryCode + userData.PhoneNumber, "", userData.Address, "", accountPassword, pin.ToString(),
                                              userData.State))
                            {
                                userData.IsvertualAccountCreated = true;
                            }
                            await _accountRepository.UpdateVirtualAccountStatus(userData);

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (userData.IsvertualAccountCreated)
                    {
                        var accountPassword = AES256.Decrypt(adminKeyPair.PrivateKey, userData.Password);
                        await AuthenticateVirtualAccount(userData.CountryCode + userData.PhoneNumber, accountPassword, true, AppSetting.schemeId.ToString(), "", userData.Id);
                    }
                }

                if (virtualAccountDetail != null)
                {

                    var authenticateResponse = await _thirdParty.GetVirtualAccount(virtualAccountDetail.AuthToken, AppSetting.CurrentBalanceAndNuban.ToString() + "/" + virtualAccountDetail.PhoneNumber + "/" + AppSetting.schemeId);

                    // var authenticateResponse = await _thirdParty.GetVirtualAccount(virtualAccountDetail.AuthToken, AppSetting.CustomerWallets.ToString());
                    var JsonResult = JsonConvert.DeserializeObject<dynamic>(authenticateResponse);
                    // var nubanRes = JsonConvert.DeserializeObject<NubanResponse>(virtualAccountDetail.AuthJson);
                    if (JsonResult.Count > 0)
                    {
                        result.Balance = JsonResult[0].actualBalance.ToString("0.00");
                        result.AccountNumber = JsonResult[0].accountNumber.ToString();
                        result.BankName = JsonResult[0].accountFullName.ToString();
                        result.RstKey = 1;
                        result.NubanAccountNumber = JsonResult[0].nubanAccountNo;
                        result.Status = true;
                    }
                    else
                    {
                        result.Balance = "0.00";
                        result.AccountNumber = virtualAccountDetail.AccountNumber;
                        result.BankName = virtualAccountDetail.AccountName;
                        result.RstKey = 1;
                    }
                }
                else
                {
                    result.Balance = "0.00";
                    result.AccountNumber = "NA";
                    result.BankName = "NA";
                    result.NubanAccountNumber = "NA";
                    result.RstKey = 1;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public async Task<CurrentBalanceResponse> ChangesWalletPassword(Guid guid, string password)
        {
            var result = new CurrentBalanceResponse();
            try
            {

                var userData = await _accountRepository.GetUserByGuid(guid);
                var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var req = new ChangeWalletPasswordRequest
                {
                    newPassword = password,
                    phoneNumber = virtualAccountDetail.PhoneNumber,
                    pin = userData.WalletPin.ToString()//AppSetting.PINNUMBER
                };
                var JsonReq = JsonConvert.SerializeObject(req);
                var authenticateResponse = await _thirdParty.ChangePassword(AppSetting.ChangeWalletPassword.ToString(), JsonReq);
                // var array = JArray.Parse(authenticateResponse);
                var JsonResult = JsonConvert.DeserializeObject<ChangeWalletPasswordResponse>(authenticateResponse);
                if (JsonResult != null && JsonResult.code == "00")
                {
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<bool> UpdatePin(long userId, int pin)
        {
            bool res = false;
            try
            {
                var virtualAccountDetail = await _virtualAccountRepository.GetVirtualAccountDetailByUserId(userId);
                var req = new UpdatePinRequest
                {
                    phoneNumber = virtualAccountDetail.PhoneNumber,
                    newPin = pin.ToString(),
                };
                var json = JsonConvert.SerializeObject(req);

                var authenticateResponse = await _thirdParty.UpdatePin(json, AppSetting.UpdatePin.ToString(), virtualAccountDetail.AuthToken);// "{\n  \"message\" : \"Login success\",\n  \"code\" : null,\n  \"token\" : \"eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0ODkzOTM5ODg4OSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjM5MTQwNTR9.k_NCa24vLx3MxQMVXnMXstos0tjQBfKfbG8K5Xpr8yUd9f7wQ5fJwxFiozYvKqluN1fFJSX5BYJhRDT9pfOKZQ\",\n  \"user\" : {\n    \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n    \"lastModifiedDate\" : \"2022-08-24T06:18:44.643Z\",\n    \"id\" : 1790651,\n    \"profileID\" : \"2\",\n    \"pin\" : \"$2a$10$I2LUjQ.bRdnTinvgvHdVbeOroo8v2nsRFIYhbSuKFgYWhHjLGKVdG\",\n    \"deviceNotificationToken\" : null,\n    \"phoneNumber\" : \"+2348939398889\",\n    \"gender\" : \"MALE\",\n    \"dateOfBirth\" : \"1974-02-17\",\n    \"address\" : \"B-1000, 4th Floor\",\n    \"secretQuestion\" : null,\n    \"secretAnswer\" : null,\n    \"photo\" : null,\n    \"photoContentType\" : null,\n    \"bvn\" : null,\n    \"validID\" : null,\n    \"validDocType\" : \"NIN\",\n    \"nin\" : null,\n    \"profilePicture\" : null,\n    \"totalBonus\" : 0.0,\n    \"walletAccounts\" : null,\n    \"myDevices\" : null,\n    \"paymentTransactions\" : null,\n    \"billerTransactions\" : null,\n    \"customersubscriptions\" : null,\n    \"user\" : {\n      \"createdDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"lastModifiedDate\" : \"2022-08-24T06:18:44.390Z\",\n      \"id\" : 1790601,\n      \"login\" : \"+2348939398889\",\n      \"firstName\" : \"Festus\",\n      \"lastName\" : \"Oluwadurotimi\",\n      \"email\" : \"festus@gmail.com\",\n      \"activated\" : true,\n      \"langKey\" : null,\n      \"imageUrl\" : null,\n      \"resetDate\" : null,\n      \"status\" : \"OK\"\n    },\n    \"bonusPoints\" : null,\n    \"approvalGroup\" : null,\n    \"profileType\" : null,\n    \"kyc\" : {\n      \"id\" : 1,\n      \"kycID\" : 1,\n      \"kyc\" : \"1\",\n      \"description\" : \"KYC Level 1\",\n      \"kycLevel\" : 1,\n      \"phoneNumber\" : true,\n      \"emailAddress\" : true,\n      \"firstName\" : true,\n      \"lastName\" : true,\n      \"gender\" : true,\n      \"dateofBirth\" : true,\n      \"address\" : true,\n      \"photoUpload\" : true,\n      \"verifiedBVN\" : true,\n      \"verifiedValidID\" : true,\n      \"evidenceofAddress\" : true,\n      \"verificationofAddress\" : null,\n      \"employmentDetails\" : null,\n      \"dailyTransactionLimit\" : 50000.0,\n      \"cumulativeBalanceLimit\" : 300000.0,\n      \"paymentTransaction\" : null,\n      \"billerTransaction\" : null\n    },\n    \"beneficiaries\" : null,\n    \"addresses\" : null,\n    \"fullName\" : \"Oluwadurotimi Festus\"\n  },\n  \"userType\" : \"CUSTOMER\",\n  \"walletAccountList\" : [ {\n    \"id\" : 1790751,\n    \"accountNumber\" : \"3397367646\",\n    \"currentBalance\" : 0.0,\n    \"dateOpened\" : \"2022-08-24\",\n    \"schemeId\" : 1788401,\n    \"schemeName\" : \"PayMasta\",\n    \"walletAccountTypeId\" : 1,\n    \"accountOwnerId\" : 1790651,\n    \"accountOwnerName\" : \"Oluwadurotimi Festus\",\n    \"accountOwnerPhoneNumber\" : \"+2348939398889\",\n    \"accountName\" : \"Festus\",\n    \"status\" : \"ACTIVE\",\n    \"actualBalance\" : 0.0,\n    \"walletLimit\" : \"0.0\",\n    \"trackingRef\" : null,\n    \"nubanAccountNo\" : \"\",\n    \"accountFullName\" : \"Oluwadurotimi Festus\",\n    \"totalCustomerBalances\" : 0.0\n  } ]\n}";//await _thirdParty.CreateProvidusVirtualAccount(json, AppSetting.AuthenticatePouchii.ToString());
                var JsonResult = JsonConvert.DeserializeObject<UpdatePinResponse>(authenticateResponse);

                if (JsonResult.code == "00")
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
