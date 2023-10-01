using PayMasta.Service.Earning;
using PayMasta.Service.Okra;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.VirtualAccount;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using PayMasta.ViewModel.OkraAPIVM;
using PayMasta.ViewModel.OkraVM;
using PayMasta.ViewModel.ProvidusExpresssWalletVM;
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
    public class HomeController : MyBaseController
    {
        private IEarningService _earningService;
        private IOkraService _okraService;
        private IVirtualAccountService _virtualAccountService;
        private IProvidusExpresssWalletService _providusExpresssWalletService;
        public HomeController(IEarningService earningService, IOkraService okraService, IVirtualAccountService virtualAccountService)
        {
            _earningService = earningService;
            _okraService = okraService;
            _virtualAccountService=virtualAccountService;
            _providusExpresssWalletService=new ProvidusExpresssWalletService();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetDashboardData(string guid)
        {
            var res = new EarningResponseForWeb();
            try
            {
                var req = new EarningRequest
                {
                    UserGuid = Guid.Parse(guid.ToString())
                };
                res = await _earningService.GetEarnings(req);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpGet]
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult ViewAccountBalance()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AccessAmountRequest(WebAccessdAmountRequest request)
        {
            var res = new AccessAmountResponse();
            try
            {
                var req = new AccessdAmountRequest
                {
                    UserGuid = Guid.Parse(request.UserGuid.ToString()),
                    Amount = request.Amount,
                };

                res = await _earningService.AccessAmountRequest(req);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        //[HttpPost]
        //public async Task<JsonResult> GetAccountBalance(Guid guid, string bankId)
        //{
        //    var result = new CurrentBalanceResponse();
        //    try
        //    {
        //        var id = Guid.Parse(guid.ToString());
        //        result = await _virtualAccountService.GetVirtualAccountBalance(guid);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(result);
        //}
        [HttpPost]
        public async Task<JsonResult> GetAccountBalance(Guid guid, string bankId)
        {
            var result = new CustomerWalletDetailResponse();
            try
            {
                var id = Guid.Parse(guid.ToString());
                result = await _providusExpresssWalletService.GetVirtualAccount(guid);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> GetWidgetLink(string guid)
        {
            var result = new WodgetLinkGenerateResponse();
            try
            {
                var id = Guid.Parse(guid.ToString());
                var req = new WodgetLinkGenerateRequest
                {
                    UserGuid = id
                };
                result = await _okraService.GetWidgetLink(req);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }

        public async Task<JsonResult> GetTransactions(string guid, string bankId)
        {
            var result = new TransactionsWodgetLinkGenerateResponse();
            try
            {
                var id = Guid.Parse(guid.ToString());
                result = await _okraService.GetTransactions(id, bankId);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetLinkedOrUnLinkedBank(LinkedOrUnlinkedBankRequest request)
        {
            var result = new LinkedOrUnlinkedBankResponse();
            try
            {
                result = await _okraService.GetLinkedOrUnLinkedBank(request.UserGuid);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetTransactionHistory(GetTransactionHistoryRequest request)
        {
            var res = new TransactionHistoryResponse();
            try
            {
                res = await _earningService.GetTransactionHistory(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardGraphtData(GetUserDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _earningService.GetDashboardGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> Invite(Guid guid, string email)
        {
            var result = new GetDashboardGraph();
            try
            {
                var id = Guid.Parse(guid.ToString());
                result = await _earningService.Invite(email, guid);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }

        public async Task<JsonResult> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _earningService.GetDashboardWeeklyGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        public async Task<JsonResult> GetDashboardMonthlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _earningService.GetDashboardMonthlyGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        public async Task<JsonResult> GetDashboardYearlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _earningService.GetDashboardYearlyGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetCommissionList(string guid)
        {
            var res = new GetCommissionResponse();
            try
            {

                res = await _earningService.GetCommisions();
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetAddedBankList(Guid guid)
        {
            var result = new GetAddedBanListResponse();
            try
            {
                result = await _earningService.GetAddedBankList(guid);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }
    }

}