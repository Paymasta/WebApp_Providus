using PayMasta.Service.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.API.Controllers.Account
{
    public class VerifyAccountController : Controller
    {
        private IAccountService _accountService;

        /// <summary>
        /// ctor
        /// </summary>
        public VerifyAccountController()
        {
            _accountService = new AccountService();
        }
        // GET: VerifyAccount
        public async Task<ActionResult> Index(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var guid = Guid.Parse(id);
                int res = await _accountService.VerifyEmail(guid);
                ViewBag.Status = res;
                ViewBag.Success = "Email verified successfully.";
                ViewBag.Failure = "Email already verified.";
               // ViewData["Test"] = res;
            }
            return View();
        }
    }
}