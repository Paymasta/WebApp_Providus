using PayMasta.DBEntity.TransactionLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.LogsRepo
{
    public interface ILogsRepository
    {
        Task<int> InsertTransactionLog(TransactionLog transactionLog, IDbConnection exdbConnection = null);
    }
}
