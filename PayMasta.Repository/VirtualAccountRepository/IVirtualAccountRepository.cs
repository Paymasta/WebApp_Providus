using PayMasta.DBEntity.ProvidusVirtualAccountDetail;
using PayMasta.DBEntity.VirtualAccountDetail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.VirtualAccountRepository
{
    public interface IVirtualAccountRepository
    {
        Task<int> InsertVirtualAccountDetail(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
        Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<int> InsertProvidusVirtualAccountDetail(ProvidusVirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
        Task<ProvidusVirtualAccountDetail> GetProvidusVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<int> UpdateVirtualAccountDetailByUserId(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
    }
}
