using PayMasta.Service.ItexRechargeService;
using PayMasta.Service.ItexService;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.ViewModel.ProvidusExpresssWalletVM;
using PayMasta.ViewModel.WalletToBankVM;
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
    public class ExpressWalletServicesController : MyBaseController
    {
        private IProvidusExpresssWalletService _providusExpresssWalletService;
        //  private IItexRechargeService _itexRechargeService;
        public ExpressWalletServicesController(IProvidusExpresssWalletService providusExpresssWalletService)
        {
            _providusExpresssWalletService = providusExpresssWalletService;
        }
        // GET: ExpressWalletServices
        [HttpPost]
        public async Task<JsonResult> WalletToWalletTransfer(WalletToWalletRequest request)
        {
            var res = new GetExpressWalletToBankResponse();

            try
            {
                res = await _providusExpresssWalletService.WalletToWalletTransfer(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetExpressBankList(Guid request)
        {
            var res = new GetExpressBankListResponse();

            try
            {
                res = await _providusExpresssWalletService.GetExpressBankList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> WalletToBankTransfer(WalletToBankPaymentRequest request)
        {
            var res = new GetExpressWalletToBankRes();

            try
            {
                res = await _providusExpresssWalletService.WalletToBankTransfer(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> VerifyAccount(string bankCode, string accountNumber)
        {
            var res = new VerifyExpressBankAccount();

            try
            {
                res = await _providusExpresssWalletService.VerifyAccount(bankCode, accountNumber);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}