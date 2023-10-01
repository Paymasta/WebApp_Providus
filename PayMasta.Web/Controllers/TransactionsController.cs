using PayMasta.Service.Common;
using PayMasta.Service.Earning;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EarningVM;
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
    public class TransactionsController : MyBaseController
    {
        private IEarningService _earningService;
        private ICommonService _commonService;
        public TransactionsController(IEarningService earningService, ICommonService commonService)
        {
            _earningService = earningService;
            _commonService = commonService;
        }

        // GET: Transactions
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> TransactionHistory(GetTransactionHistoryRequest request)
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
        public async Task<JsonResult> GetCategories(Guid userGuid)
        {
            var res = new GetCategoryResponse();
            try
            {
                res = await _commonService.GetCategories();
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
    }
}