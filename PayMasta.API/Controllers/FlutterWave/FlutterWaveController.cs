using PayMasta.API.Filters;
using PayMasta.Service.FlutterWave;
using PayMasta.ViewModel;
using PayMasta.ViewModel.BulkPaymentVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.FlutterWaveVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.FlutterWave
{
    [RoutePrefix("api/FlutterWaveController")]
    public class FlutterWaveController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IFlutterWaveService _flutterWaveService;
        private Converter _converter;
        public FlutterWaveController(IFlutterWaveService flutterWaveService)
        {
            //  _logUtils = logUtils;
            _flutterWaveService = flutterWaveService;
            _converter = new Converter();
        }
        [HttpGet]
        [Route("GetAirtimeOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AirtimeResponse>))]
        public async Task<IHttpActionResult> GetAirtimeOperatorList()
        {
            var response = new Response<AirtimeResponse>();
            var result = new AirtimeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.GetAirtimeOperatorList();
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

        [HttpGet]
        [Route("GetDataBundleOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AirtimeResponse>))]
        public async Task<IHttpActionResult> GetDataBundleOperatorList()
        {
            var response = new Response<AirtimeResponse>();
            var result = new AirtimeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.GetDataBundleOperatorList();
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


        [HttpGet]
        [Route("GetWifiInternetOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AirtimeResponse>))]
        public async Task<IHttpActionResult> GetWifiInternetOperatorList()
        {
            var response = new Response<AirtimeResponse>();
            var result = new AirtimeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.GetWifiInternetOperatorList();
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

        [HttpGet]
        [Route("GetTvOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AirtimeResponse>))]
        public async Task<IHttpActionResult> GetTvOperatorList()
        {
            var response = new Response<AirtimeResponse>();
            var result = new AirtimeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.GetTVOperatorList();
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

        [HttpGet]
        [Route("GetElectricityOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AirtimeResponse>))]
        public async Task<IHttpActionResult> GetElectricityOperatorList()
        {
            var response = new Response<AirtimeResponse>();
            var result = new AirtimeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.GetElectricityOperatorList();
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
        [Route("FilterOperatorByBillerCode")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<ProductResponse>))]
        public async Task<IHttpActionResult> FilterOperatorByBillerCode(WalletServiceFilterRequest request)
        {
            var response = new Response<ProductResponse>();
            var result = new ProductResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.FilterOperatorByBillerCode(request.BillerCode);
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
        [Route("AirtimeBillPayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.AirtimeRechargeResponse>))]
        public async Task<IHttpActionResult> AirtimeBillPayment(AirtimeRechargeRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.AirtimeRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.AirtimeRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.AirtimeRecharge(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
        [Route("TvBillPayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.TVRechargeResponse>))]
        public async Task<IHttpActionResult> TvBillPayment(AirtimeRechargeRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.TVRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.TVRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.TVRecharge(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
        [Route("DataBundleBillPayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse>))]
        public async Task<IHttpActionResult> DataBundleBillPayment(AirtimeRechargeRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.InternetRecharge(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
        [Route("WifiInternetBillPayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse>))]
        public async Task<IHttpActionResult> WifiInternetBillPayment(AirtimeRechargeRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.DataBundleRecharge(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
        [Route("ElectriCityBillRecharge")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse>))]
        public async Task<IHttpActionResult> ElectriCityBillRecharge(AirtimeRechargeRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.ElectriCityBillRecharge(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
        [Route("BulkBillPayment")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<PayMasta.ViewModel.AirtimeVM.BulkBillRechargeResponse>))]
        public async Task<IHttpActionResult> BulkBillPayment(BulkPaymentRequest request)
        {
            var response = new Response<PayMasta.ViewModel.AirtimeVM.BulkBillRechargeResponse>();
            var result = new PayMasta.ViewModel.AirtimeVM.BulkBillRechargeResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _flutterWaveService.BulkBillPayment(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(true, ResponseMessages.TRANSACTION_DONE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(true, ResponseMessages.PENDING_TRANSACTION, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 6)
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.TRANSACTION_FAIL, null, result);
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
