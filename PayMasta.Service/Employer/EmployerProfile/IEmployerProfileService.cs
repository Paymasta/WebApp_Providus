using PayMasta.ViewModel;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EmployerProfile
{
    public interface IEmployerProfileService
    {
        Task<GetEmployerProfile> GetEmployerProfileForUpdate(string guid);
        Task<EmployerUpdateProfileResponse> UpdateEmployerProfile(UpdateEmployerRequest request);
    }
}
