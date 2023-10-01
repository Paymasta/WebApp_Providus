using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.OkraCallBack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.CallBack
{
    public interface ICallBackRepository
    {
        Task<int> InsertOkraCallBackDetail(OkraCallBackResponse okraCallBackResponse, IDbConnection exdbConnection = null);
        Task<BankDetail> GetBankDetailByCustomerId(string customerid, IDbConnection exdbConnection = null);
        Task<int> UpdateOkraLinkStatus(UserMaster userEntity, IDbConnection exdbConnection = null);
    }
}
