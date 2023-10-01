using PayMasta.Service.Employer.EwaRequests;
using PayMasta.ViewModel.Employer.EWAVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers.Employer
{
    [CustomAuthorize(Roles = "Employer")]
    [SessionExpireFilter]
    public class WithdrawRequestController : MyBaseController
    {
        private IEwaRequestsServices _ewaRequestsServices;
        public WithdrawRequestController(IEwaRequestsServices ewaRequestsServices)
        {
            _ewaRequestsServices= ewaRequestsServices;
        }
        // GET: WithdrawRequest
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesWithdrawsRequestList(GetEmployerListRequest request)
        {
            
            var result = new PayMasta.ViewModel.Employer.EWAVM.EmployeesReponse();
            try
            {
                result = await _ewaRequestsServices.GetEmployeeListbyEmployerGuid(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateAccessAmountRequestById(UpdateEWAStatusRequest request)
        {

            var result = new UpdateEWAStatusResponse();
            try
            {
                result = await _ewaRequestsServices.UpdateAccessAmountRequestById(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReport(GetEmployerListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _ewaRequestsServices.ExportUserListReport(request);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(memoryStream.ToArray())
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue
                      ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment")
                   {
                       FileName = $"{filename}_{DateTime.Now.Ticks.ToString()}.xls"
                   };
            //response.Content.Headers.ContentLength = stream.Length;
            memoryStream.WriteTo(memoryStream);
            memoryStream.Close();
            robj = File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "TeamMembers.xlsx");
            return Json(robj, JsonRequestBehavior.AllowGet);
        }
    }
}