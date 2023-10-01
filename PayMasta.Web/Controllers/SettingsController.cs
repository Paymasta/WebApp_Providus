using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    [SessionExpireFilter]
    public class SettingsController : MyBaseController
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }
    }
}