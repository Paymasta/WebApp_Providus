using PayMasta.ViewModel.Employer.EmployeeBankDetailVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EmployeesBankAccount
{
    public interface IEmployeesBankAccountService
    {
        Task<EmployeesBankReponse> GetEmployeesBankListByEmployerId(GetEmployeesBankListRequest request);
        Task<SetSalaryAccountReponse> SetSalaryAccount(SetSalaryAccountRequest request);
    }
}
