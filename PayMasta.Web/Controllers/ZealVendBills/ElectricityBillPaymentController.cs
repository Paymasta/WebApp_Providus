using PayMasta.Service.ZealvendService.DataBundle;
using PayMasta.Service.ZealvendService.Electricity;
using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.ItexVM;
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
    public class ElectricityBillPaymentController : Controller
    {
        private IElectricityService _electricityService;
        public ElectricityBillPaymentController(IElectricityService electricityService)
        {

            _electricityService = electricityService;
        }
        [HttpPost]
        public async Task<JsonResult> GetElectricityOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _electricityService.GetElectricityOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> MeterVerify(VerifyRequest request)
        {
            var res = new MeterVerifyResponse();

            try
            {
                res = await _electricityService.MeterVerify(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ElectricityRechargePayment(ElectricityBillPaymentRequest request)
        {
            var res = new GetZealElectricityRechargeResponse();

            try
            {
                res = await _electricityService.ElectricityRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}