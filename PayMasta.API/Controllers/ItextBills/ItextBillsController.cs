using PayMasta.API.Filters;
using PayMasta.Service.ItexService;
using PayMasta.ViewModel;
using PayMasta.ViewModel.BouquetsVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ElectricityVM;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.InternetVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ValidateMultichoiceVM;
using PayMasta.ViewModel.ValidateStartimesVM;
using PayMasta.ViewModel.ValidatInternetVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.ItextBills
{
    [RoutePrefix("api/ItextBillsController")]
    public class ItextBillsController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IItexService _itexService;
        private Converter _converter;
        public ItextBillsController(IItexService itexService)
        {
            //  _logUtils = logUtils;
            _itexService = itexService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("GetiTextAirtimeOperatorList")]
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
        [Route("GetiTextDataOperatorList")]
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
                    result = await _itexService.GetDataOperatorList(operatorRequest);
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
        [Route("GetiTextTvOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<OperatorResponse>))]
        public async Task<IHttpActionResult> GetTvOperatorList(OperatorRequest operatorRequest)
        {
            var response = new Response<OperatorResponse>();
            var result = new OperatorResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetTvOperatorList(operatorRequest);
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
        [Route("GetiTextInternetOperatorList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<OperatorResponse>))]
        public async Task<IHttpActionResult> GetInternetOperatorList(OperatorRequest operatorRequest)
        {
            var response = new Response<OperatorResponse>();
            var result = new OperatorResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetInternetOperatorList(operatorRequest);
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
        [Route("GetiTextElectricityOperatorList")]
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
                    result = await _itexService.GetElectricityOperatorList(operatorRequest);
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
        [Route("GetDataOperatorPlanList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetPlanListResponses>))]
        public async Task<IHttpActionResult> GetDataOperatorPlanList(OperatorPlanRequest operatorRequest)
        {
            var response = new Response<GetPlanListResponses>();
            var result = new GetPlanListResponses();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetDataOperatorPlanList(operatorRequest);
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
        [Route("MeterValidate")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetValidateMeterResponseResponses>))]
        public async Task<IHttpActionResult> MeterValidate(MeterValidateRequest operatorRequest)
        {
            var response = new Response<GetValidateMeterResponseResponses>();
            var result = new GetValidateMeterResponseResponses();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.MeterValidate(operatorRequest);
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
        [Route("MultichoiceBouquets")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetBouquestResponses>))]
        public async Task<IHttpActionResult> MultichoiceBouquets(GetBouquetDataRequest operatorRequest)
        {
            var response = new Response<GetBouquestResponses>();
            var result = new GetBouquestResponses();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.MultichoiceBouquets(operatorRequest);
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
        [Route("ValidateMultichoice")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetValidateBouquestResponses>))]
        public async Task<IHttpActionResult> ValidateMultichoice(GetValidatBouquetDataRequest operatorRequest)
        {
            var response = new Response<GetValidateBouquestResponses>();
            var result = new GetValidateBouquestResponses();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.ValidateMultichoice(operatorRequest);
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
        [Route("ValidateStartimes")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetValidateStarTimesResponses>))]
        public async Task<IHttpActionResult> ValidateStartimes(GetValidatStarTimesDataRequest operatorRequest)
        {
            var response = new Response<GetValidateStarTimesResponses>();
            var result = new GetValidateStarTimesResponses();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.ValidateStartimes(operatorRequest);
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
        [Route("GetInternetBundles")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetInternetBundlesResponse>))]
        public async Task<IHttpActionResult> GetInternetBundles(GetInternetBundlesRequest operatorRequest)
        {
            var response = new Response<GetInternetBundlesResponse>();
            var result = new GetInternetBundlesResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetInternetBundles(operatorRequest);
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
        [Route("GetBankList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<BankListResponse>))]
        public async Task<IHttpActionResult> GetBankList(OperatorRequest operatorRequest)
        {
            var response = new Response<BankListResponse>();
            var result = new BankListResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _itexService.GetBankList(operatorRequest);
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

        //[HttpPost]
        //[Route("GetInternetValidation")]
        //// [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        //[ResponseType(typeof(Response<GetValidateInternetResponse>))]
        //public async Task<IHttpActionResult> GetInternetValidation(GetInternetValidateRequest operatorRequest)
        //{
        //    var response = new Response<GetValidateInternetResponse>();
        //    var result = new GetValidateInternetResponse();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            result = await _itexService.GetInternetValidation(operatorRequest);
        //            if (result.RstKey == 1)
        //            {
        //                response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
        //                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
        //            }
        //            else if (result.RstKey == 2)
        //            {
        //                response = response.Create(true, AdminResponseMessages.DATA_NOT_FOUND_GENERIC, null, result);
        //                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
    }
}
