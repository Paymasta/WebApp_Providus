using PayMasta.Service.Earning;
using PayMasta.Service.ManageFinance;
using PayMasta.Service.VirtualAccount;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.ManageFinanceVM;
using PayMasta.ViewModel.VirtualAccountVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    [CustomAuthorize(Roles = "Employee")]
    [SessionExpireFilter]
    public class ManageFinanceController : MyBaseController
    {
        private IManageFinanceService _manageFinance;
        private IEarningService _earningService;
        private IVirtualAccountService _virtualAccountService;
        public ManageFinanceController(IManageFinanceService manageFinance, IEarningService earningService, IVirtualAccountService virtualAccountService)
        {
            _manageFinance=manageFinance;
            _earningService=earningService;
            _virtualAccountService=virtualAccountService;
        }
        // GET: ManageFinance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpComingBill()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetPiChartData(ManageFinanceRequest request)
        {
            var res = new GetManageFinanceResponse();

            try
            {
                res = await _manageFinance.GetPiChartData(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetUpcomingBillsHistory(UpComingBillsRequest request)
        {
            var res = new UpComingBillsResponse();

            try
            {
                res = await _earningService.GetUpcomingBillsHistory(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetTodaysTransactionHistory(GetTodayTransactionHistoryRequest request)
        {
            var res = new TodayTransactionHistoryResponse();

            try
            {
                res = await _earningService.GetTodaysTransactionHistory(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveBillfromUpcomingBilsList(RemoveUpComingBillsRequest request)
        {
            var res = new RemoveUpComingBillsResponse();

            try
            {
                res = await _earningService.RemoveBillfromUpcomingBilsList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetTransactionByWalletTransactionId(RemoveUpComingBillsRequest request)
        {
            var res = new TransactionResponse();

            try
            {
                res = await _earningService.GetTransactionByWalletTransactionId(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        //[HttpPost]
        //public async Task<JsonResult> GetVirtualAccountBalance(Guid userGuid)
        //{
        //    var res = new GetVirtualAccountBalanceResponse();

        //    try
        //    {
        //        res = await _virtualAccountService.GetVirtualAccountBalance(userGuid);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}
    }
}