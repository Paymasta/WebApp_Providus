using PayMasta.DBEntity.TransactionLog;
using PayMasta.Repository.LogsRepo;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.LogsService
{
    public class LogsService : ILogsService
    {
        private readonly ILogsRepository _logsRepository;
        public LogsService()
        {
            _logsRepository = new LogsRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<int> InsertLogs(string CreditAccount, string DebitAccount, string TransactionAmount, string TransactionReference, string LogJson)
        {
            int result = 0;
            try
            {
                var req = new TransactionLog
                {
                    CreditAccount = CreditAccount,
                    DebitAccount = DebitAccount,
                    TransactionAmount = TransactionAmount,
                    TransactionReference = TransactionReference,
                    LogJson = LogJson,
                    IsActive = true,
                    IsDeleted = false,
                    LogDate = DateTime.UtcNow,
                    Detail = LogJson,
                    TransactionName = "Providus",
                    LogType = "Debit from employer"
                };
                result = await _logsRepository.InsertTransactionLog(req);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
