using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.EmployerDetail;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EmployerProfile
{
    public interface IEmployerProfileRepository
    {
        Task<GetEmployerProfileDetailResponse> GetEmployerProfileForUpdate(Guid guid, IDbConnection exdbConnection = null);
        Task<EmployerDetail> GetEmployerDetail(long employerId, IDbConnection exdbConnection = null);
        Task<UserMaster> GetEmployerUserMasterDetail(Guid employerId, IDbConnection exdbConnection = null);
        Task<int> UpdateEmployerUser(UserMaster userEntity, IDbConnection exdbConnection = null);
        Task<int> UpdateDetail(EmployerDetail employerDetail, IDbConnection exdbConnection = null);
    }
}
