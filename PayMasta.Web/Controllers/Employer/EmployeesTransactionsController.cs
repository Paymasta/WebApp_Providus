using PayMasta.Service.Employer.EmployeeTransaction;
using PayMasta.ViewModel.Employer.EmployeesVM;
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
    public class EmployeesTransactionsController : MyBaseController
    {
        private IEmployeeTransactionService _employeeTransactionService;

        public EmployeesTransactionsController(IEmployeeTransactionService employeeTransactionService)
        {
            _employeeTransactionService = employeeTransactionService;
        }
        // GET: EmployeesTransactions
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListForTransactionRequest request)
        {
            var result = new EmployeesListForTransactionsReponse();

            try
            {
                result = await _employeeTransactionService.GetEmployeesList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        public ActionResult GetEmployeDetailByGuid(string id)
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeeEarningDetailByUserGuid(EmployeesEWAWithdrawlsRequest request)
        {
            var result = new EmployeeEwaDetailReponse();

            try
            {
                result = await _employeeTransactionService.GetEmployeesEwaRequestDetail(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request)
        {
            var result = new EmployeesWithdrawlsResponse();

            try
            {

                result = await _employeeTransactionService.GetEmployeesWithdrwals(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReport(DownloadLogReportRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _employeeTransactionService.ExportUserListReport(request);
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

        [HttpPost]
        public async Task<JsonResult> ExportWithdrawlsCsvReport(EmployeesWithdrawlsRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _employeeTransactionService.ExportWithdrawlsListReport(request);
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

        [HttpPost]
        public async Task<JsonResult> ExportCsvReportForPayCycle(EmployeesWithdrawlsRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);

            var data = await _employeeTransactionService.IsDataExists(request);
            if (data.Count > 0)
            {

                string filename = "PayMastaLog";
                MemoryStream memoryStream = null;
                FileContentResult robj;
                memoryStream = await _employeeTransactionService.ExportWithdrawlsListReportForPayCycle(request);
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
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}