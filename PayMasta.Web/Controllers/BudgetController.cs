using PayMasta.Service.Budget;
using PayMasta.Service.ItexService;
using PayMasta.ViewModel.BudgetVM;
using PayMasta.ViewModel.ItexVM;
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
    public class BudgetController : MyBaseController
    {
        private IBudgetService _budgetService;
        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        // GET: Budget
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetServiceList(Guid request)
        {
            var res = new ServiceCategoryResponse();

            try
            {
                res = await _budgetService.GetServiceList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> CreateBudget(CreateBudgetRequest request)
        {
            var res = new SaveBudgetResponse();

            try
            {
                res = await _budgetService.SaveBudget(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetExpenses(Guid request)
        {
            var res = new ExpenseTrackResponse();

            try
            {
                res = await _budgetService.GetExpenses(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetBudget(Guid request)
        {
            var res = new ExpenseTrackResponse();

            try
            {
                res = await _budgetService.GetBudget(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetDailyExpense(Guid request)
        {
            var res = new DailyExpenseTrackResponse();

            try
            {
                res = await _budgetService.GetDailyExpense(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}