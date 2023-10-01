using PayMasta.Service.Employer.Dashboard;
using PayMasta.Service.Employer.EmployeeTransaction;
using PayMasta.ViewModel.Employer.Dashboard;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers.Employer
{
    [CustomAuthorize(Roles = "Employer")]
    [SessionExpireFilter]
    public class EmployerDashbordController : MyBaseController
    {
        private IDashboardService _dashboardService;
        private IEmployeeTransactionService _employeeTransactionService;
        public EmployerDashbordController(IDashboardService dashboardService, IEmployeeTransactionService employeeTransactionService)
        {
            _dashboardService = dashboardService;
            _employeeTransactionService = employeeTransactionService;
        }
        // GET: EmployerDashbord
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardData(DashboardRequest request)
        {
            var res = new DashboardTabResponse();
            try
            {
                res = await _dashboardService.GetDashboardData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardEWARequestData(PendingEWRRequest request)
        {
            var res = new PendingEWRResponse();
            try
            {
                res = await _dashboardService.GetDashboardEWARequestData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }

        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardGraphtData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _dashboardService.GetDashboardGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        public async Task<JsonResult> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();
            try
            {
                res = await _dashboardService.GetDashboardWeeklyGraphData(request);
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
                res = await _dashboardService.GetDashboardMonthlyGraphData(request);
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
                res = await _dashboardService.GetDashboardYearlyGraphData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListForTransactionRequest request)
        {
            var result = new EmployeesListForTransactionsReponse();

            try
            {
                result = await _employeeTransactionService.GetEmployeesListWeb(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        //  [SessionExpireFilter]
        public async Task<JsonResult> GetDashboardPayCycleData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardPayCycle();
            try
            {
                res = await _dashboardService.GetDashboardPayCycleData(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
    }
}