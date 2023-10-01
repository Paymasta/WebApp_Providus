using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.PayToPayMasta
{
    public interface IPayToPayMastaRepository
    {
        Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText, long EmployerId, IDbConnection exdbConnection = null);
        Task<PayableAmount> GetPayAbleAmount(long EmployerId, IDbConnection exdbConnection = null);
        Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestListForCsv(int status, DateTime? fromDate, DateTime? toDate, string searchText, long EmployerId, IDbConnection exdbConnection = null);
        Task<int> UpdatePayToPayMastaFlag(long employerId, IDbConnection exdbConnection = null);
    }
}
