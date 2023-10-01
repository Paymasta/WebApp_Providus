using PayMasta.API.Filters;
using PayMasta.Service.Common;
using PayMasta.ViewModel;
using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.NotificationsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.Common
{
    [RoutePrefix("api/CommonController")]
    public class CommonController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private readonly ICommonService _commonService;
        private Converter _converter;
        public CommonController(ICommonService commonService)
        {
            //  _logUtils = logUtils;
            _commonService = commonService;
            _converter = new Converter();
        }
        [HttpGet]
        [Route("GetTicket")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UserModel>))]
        public async Task<IHttpActionResult> GetTicket()
        {
            var response = new Response<InvoiceNumberResponse>();
            var result = new InvoiceNumberResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetInvoiceNumber();
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
        [Route("GetCategories")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetCategoryResponse>))]
        public async Task<IHttpActionResult> GetCategories()
        {
            var response = new Response<GetCategoryResponse>();
            var result = new GetCategoryResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetCategories();
                    if (result.IsSuccess == true)
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
        [Route("GetNotification")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetNotificationsResponse>))]
        public async Task<IHttpActionResult> GetNotification(NotificationsRequest request)
        {
            var response = new Response<GetNotificationsResponse>();
            var result = new GetNotificationsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetNotificationByUserGuid(request);
                    if (result.IsSuccess == true)
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
        [Route("UpdateNotificationAsRead")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UpdateNotificationsResponse>))]
        public async Task<IHttpActionResult> UpdateNotificationAsRead(NotificationsRequest request)
        {
            var response = new Response<UpdateNotificationsResponse>();
            var result = new UpdateNotificationsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.UpdateNotificationByUserGuid(request);
                    if (result.IsSuccess == true)
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
        [Route("InsertDailyEarningByScheduler")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<UpdateNotificationsResponse>))]
        public async Task<IHttpActionResult> InsertDailyEarningByScheduler(RequestModel request)
        {
            var response = new Response<UpdateNotificationsResponse>();
            var result = new UpdateNotificationsResponse();

            if (ModelState.IsValid)
            {
                try
                {   //for encrypt request or response
                    var requestModel1 = new EncrDecr<EncryptedParamsRequestModel>().Decrypt(request.Value);
                    if (requestModel1.EncryptedParams == "RajdeepPayMasta")
                    {

                        result = await _commonService.InsertDailyEarningByScheduler();
                        if (result.IsSuccess == true)
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
        [Route("GetFaq")]
        [JWTAuthorization(RoleSettings.NoAction, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<List<FAQResponse>>))]
        public async Task<IHttpActionResult> GetFaq()
        {
            var response = new Response<List<FAQResponse>>();
            var result = new List<FAQResponse>();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetFaq();
                    if (result.Count > 0)
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
        [Route("GetPrivacyPolicy")]
        [JWTAuthorization(RoleSettings.NoAction, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<CmsResponse>))]
        public async Task<IHttpActionResult> GetPrivacyPolicy()
        {
            var response = new Response<CmsResponse>();
            var result = new CmsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetPrivacyPolicy();
                    if (result.IsSuccess == true)
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
        [Route("GetTermAndCondition")]
        [JWTAuthorization(RoleSettings.NoAction, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<CmsResponse>))]
        public async Task<IHttpActionResult> GetTermAndCondition()
        {
            var response = new Response<CmsResponse>();
            var result = new CmsResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.GetTermAndCondition();
                    if (result.IsSuccess == true)
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
        [Route("GetMoviesList")]
        [ResponseType(typeof(Response<object>))]
        public async Task<IHttpActionResult> GetMoviesList()
        {
            var response = new Response<object>();
            var result = new object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.MovieList();
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
        [Route("YourMethod")]
       // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<object>))]
        public async Task<IHttpActionResult> YourMethod()
        {
            var response = new Response<object>();
            var result = new object();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonService.YourMethod();
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
