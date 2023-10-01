using PayMasta.DBEntity.NonRegisterEmployerDetail;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.CommonEmployerRepository
{
    public interface ICommonEmployerRepository
    {
        Task<List<EmployerResponse>> GetEmployerList(bool IsActive, IDbConnection exdbConnection = null);
        Task<List<NonRegisterEmployerResponse>> GetNonRegisteredEmployerList(string searchText, IDbConnection exdbConnection = null);
        Task<NonRegisterEmployerDetail> GetNonRegisteredEmployerByGuid(long? Id, Guid? userGuid, IDbConnection exdbConnection = null);
    }
}
