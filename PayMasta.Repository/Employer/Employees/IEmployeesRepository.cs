using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.ViewModel.Employer.EmployeesVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.Employees
{
    public interface IEmployeesRepository
    {
        Task<GetEmployerDetailResponse> GetEmployerDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdError(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<int> UpdateUserNetAndGrossPay(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdCSV(long employerId, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null);
        Task<int> BulkUploadUsersList(DataTable dt, string employerName, long empId);
        Task<List<BankDetail>> GetBankDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<int> DeleteBankByBankDetailId(BankDetail bankDetail, IDbConnection exdbConnection = null);
        Task<int> BulkUploadUsersListError(DataTable dt, string employerName, long empId);
        Task<int> ApproveUserProfileByEmployer(UserMaster userEntity, IDbConnection exdbConnection = null);
    }
}
