using PayMasta.Service.ItexRechargeService;
using PayMasta.Service.ItexService;
using PayMasta.Service.ZealvendService;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PayMasta.Web.Controllers
{
    [CustomAuthorize(Roles = "Employee")]
    [SessionExpireFilter]
    public class ZealvendBillsPayController : MyBaseController
    {
      
        private IZealvendBillService _zealvendBillService;
        public ZealvendBillsPayController(IZealvendBillService zealvendBillService)
        {

            _zealvendBillService = zealvendBillService;
        }

        [HttpPost]
        public async Task<JsonResult> AirtimePayment(VTUBillPaymentRequest request)
        {
            var res = new GetAirtimeZealvendResponse();

            try
            {
                res = await _zealvendBillService.ZealvendAirtimePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> PayTvVendPayment(PayTvVendRequestVM request)
        {
            var res = new PayTvVendResponseVM();

            try
            {
                res = await _zealvendBillService.PayTvVendPayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetPayTvProductList(string product)
        {
            var res = new PayTvProductResponseVM();

            try
            {
                res = await _zealvendBillService.GetPayTvProductList(product);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> VerifyPayTv(PayTvVerifyRequest request)
        {
            var res = new PayTvVerifyResponseVM();

            try
            {
                res = await _zealvendBillService.VerifyPayTv(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }


    }
}