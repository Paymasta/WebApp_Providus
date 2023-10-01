using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EmployeeTransaction
{
    public interface IEmployeeTransactionRepository
    {
        Task<List<EmployeesListForTransactions>> GetEmployeesList(long employerId,int pageNumber, int pageSize, int month, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwals(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null);
        Task<EmployeeEwaDetail> GetEmployeesEwaRequestDetail(long userid, IDbConnection exdbConnection = null);
        Task<List<EmployeesListForTransactions>> GetEmployeesListForCsv(long employerId, int month, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForDownload(long userid, int month, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForPayCycle(long userid, IDbConnection exdbConnection = null);
        Task<List<EmployeesListForTransactions>> GetEmployeesListWeb(long employerId, int pageNumber, int pageSize, int month, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawlsForApp>> GetEmployeesWithdrwalsForApp(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null);
        Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForEmployer(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null);
    }
}
