using NPOI.HSSF.UserModel;
using PayMasta.Repository.Account;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Repository.Employer.EmployeesBankAccount;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.EmployeeBankDetailVM;
using PayMasta.ViewModel.Employer.EmployeesVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EmployeesBankAccount
{
    public class EmployeesBankAccountService : IEmployeesBankAccountService
    {
        private HSSFWorkbook _hssfWorkbook;
        private readonly IEmployeesBankAccountRepository _employeesBankAccountRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAccountRepository _accountRepository;
        public EmployeesBankAccountService()
        {
            _employeesBankAccountRepository = new EmployeesBankAccountRepository();
            _employeesRepository = new EmployeesRepository();
            _accountRepository = new AccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<EmployeesBankReponse> GetEmployeesBankListByEmployerId(GetEmployeesBankListRequest request)
        {
            var res = new EmployeesBankReponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employerGuid = Guid.Parse(request.EmployerGuid.ToString());
                var userGuid = Guid.Parse(request.UserGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employerGuid);
                var userData = await _accountRepository.GetUserByGuid(userGuid);
                if (result != null)
                {
                    var employeesList = await _employeesBankAccountRepository.GetEmployeesBankListByEmployerId(result.Id, userData.Id, request.pageNumber, request.PageSize, request.SearchTest, request.Status, request.FromDate, request.ToDate);
                    if (employeesList.Count > 0)
                    {
                        res.employeesListViewModel = employeesList;
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.DATA_RECEIVED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<SetSalaryAccountReponse> SetSalaryAccount(SetSalaryAccountRequest request)
        {
            var res = new SetSalaryAccountReponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employerGuid = Guid.Parse(request.EmployerGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employerGuid);
                if (result != null)
                {
                    var isSalaryAccount = await _employeesBankAccountRepository.SetSalaryAccount(request.UserId, request.BankDetailId);
                    if (isSalaryAccount > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.DATA_SAVED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.DATA_NOT_SAVED;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
