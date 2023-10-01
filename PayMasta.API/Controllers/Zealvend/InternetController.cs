using PayMasta.API.Filters;
using PayMasta.Service.ItexService;
using PayMasta.Service.ZealvendService;
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
using PayMasta.Service.ZealvendService.DataBundle;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using PayMasta.ViewModel.DataRechargeVM;

namespace PayMasta.API.Controllers.Zealvend
{
    [RoutePrefix("api/InternetController")]
    public class InternetController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IZealvendDataService _zealvendDataService;
        private Converter _converter;
        public InternetController(IItexService itexService, IZealvendDataService zealvendDataService)
        {
            //  _logUtils = logUtils;
            _converter = new Converter();
            _zealvendDataService = zealvendDataService;
        }
        [HttpPost]
        [Route("GetDataOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<OperatorResponse>))]
        public async Task<IHttpActionResult> GetDataOperatorList(OperatorRequest operatorRequest)
        {
            var response = new Response<OperatorResponse>();
            var result = new OperatorResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _zealvendDataService.GetDataOperatorList(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
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

        [HttpPost]
        [Route("GetDataOperatorPlanList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetDataResponse>))]
        public async Task<IHttpActionResult> GetDataOperatorPlanList(OperatorPlanRequest operatorRequest)
        {
            var response = new Response<GetDataResponse>();
            var result = new GetDataResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _zealvendDataService.GetDataOperatorPlanList(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
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

        [HttpPost]
        [Route("DataRechargePayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetZealvendDataRechargeResponse>))]
        public async Task<IHttpActionResult> DataRechargePayment(DataBillPaymentRequest operatorRequest)
        {
            var response = new Response<GetZealvendDataRechargeResponse>();
            var result = new GetZealvendDataRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _zealvendDataService.DataRechargePayment(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
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
