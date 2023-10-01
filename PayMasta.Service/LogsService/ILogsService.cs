using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.LogsService
{
    public interface ILogsService
    {
        Task<int> InsertLogs(string CreditAccount, string DebitAccount, string TransactionAmount, string TransactionReference, string LogJson);
    }
}
