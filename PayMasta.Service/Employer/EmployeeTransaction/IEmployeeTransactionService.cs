using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EmployeeTransaction
{
    public interface IEmployeeTransactionService
    {
        Task<EmployeesListForTransactionsReponse> GetEmployeesList(GetEmployeesListForTransactionRequest request);
        Task<EmployeesWithdrawlsResponse> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request);
        Task<EmployeeEwaDetailReponse> GetEmployeesEwaRequestDetail(EmployeesEWAWithdrawlsRequest request);
        Task<MemoryStream> ExportUserListReport(DownloadLogReportRequest request);
        Task<MemoryStream> ExportWithdrawlsListReport(EmployeesWithdrawlsRequest request);
        Task<MemoryStream> ExportWithdrawlsListReportForPayCycle(EmployeesWithdrawlsRequest request);
        Task<List<EmployeesWithdrawls>> IsDataExists(EmployeesWithdrawlsRequest request);
        Task<EmployeesListForTransactionsReponse> GetEmployeesListWeb(GetEmployeesListForTransactionRequest request);
        Task<EmployeesWithdrawlsResponseForApp> GetEmployeesWithdrwalsForApp(EmployeesWithdrawlsRequest request);
    }
}
