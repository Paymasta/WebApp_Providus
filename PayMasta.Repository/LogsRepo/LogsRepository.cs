using Dapper;
using PayMasta.DBEntity.TransactionLog;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.LogsRepo
{
    public class LogsRepository : ILogsRepository
    {
        private string connectionString;

        public LogsRepository()
        {
            connectionString = AppSetting.ConnectionStrings;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        public async Task<int> InsertTransactionLog(TransactionLog transactionLog, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[TransactionLog]
                                               ([LogType]
                                               ,[CreditAccount]
                                               ,[DebitAccount]
                                               ,[TransactionAmount]
                                               ,[TransactionReference]
                                               ,[TransactionName]
                                               ,[LogJson]
                                               ,[Detail]
                                               ,[LogDate])
                                         VALUES
                                               (@LogType
                                               ,@CreditAccount
                                               ,@DebitAccount
                                               ,@TransactionAmount
                                               ,@TransactionReference
                                               ,@TransactionName
                                               ,@LogJson
                                               ,@Detail
                                               ,@LogDate)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, transactionLog));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, transactionLog));
            }
        }
    }
}
