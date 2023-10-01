using PayMasta.API.Filters;
using PayMasta.Service.BankTransfer;
using PayMasta.ViewModel;
using PayMasta.ViewModel.BankTransferVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.BankTransfer
{
    [RoutePrefix("api/BankController")]
    public class BankController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IBankTransferService _bankTransferService;
        private Converter _converter;

        public BankController(IBankTransferService bankTransferService)
        {
            //  _logUtils = logUtils;
            _bankTransferService = bankTransferService;
            _converter = new Converter();
        }
        [HttpGet]
        [Route("GetBanks")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetBankListResponse>))]
        public async Task<IHttpActionResult> GetBanks()
        {
            var response = new Response<GetBankListResponse>();
            var result = new GetBankListResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _bankTransferService.GetBanks();
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
        [Route("GetNIPAccount")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetAccountResponse>))]
        public async Task<IHttpActionResult> GetNIPAccount(GetAccountRequest request)
        {
            var response = new Response<GetAccountResponse>();
            var result = new GetAccountResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _bankTransferService.GetNIPAccount(request);
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
        [Route("NIPFundTransfer")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<FundTransferResponse>))]
        public async Task<IHttpActionResult> NIPFundTransfer(FundTransferRequest request)
        {
            var response = new Response<FundTransferResponse>();
            var result = new FundTransferResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _bankTransferService.NIPFundTransfer(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(false, ResponseMessages.PAY_MONEY_SUCCESS, null, result);
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
        [Route("ProvidusFundTransfer")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<ProvidusFundResponse>))]
        public async Task<IHttpActionResult> ProvidusFundTransfer(ProvidusFundTransferRequest request)
        {
            var response = new Response<ProvidusFundResponse>();
            var result = new ProvidusFundResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _bankTransferService.ProvidusFundTransfer(request);

                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(false, ResponseMessages.PAY_MONEY_SUCCESS, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
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
        [Route("GetProvidusAccount")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<ProvidusAccountResponse>))]
        public async Task<IHttpActionResult> GetProvidusAccount(GetAccountRequest request)
        {
            var response = new Response<ProvidusAccountResponse>();
            var result = new ProvidusAccountResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _bankTransferService.GetProvidusAccount(request);
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

    }

}
