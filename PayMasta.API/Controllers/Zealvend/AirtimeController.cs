using PayMasta.API.Filters;
using PayMasta.Service.ItexService;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PayMasta.Service.ZealvendService;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.ZealvendBillsVM;

namespace PayMasta.API.Controllers.Zealvend
{
    [RoutePrefix("api/AirtimeController")]
    public class AirtimeController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IItexService _itexService;
        private IZealvendBillService _zealvendBillService;
        private Converter _converter;
        public AirtimeController(IItexService itexService, IZealvendBillService zealvendBillService)
        {
            //  _logUtils = logUtils;
            _itexService = itexService;
            _converter = new Converter();
            _zealvendBillService = zealvendBillService;
        }
        [HttpPost]
        [Route("GetAirtimeOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<OperatorResponse>))]
        public async Task<IHttpActionResult> GetAirtimeOperatorList(OperatorRequest operatorRequest)
        {
            var response = new Response<OperatorResponse>();
            var result = new OperatorResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetAirtimeOperatorList(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("AirtimePayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetAirtimeZealvendResponse>))]
        public async Task<IHttpActionResult> AirtimePayment(VTUBillPaymentRequest operatorRequest)
        {
            var response = new Response<GetAirtimeZealvendResponse>();
            var result = new GetAirtimeZealvendResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _zealvendBillService.ZealvendAirtimePayment(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.NotFound);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.NoContent);
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
