using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Transactions
{
    public interface ITransactionsRepository
    {
        Task<int> InsertTransactions(WalletTransaction walletTransactionEntity, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndService(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null);
    }
}
