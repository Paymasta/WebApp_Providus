using PayMasta.API.Filters;
using PayMasta.Service.Account;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.VerifyNin;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PayMasta.ViewModel.VerifyNinVM;

namespace PayMasta.API.Controllers.Account
{
    [RoutePrefix("api/RegisterWithNubanController")]
    public class RegisterWithNubanController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private Converter _converter;
        private IVerifyNinService _verifyNinService;
        private IAccountService _accountService;
        public RegisterWithNubanController(IVerifyNinService verifyNinService, IAccountService accountService)
        {
            //  _logUtils = logUtils;
            _converter = new Converter();
            _verifyNinService = verifyNinService;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("SignUpWithNuban")]
        [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<QoreIdBvnNubanResult>))]
        public async Task<IHttpActionResult> SignUpWithNuban(RegisterByNubanRequest request)
        {
            var response = new Response<QoreIdBvnNubanResult>();
            var result = new QoreIdBvnNubanResult();
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
                        result = await _verifyNinService.VerifyNuban(request);
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

        [HttpGet]
        [Route("BankListForRegister")]
        [ResponseType(typeof(Response<BankListResult>))]
        public async Task<IHttpActionResult> BankListForRegister()
        {
            var response = new Response<BankListResult>();
            var result = new BankListResult();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _verifyNinService.BankListForRegister();
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
        [Route("AddUserOtherDetail")]
        [JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<AddOtherDetailResponse>))]
        public async Task<IHttpActionResult> AddUserOtherDetail(AddOtherDetailPRequest request)
        {
            var response = new Response<AddOtherDetailResponse>();
            var result = new AddOtherDetailResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.State) || string.IsNullOrWhiteSpace(request.city) || string.IsNullOrWhiteSpace(request.Countryname) || string.IsNullOrWhiteSpace(request.VerificationId))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.Address))
                        {
                            errorList.Add(new Errorkey { Key = "Address", Val = "The address field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.State))
                        {
                            errorList.Add(new Errorkey { Key = "State", Val = "The State field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.city))
                        {
                            errorList.Add(new Errorkey { Key = "city", Val = "The city field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Countryname))
                        {
                            errorList.Add(new Errorkey { Key = "Countryname", Val = "The Countryname field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.VerificationId))
                        {
                            errorList.Add(new Errorkey { Key = "VerificationId", Val = "The VerificationId field is required" });
                        }

                        response = response.Create(false, ResponseMessages.USER_NOT_REGISTERED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                        return _iHttpActionResult;
                    }
                    result = await _verifyNinService.AddUserOtherDetail(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(false, ResponseMessages.PROFILE_UPDATED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.PROFILE_NOT_UPDATED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 3:
                            response = response.Create(false, ResponseMessages.OTHER_DETAIL_NOT_UPDATED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                       
                        default:
                            response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
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
