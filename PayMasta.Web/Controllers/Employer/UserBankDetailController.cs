using PayMasta.Service.Employer.Employees;
using PayMasta.Service.Employer.EmployeesBankAccount;
using PayMasta.ViewModel.Employer.EmployeeBankDetailVM;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers.Employer
{
    [CustomAuthorize(Roles = "Employer")]
    [SessionExpireFilter]
    public class UserBankDetailController : Controller
    {
        private IEmployeesService _employeesService;
        private IEmployeesBankAccountService _employeesBankAccountService;
        // GET: UserBankDetail
        public UserBankDetailController(IEmployeesService employeesService, IEmployeesBankAccountService employeesBankAccountService)
        {
            _employeesService = employeesService;
            _employeesBankAccountService = employeesBankAccountService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserBankDetails(string id)
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
        [HttpPost]
        public async Task<JsonResult> GetEmployeesBankListByEmployerId(GetEmployeesBankListRequest request)
        {
            var result = new EmployeesBankReponse();

            try
            {
                result = await _employeesBankAccountService.GetEmployeesBankListByEmployerId(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> SetSalaryAccount(SetSalaryAccountRequest request)
        {
            var result = new SetSalaryAccountReponse();

            try
            {
                result = await _employeesBankAccountService.SetSalaryAccount(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}