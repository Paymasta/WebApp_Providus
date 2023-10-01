using PayMasta.Service.Common;
using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.NotificationsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{

    public class CMSController : Controller
    {

        private ICommonService _commonService;
        public CMSController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        // GET: CMS
        public ActionResult PayMastaFaq()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult PayMastaPrivacyPolicy()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult PayMastaTermAndCondition()
        {
            return View();
        }
        public ActionResult TermsConditions()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetPayMastaFaq(int id)
        {
            var res = new List<FAQResponse>();

            try
            {
                res = await _commonService.GetFaq();
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetTermAndCondition(int id)
        {
            var res = new CmsResponse();

            try
            {
                res = await _commonService.GetTermAndCondition();
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetPrivacyPolicy(int id)
        {
            var res = new CmsResponse();

            try
            {
                res = await _commonService.GetPrivacyPolicy();
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Employees()
        {
            return View();
        }
        public ActionResult Employer()
        {
            return View();
        }
        public ActionResult RequestDemo()
        {
            return View();
        }
        public ActionResult SubmitTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RequestDemo(RequestDemoRequest request)
        {

            var res = new UpdateNotificationsResponse();
            try
            {
                res = await _commonService.RequestDemo(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
    }
}