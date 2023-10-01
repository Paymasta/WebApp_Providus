using PayMasta.API.Filters;
using PayMasta.Service.ManageFinance;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ManageFinanceVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMasta.API.Controllers.ManageFinance
{
    [RoutePrefix("api/ManageFinanceController")]
    public class ManageFinanceController : ApiController
    {
        private IHttpActionResult _iHttpActionResult;
        private IManageFinanceService _manageFinance;
        private Converter _converter;
        public ManageFinanceController(IManageFinanceService manageFinanceService)
        {
            //  _logUtils = logUtils;
            _manageFinance = manageFinanceService;
            _converter = new Converter();
        }
        [HttpPost]
        [Route("GetPiChartData")]
        [JWTAuthorization(RoleSettings.Allow, new EnumUserType[] { EnumUserType.All })]
        [ResponseType(typeof(Response<GetManageFinanceResponse>))]
        public async Task<IHttpActionResult> GetPiChartData(ManageFinanceRequest request)
        {
            var response = new Response<GetManageFinanceResponse>();
            var result = new GetManageFinanceResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _manageFinance.GetPiChartData(request);
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
    }
}
