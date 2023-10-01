using PayMasta.Service.Account;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.Lidya;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PayMasta.ViewModel.LidyaVM;

namespace PayMasta.API.Controllers.LidyaService
{
    [RoutePrefix("api/AccountController")]
    public class LidyaServiceController : ApiController
    {
        // private readonly ILogUtils _logUtils;
        private IHttpActionResult _iHttpActionResult;
        private ILidyaService _lidyaService;
        private Converter _converter;
        public LidyaServiceController(ILidyaService lidyaService)
        {
            //  _logUtils = logUtils;
            _lidyaService = lidyaService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("CreateMandat")]
        //[JWTAuthorization(RoleSettings.NoAction)]
        [ResponseType(typeof(Response<LoginResponse>))]
        public async Task<IHttpActionResult> CreateMandat(MandateRequest request)
        {
            var response = new Response<MandateResponse>();
            var result = new MandateResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    result = await _lidyaService.CreateMandat(request);
                    if (result != null)
                    {
                        switch (result.RstKey)
                        {
                            case 1:
                                response = response.Create(true, ResponseMessages.SUCCESS, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 2:
                                response = response.Create(false, ResponseMessages.LOGIN_FAILED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 3:
                                response = response.Create(false, ResponseMessages.NO_EMAIL_RECORD_FOUND, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            case 4:
                                response = response.Create(false, ResponseMessages.EXCEPTION_OCCURED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                            default:
                                response = response.Create(false, ResponseMessages.EXCEPTION_OCCURED, null, result);
                                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.OK);
                                break;
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    response = response.Create(false, ResponseMessages.LOGIN_FAILED, null, result);
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
                response = response.Create(false, ResponseMessages.LOGIN_FAILED, errorList, result);
                _iHttpActionResult = _converter.ApiResponseMessage(response, HttpStatusCode.InternalServerError);
            }
            return _iHttpActionResult;
        }
    }
}
