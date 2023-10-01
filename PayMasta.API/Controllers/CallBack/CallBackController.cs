using PayMasta.Service.CallBack;
using PayMasta.ViewModel;
using PayMasta.ViewModel.CallBack;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.CallBack
{
    [RoutePrefix("api/CallBackController")]
    public class CallBackController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private ICallBackService _callBackService;
        private Converter _converter;

        public CallBackController(ICallBackService callBackService)
        {
            //  _logUtils = logUtils;
            _callBackService = callBackService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("WidgetCallBackResponse")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<WidgetResponse>))]
        public async Task<IHttpActionResult> WidgetCallBackResponse(AuthWidgetResponse backResponse)
        {
            var response = new Response<WidgetResponse>();
            var result = new WidgetResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _callBackService.WidgetCallBack(backResponse);
                    switch (result.RstKey)
                    {
                        case 1:
                            response = response.Create(false, ResponseMessages.SUCCESS, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 2:
                            response = response.Create(false, ResponseMessages.AGGREGATOR_FAILED_ERROR, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        case 11:
                            response = response.Create(true, ResponseMessages.USER_REGISTERED, null, result);
                            _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                            break;
                        default:
                            response = response.Create(false, ResponseMessages.INVALID_REQUEST, null, result);
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
