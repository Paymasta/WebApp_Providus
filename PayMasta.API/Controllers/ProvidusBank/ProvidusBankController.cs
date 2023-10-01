using PayMasta.Service.Common;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.ProvidusBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.ProvidusBank
{
    [RoutePrefix("api/ProvidusBankController")]
    public class ProvidusBankController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private readonly ICommonService _commonService;
        private Converter _converter;
        public ProvidusBankController(ICommonService commonService)
        {
            //  _logUtils = logUtils;
            _commonService = commonService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("GetProvidusBankResponse")]
        // [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetProvidusResponse>))]
        public async Task<IHttpActionResult> GetProvidusBankResponse(ProvidusBankRequest request)
        {
            var response = new Response<GetProvidusResponse>();
            var result = new GetProvidusResponse();

            if (ModelState.IsValid)
            {
                try
                {

                    result = await _commonService.GetProvidusBankResponse(request);
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
