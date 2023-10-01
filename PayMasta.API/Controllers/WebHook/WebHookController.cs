using PayMasta.Service.Common;
using PayMasta.Service.WebHookService;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ProvidusBank;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PayMasta.ViewModel.WebHookVM;

namespace PayMasta.API.Controllers.WebHook
{
    [RoutePrefix("api/WebHookController")]
    public class WebHookController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private readonly IWebHookService _webHookService;
        private Converter _converter;
        public WebHookController(IWebHookService webHookService)
        {
            //  _logUtils = logUtils;
            _webHookService = webHookService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("ProvidusWebhook")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<WebHookResponse>))]
        public async Task<IHttpActionResult> ProvidusWebhook(WebHookRequest request)
        {
            var response = new Response<WebHookResponse>();
            var result = new WebHookResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    result = result;// await _commonService.GetProvidusBankResponse(request);
                    if (result != null)
                    {
                        response = response.Create(true, result.responseMessage, null, result);
                        _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = response.Create(false, ResponseMessages.DATA_NOT_SAVED, null, result);
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
