using PayMasta.ViewModel.Employer.EmployeesVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.Employees
{
    public interface IEmployeesService
    {
        Task<EmployeesReponse> GetEmployeesByEmployerGuid(GetEmployeesListRequest request);
        Task<EmployeesReponse> GetEmployeesByEmployerGuidError(GetEmployeesListRequest request);
        Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest request);
        Task<ViewEmployeesProfileResponse> ViewEmployeeProfile(ViewEmployeesProfileRequest request);
        Task<UpdateEmployeeNetAndGrossPayResponse> UpdateNetAndGrossPay(UpdateEmployeeNetAndGrossPayRequest request);
        Task<MemoryStream> ExportUserListReport(GetEmployeesListRequest request);
        Task<BulkUploadRecords> BulkUploadUsersCSV(Guid employerGuid, string filePath);
        Task<BlockUnBlockEmployeeResponse> DeleteEmployees(DeleteEmployeeRequest request);
        Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployeesError(BlockUnBlockEmployeeRequest request);
        Task<UpdateEmployeeNetAndGrossPayResponse> ApproveUserProfile(ApproveUserProfileRequest request);
    }
}
