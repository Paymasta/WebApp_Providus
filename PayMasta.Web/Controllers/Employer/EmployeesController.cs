using PayMasta.Service.Employer.Employees;
using PayMasta.ViewModel.Employer.EmployeesVM;
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
    public class EmployeesController : MyBaseController
    {
        private IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EmployeesNotUploaded()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesList(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _employeesService.GetEmployeesByEmployerGuid(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        public ActionResult ViewEmployeeProfile(string id)
        {
            return View();
        }
        public ActionResult ViewEmployeeSalaryStructure()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest blockUnBlockEmployeeRequest)
        {
            var res = new BlockUnBlockEmployeeResponse();
            try
            {
                res = await _employeesService.BlockUnBlockEmployees(blockUnBlockEmployeeRequest);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> ViewEmployeeProfile(ViewEmployeesProfileRequest request)
        {
            var res = new ViewEmployeesProfileResponse();
            try
            {
                res = await _employeesService.ViewEmployeeProfile(request);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateNetAndGrossPay(UpdateEmployeeNetAndGrossPayRequest request)
        {
            var res = new UpdateEmployeeNetAndGrossPayResponse();
            try
            {
                res = await _employeesService.UpdateNetAndGrossPay(request);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> ApproveUserProfile(ApproveUserProfileRequest request)
        {
            var res = new UpdateEmployeeNetAndGrossPayResponse();
            try
            {
                res = await _employeesService.ApproveUserProfile(request);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReport(GetEmployeesListRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _employeesService.ExportUserListReport(request);
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
        public async Task<JsonResult> UploadUserCsv(HttpPostedFileBase file, Guid guid)
        {
            string fileName = "";
            var result = new BulkUploadRecords();
            if (file != null)
            {
                var id = Guid.Parse(guid.ToString());
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                fileName = Guid.NewGuid().ToString("n") + "." + file.FileName.Split('.')[1];

                string path1 = Server.MapPath("~/UploadedFiles/" + file.FileName);
                if (path != null)
                {
                    result = await _employeesService.BulkUploadUsersCSV(guid, path);
                   
                }
                FileInfo file1 = new FileInfo(path1);
                if (file1.Exists)//check file exsit or not  
                {
                    file1.Delete();

                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployeesListError(GetEmployeesListRequest request)
        {
            var result = new EmployeesReponse();

            try
            {
                result = await _employeesService.GetEmployeesByEmployerGuidError(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> BlockUnBlockEmployeesError(BlockUnBlockEmployeeRequest blockUnBlockEmployeeRequest)
        {
            var res = new BlockUnBlockEmployeeResponse();
            try
            {
                res = await _employeesService.BlockUnBlockEmployeesError(blockUnBlockEmployeeRequest);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }
    }
}