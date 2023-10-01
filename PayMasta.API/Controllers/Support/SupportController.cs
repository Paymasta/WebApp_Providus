using PayMasta.API.Filters;
using PayMasta.Service.Support;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.Support
{
    [RoutePrefix("api/SupportController")]
    public class SupportController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private ISupportService _supportService;
        private Converter _converter;

        public SupportController(ISupportService supportService)
        {
            //  _logUtils = logUtils;
            _supportService = supportService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("InsertSupportTicket")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<SupportResponse>))]
        public async Task<IHttpActionResult> InsertSupportTicket(SupportRequest request)
        {
            var response = new Response<SupportResponse>();
            var result = new SupportResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (request.UserGuid == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.DescriptionText) || string.IsNullOrWhiteSpace(request.TicketNumber))
                    {
                        var errorList = new List<Errorkey>();

                        if (string.IsNullOrWhiteSpace(request.DescriptionText))
                        {
                            errorList.Add(new Errorkey { Key = "DescriptionText", Val = "The DescriptionText field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.Title))
                        {
                            errorList.Add(new Errorkey { Key = "Title", Val = "The Title field is required" });
                        }
                        if (string.IsNullOrWhiteSpace(request.TicketNumber))
                        {
                            errorList.Add(new Errorkey { Key = "TicketNumber", Val = "The TicketNumber field is required" });
                        }
                        if (request.UserGuid == null)
                        {
                            errorList.Add(new Errorkey { Key = "UserGuid", Val = "The UserGuid field is required" });
                        }

                        response = response.Create(false, ResponseMessages.USER_NOT_REGISTERED, errorList, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
                    }


                    result = await _supportService.InsertSupportTicket(request);
                    if (result != null)
                    {
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.DATA_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
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

        [HttpPost]
        [Route("GetSupportDetailList")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<SupportMasterResponse>))]
        public async Task<IHttpActionResult> GetSupportDetailList(GetSupportListRequest request)
        {
            var response = new Response<SupportMasterResponse>();
            var result = new SupportMasterResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _supportService.GetSupportDetailList(request);
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
    }
}
