using PayMasta.API.Filters;
using PayMasta.Service.ZealvendService.DataBundle;
using PayMasta.Service.ZealvendService.Electricity;
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
using PayMasta.ViewModel.ZealvendBillsVM;
using PayMasta.ViewModel.ElectricityPurchaseVM;

namespace PayMasta.API.Controllers.Zealvend
{
    [RoutePrefix("api/ElectricityController")]
    public class ElectricityController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IElectricityService _electricityService;
        private Converter _converter;
        public ElectricityController(IElectricityService electricityService)
        {
            _converter = new Converter();
            _electricityService = electricityService;
        }
        [HttpPost]
        [Route("GetElectricityOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<OperatorResponse>))]
        public async Task<IHttpActionResult> GetElectricityOperatorList(OperatorRequest operatorRequest)
        {
            var response = new Response<OperatorResponse>();
            var result = new OperatorResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _electricityService.GetElectricityOperatorList(operatorRequest);
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
        [Route("MeterVerify")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<MeterVerifyResponse>))]
        public async Task<IHttpActionResult> MeterVerify(VerifyRequest operatorRequest)
        {
            var response = new Response<MeterVerifyResponse>();
            var result = new MeterVerifyResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _electricityService.MeterVerify(operatorRequest);
                    if (result.status != null)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    //need to check this condition
                    else if (result.status != null)
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
        [Route("ElectricityRechargePayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetZealElectricityRechargeResponse>))]
        public async Task<IHttpActionResult> ElectricityRechargePayment(ElectricityBillPaymentRequest operatorRequest)
        {
            var response = new Response<GetZealElectricityRechargeResponse>();
            var result = new GetZealElectricityRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _electricityService.ElectricityRechargePayment(operatorRequest);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(true, AdminResponseMessages.DATA_FOUND, null, result);
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
