using PayMasta.ViewModel.Employer.EmployeeBankDetailVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EmployeesBankAccount
{
    public interface IEmployeesBankAccountRepository
    {
        Task<int> SetSalaryAccount(long userId, long BankdetailId, IDbConnection exdbConnection = null);
        Task<List<BankDetailResponse>> GetEmployeesBankListByEmployerId(long employerId, long userId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
    }
}
