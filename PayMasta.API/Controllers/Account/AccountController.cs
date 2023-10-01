using Newtonsoft.Json;
using PayMasta.API.Filters;
using PayMasta.Service.Account;
using PayMasta.Service.Employer.Employees;
using PayMasta.Utilities.LogUtils;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.Account
{
    [RoutePrefix("api/AccountController")]
    public class AccountController : ApiController
    {
        // private readonly ILogUtils _logUtils;
        private IHttpActionResult _iHttpActionResult;
        private IAccountService _accountService;
        private IEmployeesService _employeesService;
        private Converter _converter;
        public AccountController(IAccountService accountService, IEmployeesService employeesService)
        {
            //  _logUtils = logUtils;
            _accountService = accountService;
            _converter = new Converter();
            _employeesService = employeesService;
        }

        [HttpPost]
        [Route("Login")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<LoginResponse>))]
        public async Task<IHttpActionResult> Login(LoginRequest request)
        {
            var response = new Response<LoginResponse>();
            var result = new LoginResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.DeviceToken) || string.IsNullOrWhiteSpace(request.DeviceId))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.Email))
                        {
                            errorList.Add(new Errorkey { Key = "Email", Val = "The Email field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Password))
                        {
                            errorList.Add(new Errorkey { Key = "Password", Val = "The Password field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.DeviceToken))
                        {
                            errorList.Add(new Errorkey { Key = "DeviceToken", Val = "The DeviceToken field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.DeviceId))
                        {
                            errorList.Add(new Errorkey { Key = "DeviceId", Val = "The DeviceId field is required" });
                        }

                        response = response.Create(false, ResponseMessages.USER_NOT_REGISTERED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        result = await _accountService.AppLogin(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.LOGIN_FAILED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.USER_BLOCKED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 4:
                                response = response.Create(false, ResponseMessages.USER_DELETED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 20:
                                response = response.Create(false, ResponseMessages.Employer_CAN_NOT_LOGIN, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 5:
                                response = response.Create(false, ResponseMessages.USER_NOT_EXIST, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.LOGIN_FAILED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.LOGIN_FAILED, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.LOGIN_FAILED, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("SignUp")]
        [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<SignupResponse>))]
        public async Task<IHttpActionResult> SignUp(SignUpRequest request)
        {
            var response = new Response<SignupResponse>();
            var result = new SignupResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.CountryCode))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.Email))
                        {
                            errorList.Add(new Errorkey { Key = "Email", Val = "The Email field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Password))
                        {
                            errorList.Add(new Errorkey { Key = "Password", Val = "The Password field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.CountryCode))
                        {
                            errorList.Add(new Errorkey { Key = "CountryCode", Val = "The CountryCode field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                        {
                            errorList.Add(new Errorkey { Key = "PhoneNumber", Val = "The PhoneNumber field is required" });
                        }

                        response = response.Create(false, ResponseMessages.USER_NOT_REGISTERED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }
                    var otpReq = new VerifyOTPRequest { MobileNumber = request.PhoneNumber, OtpCode = request.OtpCode };
                    var otp = await _accountService.VerifyOTP(otpReq);
                    if (otp.RstKey == 6)
                    {
                        result = await _accountService.SignUp(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(false, ResponseMessages.INACTIVE_USER, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.INACTIVE_USER, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.EMAIL_UNVERIFIED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 4:
                                response = response.Create(false, ResponseMessages.OTP_NOT_VERIFIED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 5:
                                response = response.Create(false, ResponseMessages.INACTIVE_USER, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 6:
                                response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 7:
                                response = response.Create(false, ResponseMessages.DUPLICATE_CREDENTIALS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 8:
                                response = response.Create(false, ResponseMessages.DUPLICATE_CREDENTIALS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 10:
                                var errorList = new List<Errorkey>();

                                if (string.IsNullOrWhiteSpace(request.Email))
                                {
                                    errorList.Add(new Errorkey { Key = "Email", Val = "Email is required" });
                                }

                                if (string.IsNullOrWhiteSpace(request.CountryCode))
                                {
                                    errorList.Add(new Errorkey { Key = "CountryCode", Val = "CountryCode is required" });
                                }
                                if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                                {
                                    errorList.Add(new Errorkey { Key = "PhoneNumber", Val = "PhoneNumber is required" });
                                }

                                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 11:
                                response = response.Create(true, ResponseMessages.USER_REGISTERED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_OTP, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("ForgotPassword")]
        //  [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<Object>))]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = new Response<Object>();
            var result = new Object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.ForgotPassword(request);
                    if (!string.IsNullOrWhiteSpace((string)result))
                    {
                        response = response.Create(true, ResponseMessages.OTP_SENT, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((string)result == "")
                    {
                        response = response.Create(false, ResponseMessages.USER_NOT_EXIST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("ResetPassword")]
        // [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<OtpResponse>))]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var response = new Response<OtpResponse>();
            var result = new OtpResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.ResetPassword(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.PASSWORD_CHANGED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, result.Message, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(false, result.Message, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("SendOtp")]
        // [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<OtpResponse>))]
        public async Task<IHttpActionResult> SendOtp(OtpRequest request)
        {
            var response = new Response<OtpResponse>();
            var result = new OtpResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    var req = new UserExistanceRequest { MobileNo = request.MobileNo, Email = request.Email };
                    //   var resultCreds = await _accountService.CredentialsExistanceForMobileNumber(req);
                    if (!await _accountService.CredentialsExistanceForMobileNumberOrEmail(req))
                    {
                        result = await _accountService.SendOTP(request);
                        if (result.RstKey == 1)
                        {
                            response = response.Create(true, ResponseMessages.OTP_SENT, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        }
                        else
                        {
                            response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        }
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.EXIST_MOBILE_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("ResendOTP")]
        // [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<Object>))]
        public async Task<IHttpActionResult> ResendOTP(ResendOTPRequest request)
        {
            var response = new Response<Object>();
            var result = new Object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.ResendOTP(request);
                    if ((int)result == 1)
                    {
                        response = response.Create(true, ResponseMessages.OTP_SENT, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("VerifyOTP")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<LoginResponse>))]
        public async Task<IHttpActionResult> VerifyOTP(VerifyOTPRequest request)
        {
            var response = new Response<LoginResponse>();
            var result = new LoginResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.VerifyOTP(request);
                    if (result.RstKey == 6)
                    {
                        response = response.Create(true, ResponseMessages.OTP_VERIFIED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 501)
                    {
                        response = response.Create(false, ResponseMessages.OTP_NOT_VERIFIED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("Logout")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<Object>))]
        public async Task<IHttpActionResult> Logout(LogoutRequest request)
        {
            var response = new Response<Object>();
            var result = new Object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.Logout(request);
                    if ((int)result == 1)
                    {
                        response = response.Create(true, ResponseMessages.LOGOUT_SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        /// <summary>
        /// GetProfile
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProfile")]
        //[JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UserModel>))]
        public async Task<IHttpActionResult> GetProfile(Guid userGuid)
        // public async Task<IHttpActionResult> GetProfile(RequestModel request)
        {
            //var requestModel = new EncrDecr<GetUserProfileRequest>().Decrypt(request.Value);

            var response = new Response<UserModel>();
            var result = new UserModel();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetProfile(userGuid);
                    if (result != null)
                    {
                        response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }


        [HttpPost]
        [Route("CompleteProfile")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeeUpdateProfileResponse>))]
        public async Task<IHttpActionResult> CompleteProfile(UpdateUserRequest request)
        {
            var response = new Response<EmployeeUpdateProfileResponse>();
            var result = new EmployeeUpdateProfileResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.UpdateProfile(request);
                    if ((int)result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.PROFILE_UPDATED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.STAFFIDDUPLICATE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result.RstKey == 8)
                    {
                        response = response.Create(false, ResponseMessages.INVALID_MOBILE_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result.RstKey == 9)
                    {
                        response = response.Create(false, result.Message, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("ChangePassword")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<Object>))]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var response = new Response<Object>();
            var result = new Object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.ChangePassword(request);
                    if ((int)result == 1)
                    {
                        response = response.Create(true, ResponseMessages.PASSWORD_CHANGED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result == 2)
                    {
                        response = response.Create(false, ResponseMessages.PASSWORD_NOT_CURRECT, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result == 3)
                    {
                        response = response.Create(false, ResponseMessages.OLD_PASSWORD_NEW_PASSWORD_SAME, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("NinVerify")]
        //  [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<NinResponse>))]
        public async Task<IHttpActionResult> NinVerify(NINNumberVerifyRequest request)
        {
            var response = new Response<NinResponse>();
            var result = new NinResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.NinVerify(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.USER_VERIFIED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    if (result.RstKey == 10)
                    {
                        response = response.Create(false, ResponseMessages.INVALIDNIN_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.NotFound);
                    }
                    else if (result.RstKey == 9)
                    {
                        response = response.Create(false, ResponseMessages.EXIST_NIN_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.BadRequest);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpGet]
        [Route("GetNotificationCount")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<int>))]
        public async Task<IHttpActionResult> GetNotificationCount(Guid guid)
        {
            var response = new Response<Object>();
            var result = new Object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetNotificationCount(guid);
                    if (result != null)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }


        [HttpGet]
        [Route("GetCountry")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetCountryResponse>))]
        public async Task<IHttpActionResult> GetCountry()
        {
            var response = new Response<GetCountryResponse>();
            var result = new GetCountryResponse();

            //var str = "appventurez";
            //var outString = string.Empty;

            //for (int i = str.Length - 1; i >= 0; i--)
            //{
            //    outString = outString+str[i];
            //}
            //var d = outString;

            //int number = 12345;
            //int rem = 0;
            //int result1 = 0;
            //while (number != 0)
            //{
            //    rem = number % 10;
            //    result1 = result1 * 10 + rem;
            //    number = number / 10;

            //}



            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetCountry();
                    //for encrypt request or response
                    //var requestModel = new EncrDecr<GetCountryResponse>().Encrypt(JsonConvert.SerializeObject(result));
                    //var requestModel1 = new EncrDecr<GetCountryResponse>().Decrypt(requestModel);
                    if (result != null)
                    {
                        response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpGet]
        [Route("GetStateByCountryGuid")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetSateResponse>))]
        public async Task<IHttpActionResult> GetStateByCountryGuid(Guid guid)
        {
            var response = new Response<GetSateResponse>();
            var result = new GetSateResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetState(guid);
                    if (result != null)
                    {
                        response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpGet]
        [Route("GetCityByStateGuid")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetCityResponse>))]
        public async Task<IHttpActionResult> GetCityByStateGuid(Guid guid)
        {
            var response = new Response<GetCityResponse>();
            var result = new GetCityResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetCityByStateGuid(guid);
                    if (result != null)
                    {
                        response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("VerifyForgetPasswordOTP")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<LoginResponse>))]
        public async Task<IHttpActionResult> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request)
        {
            var response = new Response<LoginResponse>();
            var result = new LoginResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.VerifyForgetPasswordOTP(request);
                    if (result.RstKey == 6)
                    {
                        response = response.Create(true, ResponseMessages.OTP_VERIFIED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 501)
                    {
                        response = response.Create(false, ResponseMessages.OTP_NOT_VERIFIED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("AddBanks")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AddBankListResponse>))]
        public async Task<IHttpActionResult> AddBanks(AddBankListRequest request)
        {
            var response = new Response<AddBankListResponse>();
            var result = new AddBankListResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    if (!await _accountService.IsBankExists(request.UserGuid, request.AccountNumber))
                    {
                        result = await _accountService.InsertBank(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.DATA_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.BANK_ALREADY_EXIST, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 4:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 5:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 6:
                                response = response.Create(true, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 7:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 8:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 10:
                                var errorList = new List<Errorkey>();

                                if (string.IsNullOrWhiteSpace(request.AccountNumber))
                                {
                                    errorList.Add(new Errorkey { Key = "AccountNumber", Val = "AccountNumber is required" });
                                }

                                if (request.UserGuid == null)
                                {
                                    errorList.Add(new Errorkey { Key = "UserGuid", Val = "UserGuid is required" });
                                }
                                if (string.IsNullOrWhiteSpace(request.BankName))
                                {
                                    errorList.Add(new Errorkey { Key = "BankName", Val = "BankName is required" });
                                }
                                if (string.IsNullOrWhiteSpace(request.BankCode))
                                {
                                    errorList.Add(new Errorkey { Key = "BankCode", Val = "BankCode is required" });
                                }

                                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 11:
                                response = response.Create(true, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }
                        //if (result.RstKey == 1)
                        //{
                        //    response = response.Create(true, ResponseMessages.DATA_SAVED, null, result);
                        //    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        //}
                        //else
                        //{
                        //    response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        //    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        //}
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.EXIST_ACCOUNT_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("GetBankListByUserGuid")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetBanksListResponse>))]
        public async Task<IHttpActionResult> GetBankListByUserGuid(GetBankListRequest request)
        {
            var response = new Response<GetBanksListResponse>();
            var result = new GetBanksListResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    if (request.UserGuid != null)
                    {
                        result = await _accountService.GetBankListByUserGuid(request.UserGuid);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 4:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 5:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 6:
                                response = response.Create(true, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 7:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 8:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 10:
                                var errorList = new List<Errorkey>();
                                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 11:
                                response = response.Create(true, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }
                        //if (result.RstKey == 1)
                        //{
                        //    response = response.Create(true, ResponseMessages.DATA_SAVED, null, result);
                        //    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        //}
                        //else
                        //{
                        //    response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        //    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        //}
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.USER_NOT_EXIST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        //[HttpPost]
        //[Route("UpdateUserProfile")]
        //// [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        //[ResponseType(typeof(Response<EmployeeUpdateProfileResponse>))]
        //public async Task<IHttpActionResult> UpdateUserProfile(UpdateUserProfileRequest request)
        //{
        //    var response = new Response<EmployeeUpdateProfileResponse>();
        //    var result = new EmployeeUpdateProfileResponse();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            result = await _accountService.UpdateUserProfile(request);
        //            if (result.RstKey == 1)
        //            {
        //                response = response.Create(true, ResponseMessages.PROFILE_UPDATED, null, result);
        //                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
        //            }
        //            else if ((int)result.RstKey == 8)
        //            {
        //                response = response.Create(false, ResponseMessages.INVALID_MOBILE_NO, null, result);
        //                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
        //                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
        //            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
        //        }
        //    }
        //    else
        //    {
        //        var errorList = new List<Errorkey>();
        //        foreach (var mod in ModelState)
        //        {
        //            Errorkey objkey = new Errorkey();
        //            objkey.Key = mod.Key;
        //            if (mod.Value.Errors.Count > 0)
        //            {
        //                objkey.Val = mod.Value.Errors[0].ErrorMessage;
        //            }
        //            errorList.Add(objkey);
        //        }
        //        response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
        //        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
        //    }
        //    return _iHttpActionResult;
        //}

        [HttpPost]
        [Route("UpdateUserProfileRequest")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeeUpdateProfileResponse>))]
        public async Task<IHttpActionResult> UpdateUserProfileRequest(UpdateUserProfileRequest request)
        {
            var response = new Response<EmployeeUpdateProfileResponse>();
            var result = new EmployeeUpdateProfileResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.UpdateUserProfileRequest(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.PROFILE_UPDATED_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if ((int)result.RstKey == 8)
                    {
                        response = response.Create(false, ResponseMessages.INVALID_MOBILE_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("DeleteBankByBankDetailId")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeeUpdateProfileResponse>))]
        public async Task<IHttpActionResult> DeleteBankByBankDetailId(DeleteBankRequest request)
        {
            var response = new Response<EmployeeUpdateProfileResponse>();
            var result = new EmployeeUpdateProfileResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    if (request.UserGuid != null && request.BankId > 0)
                    {
                        result = await _accountService.DeleteBankByBankDetailId(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.Bank_DELETED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.BANK_NOT_DELETED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;

                            default:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }

                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.EXIST_ACCOUNT_NO, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("UploadProfileImage")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UploadProfileImageResponse>))]
        public async Task<IHttpActionResult> UploadProfileImage(UploadProfileImageRequest request)
        {
            var response = new Response<UploadProfileImageResponse>();
            var result = new UploadProfileImageResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.UploadProfileImage(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.PROFILE_UPDATED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.PROFILE_NOT_UPDATED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(false, ResponseMessages.PROFILE_NOT_UPDATED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("IsPasswordValid")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<IsPasswordValidResponse>))]
        public async Task<IHttpActionResult> IsPasswordValid(IsPasswordValidRequest request)
        {
            var response = new Response<IsPasswordValidResponse>();
            var result = new IsPasswordValidResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    if (request.UserGuid != null && request.Password != null)
                    {
                        result = await _accountService.IsPasswordValid(request);
                        if (result.RstKey == 1)
                        {
                            response = response.Create(true, ResponseMessages.PASSWORD_CURRECT, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        }
                        else
                        {
                            response = response.Create(false, ResponseMessages.PASSWORD_INVALID, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                        }
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.PASSWORD_INVALID, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }


        [HttpPost]
        [Route("DeleteEmployee")]
        //[JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<Object>))]
        public async Task<IHttpActionResult> DeleteEmployees(DeleteEmployeeRequest request)
        {
            var response = new Response<BlockUnBlockEmployeeResponse>();
            var result = new BlockUnBlockEmployeeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _employeesService.DeleteEmployees(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, AdminResponseMessages.USER_DELETED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, AdminResponseMessages.USER_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(false, AdminResponseMessages.USER_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }

        [HttpPost]
        [Route("UpdateEmployer")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeeUpdateProfileResponse>))]
        public async Task<IHttpActionResult> UpdateEmployer(UpdateEmployerByUserGuidRequest request)
        {
            var response = new Response<EmployeeUpdateProfileResponse>();
            var result = new EmployeeUpdateProfileResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.UpdateEmployer(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATAUPDATED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, AdminResponseMessages.DATA_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(false, AdminResponseMessages.USER_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                    _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                var errorList = new List<Errorkey>();
                foreach (var mod in ModelState)
                {
                    Errorkey objkey = new Errorkey();
                    objkey.Key = mod.Key;
                    if (mod.Value.Errors.Count > 0)
                    {
                        objkey.Val = mod.Value.Errors[0].ErrorMessage;
                    }
                    errorList.Add(objkey);
                }
                response = response.Create(false, ResponseMessages.INVALID_REQUEST, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }
    }
}
