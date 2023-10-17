using PayMasta.DBEntity.CustomerQrCodeDetail;
using PayMasta.DBEntity.ExpressWallet;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ExpressWalletRepository
{
    public interface IExpressWalletRepository
    {
        Task<int> InsertVirtualAccountDetail(ExpressVirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null);
        Task<ExpressVirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<ExpressVirtualAccountDetail> GetVirtualAccountDetailByWalletAccount(string walletAccount, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServices(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null);
        Task<int> InsertTransactions(WalletTransaction walletTransactionEntity, IDbConnection exdbConnection = null);
        Task<int> InsertQRCodeDetail(CustomerQrCodeDetail customerQrCodeDetail, IDbConnection exdbConnection = null);
        Task<CustomerQrCodeDetail> GetQRCodeDetailByUserId(long userId, IDbConnection exdbConnection = null);
    }
}
