using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    [CustomAuthorize(Roles = "Employee")]
    [SessionExpireFilter]
    public class DashBoardController : MyBaseController
    {
        // GET: DashBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}