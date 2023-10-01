using PayMasta.Service.Common;
using PayMasta.Service.Support;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.NotificationsVM;
using PayMasta.ViewModel.SupportVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    [SessionExpireFilter]
    public class SupportController : MyBaseController
    {
        private ICommonService _commonService;
        private ISupportService _supportService;
        public SupportController(ICommonService commonService, ISupportService supportService)
        {
            _commonService = commonService;
            _supportService = supportService;
        }
        // GET: Support
        public async Task<ActionResult> Index()
        {
            return View();
        }
        public async Task<ActionResult> ViewTickets()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GeticketNumber(int Size)
        {
            var res = new InvoiceNumberResponse();
            res = await _commonService.GetInvoiceNumber();
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> InsertSupportTicket(SupportRequest request)
        {
            var res = new SupportResponse();
            try
            {
                res = await _supportService.InsertSupportTicket(request);
            }
            catch (Exception ex)
            {

            }
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetSupportDetailList(GetSupportMasterRequest req)
        {
            //var req = new GetSupportMasterRequest
            //{
            //    UserGuid = Guid.Parse(UserGuid.ToString())
            //};
            var res = new SupportMasterResponse();
            res = await _supportService.GetSupportDetailList(req);
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetNotificationByUserGuid(NotificationsRequest request)
        {
            var res = new GetNotificationsResponse();
            try
            {
                //var req = new NotificationsRequest
                //{
                //    UserGuid = Guid.Parse(UserGuid.ToString())
                //};

                res = await _commonService.GetNotificationByUserGuid(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateNotificationByUserGuid(NotificationsRequest request)
        {
            var res = new UpdateNotificationsResponse();
            try
            {
                //var req = new NotificationsRequest
                //{
                //    UserGuid = Guid.Parse(UserGuid.ToString())
                //};

                res = await _commonService.UpdateNotificationByUserGuid(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}