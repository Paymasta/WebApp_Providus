using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.CommonEmployerService
{
    public interface ICommonEmployerService
    {
        Task<GetEmployerResponse> GetEmployerList();
        Task<GetNonRegisterEmployerResponse> GetNonRegisteredEmployerList(string searchText = "");
    }
}
