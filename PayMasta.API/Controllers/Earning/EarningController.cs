using PayMasta.API.Filters;
using PayMasta.Service.Earning;
using PayMasta.Service.Employer.EmployeeTransaction;
using PayMasta.Service.VirtualAccount;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using PayMasta.ViewModel.Employer.EWAVM;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.VirtualAccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.Earning
{
    [RoutePrefix("api/EarningController")]
    public class EarningController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IEarningService _earningService;
        private IEmployeeTransactionService _employeeTransactionService;
        private IVirtualAccountService _virtualAccountService;
        private Converter _converter;
        public EarningController(IEarningService earningService, IEmployeeTransactionService employeeTransactionService, IVirtualAccountService virtualAccountService)
        {
            //  _logUtils = logUtils;
            _earningService = earningService;
            _converter = new Converter();
            _employeeTransactionService = employeeTransactionService;
            _virtualAccountService= virtualAccountService;
        }
        [HttpPost]
        [Route("GetEarnings")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EarningResponseForWeb>))]
        public async Task<IHttpActionResult> GetEarnings(EarningRequest request)
        {
            var response = new Response<EarningResponseForWeb>();
            var result = new EarningResponseForWeb();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetEarnings(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
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
        [Route("TransactionHistory")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<TransactionHistoryResponse>))]
        public async Task<IHttpActionResult> TransactionHistory(GetTransactionHistoryRequest request)
        {
            var response = new Response<TransactionHistoryResponse>();
            var result = new TransactionHistoryResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetTransactionHistory(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("GetTransactionByWalletTransactionId")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<TransactionHistoryResponse>))]
        public async Task<IHttpActionResult> GetTransactionByWalletTransactionId(RemoveUpComingBillsRequest request)
        {
            var response = new Response<TransactionResponse>();
            var result = new TransactionResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetTransactionByWalletTransactionId(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("AccessAmountRequest")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<AccessAmountResponse>))]
        public async Task<IHttpActionResult> AccessAmountRequest(AccessdAmountRequest request)
        {
            var response = new Response<AccessAmountResponse>();
            var result = new AccessAmountResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.AccessAmountRequest(request);
                    if (result.RstKey == 1)
                    {
                        response = response.Create(true, ResponseMessages.REQUEST_SENTTO_ADMIN, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 2)
                    {
                        response = response.Create(false, ResponseMessages.ACCESS_AMOUTN_ERROR, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 3)
                    {
                        response = response.Create(false, ResponseMessages.USER_NOT_EXIST, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 4)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 5)
                    {
                        response = response.Create(false, ResponseMessages.EWA_REQUEST_PENDING, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 8)
                    {
                        response = response.Create(false, result.Message, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 9)
                    {
                        response = response.Create(false, result.Message, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else if (result.RstKey == 31)
                    {
                        response = response.Create(false, ResponseMessages.INSUFICIENT_BALANCE, null, result);
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
        [Route("TodaysHistory")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<TodayTransactionHistoryResponse>))]
        public async Task<IHttpActionResult> TodaysHistory(GetTodayTransactionHistoryRequest request)
        {
            var response = new Response<TodayTransactionHistoryResponse>();
            var result = new TodayTransactionHistoryResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetTodaysTransactionHistory(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("UpcomingBilsList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UpComingBillsResponse>))]
        public async Task<IHttpActionResult> UpcomingBilsList(UpComingBillsRequest request)
        {
            var response = new Response<UpComingBillsResponse>();
            var result = new UpComingBillsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetUpcomingBillsHistory(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("RemoveBillfromUpcomingBilsList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<RemoveUpComingBillsResponse>))]
        public async Task<IHttpActionResult> RemoveBillfromUpcomingBilsList(RemoveUpComingBillsRequest request)
        {
            var response = new Response<RemoveUpComingBillsResponse>();
            var result = new RemoveUpComingBillsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.RemoveBillfromUpcomingBilsList(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATAUPDATED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
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


        [HttpGet]
        [Route("GetCommisions")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetCommissionResponse>))]
        public async Task<IHttpActionResult> GetCommisions()
        {
            var response = new Response<GetCommissionResponse>();
            var result = new GetCommissionResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetCommisions();
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.DATA_NOT_RECEIVED, null, result);
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
        [Route("GetEmployeesEwaRequestDetail")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeeEwaDetailReponse>))]
        public async Task<IHttpActionResult> GetEmployeesEwaRequestDetail(EmployeesEWAWithdrawlsRequest request)
        {
            var response = new Response<EmployeeEwaDetailReponse>();
            var result = new EmployeeEwaDetailReponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _employeeTransactionService.GetEmployeesEwaRequestDetail(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
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
        [Route("GetEmployeesWithdrwals")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<EmployeesWithdrawlsResponseForApp>))]
        public async Task<IHttpActionResult> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request)
        {
            var response = new Response<EmployeesWithdrawlsResponseForApp>();
            var result = new EmployeesWithdrawlsResponseForApp();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _employeeTransactionService.GetEmployeesWithdrwalsForApp(request);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
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
        [Route("GetVirtualAccountBalance")]
       // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<CurrentBalanceResponse>))]
        public async Task<IHttpActionResult> GetVirtualAccountBalance(EmployeesEWAWithdrawlsRequest request)
        {
            var response = new Response<CurrentBalanceResponse>();
            var result = new CurrentBalanceResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
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
        [Route("GetAddedBankList")]
       // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetAddedBanListResponse>))]
        public async Task<IHttpActionResult> GetAddedBankList(EmployeesEWAWithdrawlsRequest request)
        {
            var response = new Response<GetAddedBanListResponse>();
            var result = new GetAddedBanListResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _earningService.GetAddedBankList(request.UserGuid);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(true, ResponseMessages.DATA_RECEIVED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.REQUESTDATA_NOT_EXIST, null, result);
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
