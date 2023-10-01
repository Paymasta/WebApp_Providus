using PayMasta.DBEntity.AccessAmountRequest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EwaRequests
{
    public interface IEwaRequestsRepository
    {
        Task<int> UPdateAccessAmountRequestById(AccessAmountRequest accessAmountRequest, IDbConnection exdbConnection = null);
        Task<AccessAmountRequest> GetAccessAmountRequestById(long accessAmountId, IDbConnection exdbConnection = null);
        Task<List<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>> DownloadCsvEmployeesListByEmployerId(long employerId, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
    }
}
