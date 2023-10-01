using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers.Vendor
{
    public class DailyTransactionsController : Controller
    {
        // GET: DailyTransactions
        public ActionResult Index()
        {
            return View();
        }
    }
}