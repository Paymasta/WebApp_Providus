using PayMasta.Service.ZealvendService;
using PayMasta.Service.ZealvendService.DataBundle;
using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PayMasta.Web.Controllers.ZealVendBills
{
    [CustomAuthorize(Roles = "Employee")]
    [SessionExpireFilter]
    public class DataPaybillsController : MyBaseController
    {
        private IZealvendDataService _zealvendDataService;
        public DataPaybillsController(IZealvendDataService zealvendDataService)
        {

            _zealvendDataService = zealvendDataService;
        }
        [HttpPost]
        public async Task<JsonResult> GetDataOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _zealvendDataService.GetDataOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetDataOperatorPlanList(OperatorPlanRequest request)
        {
            var res = new GetDataResponse();

            try
            {
                res = await _zealvendDataService.GetDataOperatorPlanList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> DataRechargePayment(DataBillPaymentRequest request)
        {
            var res = new GetZealvendDataRechargeResponse();

            try
            {
                res = await _zealvendDataService.DataRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}