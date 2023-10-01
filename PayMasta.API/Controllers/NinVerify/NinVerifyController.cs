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

namespace PayMasta.API.Controllers.NinVerify
{
    [RoutePrefix("api/NinVerifyController")]
    public class NinVerifyController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IVerifyNinService _verifyNinService;
        private Converter _converter;
        public NinVerifyController(IVerifyNinService verifyNinService)
        {
            //  _logUtils = logUtils;
            _verifyNinService = verifyNinService;
            _converter = new Converter();
        }

        [HttpPost]
        [Route("RegisterByNuban")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<QoreIdBvnNubanResult>))]
        public async Task<IHttpActionResult> RegisterByNuban(RegisterByNubanRequest request)
        {
            var response = new Response<QoreIdBvnNubanResult>();
            var result = new QoreIdBvnNubanResult();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Firstname) || string.IsNullOrWhiteSpace(request.Lastname) || string.IsNullOrWhiteSpace(request.BankCode) || string.IsNullOrWhiteSpace(request.AccountNumber))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.BankCode))
                        {
                            errorList.Add(new Errorkey { Key = "bankCode", Val = "The bankCode field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Firstname))
                        {
                            errorList.Add(new Errorkey { Key = "Firstname", Val = "The Firstname field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Lastname))
                        {
                            errorList.Add(new Errorkey { Key = "Lastname", Val = "The Lastname field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.AccountNumber))
                        {
                            errorList.Add(new Errorkey { Key = "accountNumber", Val = "The accountNumber field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.AccountNumber))
                        {
                            errorList.Add(new Errorkey { Key = "accountNumber", Val = "The accountNumber field is required" });
                        }

                        response = response.Create(false, ResponseMessages.FAILED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        result = await _verifyNinService.VerifyNuban(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, result.Message, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 7:
                                response = response.Create(false, ResponseMessages.EXIST_EMAIL, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 8:
                                response = response.Create(false, ResponseMessages.EXIST_MOBILE_NO, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.FAILED, null, result);
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

        [HttpGet]
        [Route("GetBankListForRegister")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<BankListResult>))]
        public async Task<IHttpActionResult> GetBankListForRegister()
        {
            var response = new Response<BankListResult>();
            var result = new BankListResult();

            if (ModelState.IsValid)
            {
                try
                {

                    result = await _verifyNinService.BankListForRegister();
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, result.Message, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.FAILED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
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
        [Route("VerifyOTP")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<IsOtpVerifiedResponse>))]
        public async Task<IHttpActionResult> VerifyOTP(VerifyOTPRequest request)
        {
            var response = new Response<IsOtpVerifiedResponse>();
            var result = new IsOtpVerifiedResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _verifyNinService.VerifyOTPWeb(request);
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
        [Route("AddUserOtherDetail")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<AddOtherDetailResponse>))]
        public async Task<IHttpActionResult> AddUserOtherDetail(AddOtherDetailPRequest request)
        {
            var response = new Response<AddOtherDetailResponse>();
            var result = new AddOtherDetailResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _verifyNinService.AddUserOtherDetail(request);
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
        [Route("RegisterByVNin")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<QoreIdBvnNubanResult>))]
        public async Task<IHttpActionResult> RegisterByVNin(RegisterByVNinRequest request)
        {
            var response = new Response<QoreIdBvnNubanResult>();
            var result = new QoreIdBvnNubanResult();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Vnin) || string.IsNullOrWhiteSpace(request.Firstname) || string.IsNullOrWhiteSpace(request.Lastname) || string.IsNullOrWhiteSpace(request.BankCode) || string.IsNullOrWhiteSpace(request.AccountNumber))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.BankCode))
                        {
                            errorList.Add(new Errorkey { Key = "bankCode", Val = "The bankCode field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Firstname))
                        {
                            errorList.Add(new Errorkey { Key = "Firstname", Val = "The Firstname field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Lastname))
                        {
                            errorList.Add(new Errorkey { Key = "Lastname", Val = "The Lastname field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.AccountNumber))
                        {
                            errorList.Add(new Errorkey { Key = "accountNumber", Val = "The accountNumber field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.AccountNumber))
                        {
                            errorList.Add(new Errorkey { Key = "accountNumber", Val = "The accountNumber field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Vnin))
                        {
                            errorList.Add(new Errorkey { Key = "Vnin", Val = "The Vnin field is required" });
                        }

                        response = response.Create(false, ResponseMessages.FAILED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        result = await _verifyNinService.VerifyVnin(request);
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, result.Message, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 7:
                                response = response.Create(false, ResponseMessages.EXIST_EMAIL, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 8:
                                response = response.Create(false, ResponseMessages.EXIST_MOBILE_NO, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.FAILED, null, result);
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
    }
}
