using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EwaRequests
{
    public interface IEwaRequestsServices
    {
        Task<PayMasta.ViewModel.Employer.EWAVM.EmployeesReponse> GetEmployeeListbyEmployerGuid(GetEmployerListRequest request);
        Task<UpdateEWAStatusResponse> UpdateAccessAmountRequestById(UpdateEWAStatusRequest request);
        Task<MemoryStream> ExportUserListReport(GetEmployerListRequest request);
    }
}
