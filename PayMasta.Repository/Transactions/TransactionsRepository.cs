using Dapper;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Transactions
{
    public class TransactionsRepository : ITransactionsRepository
    { //not in use for now

        private string connectionString;

        public TransactionsRepository()
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
        public async Task<int> InsertTransactions(WalletTransaction walletTransactionEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[WalletTransaction]
                                                               ([TotalAmount]
                                                               ,[CommisionId]
                                                               ,[CommisionAmount]
                                                               ,[WalletAmount]
                                                               ,[ServiceTaxRate]
                                                               ,[ServiceTax]
                                                               ,[ServiceCategoryId]
                                                               ,[SenderId]
                                                               ,[ReceiverId]
                                                               ,[AccountNo]
                                                               ,[TransactionId]
                                                               ,[IsAdminTransaction]
                                                               ,[IsActive]
                                                               ,[IsDeleted]
                                                               ,[CreatedAt]
                                                               ,[UpdatedAt]
                                                               ,[Comments]
                                                               ,[InvoiceNo]
                                                               ,[TransactionStatus]
                                                               ,[TransactionType]
                                                               ,[TransactionTypeInfo]
                                                               ,[IsBankTransaction]
                                                               ,[BankBranchCode]
                                                               ,[BankTransactionId]
                                                               ,[VoucherCode]
                                                               ,[FlatCharges]
                                                               ,[BenchmarkCharges]
                                                               ,[CommisionPercent]
                                                               ,[DisplayContent]
                                                               ,[IsUpcomingBillShow]
                                                               ,[SubCategoryId],[IsAmountPaid])
                                                         VALUES
                                                               (@TotalAmount
                                                               ,@CommisionId
                                                               ,@CommisionAmount
                                                               ,@WalletAmount
                                                               ,@ServiceTaxRate
                                                               ,@ServiceTax
                                                               ,@ServiceCategoryId
                                                               ,@SenderId
                                                               ,@ReceiverId
                                                               ,@AccountNo
                                                               ,@TransactionId
                                                               ,@IsAdminTransaction
                                                               ,@IsActive
                                                               ,@IsDeleted
                                                               ,@CreatedAt
                                                               ,@UpdatedAt
                                                               ,@Comments
                                                               ,@InvoiceNo
                                                               ,@TransactionStatus
                                                               ,@TransactionType
                                                               ,@TransactionTypeInfo
                                                               ,@IsBankTransaction
                                                               ,@BankBranchCode
                                                               ,@BankTransactionId
                                                               ,@VoucherCode
                                                               ,@FlatCharges
                                                               ,@BenchmarkCharges
                                                               ,@CommisionPercent
                                                               ,@DisplayContent
                                                               ,@IsUpcomingBillShow
                                                               ,@SubCategoryId,@IsAmountPaid)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, walletTransactionEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, walletTransactionEntity));
            }
        }

        public async Task<WalletService> GetWalletServicesListBySubcategoryIdAndService(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                              ,[Guid]
                                              ,[ServiceName]
                                              ,[SubCategoryId]
                                              ,[IsActive]
                                              ,[IsDeleted]
                                              ,[CreatedAt]
                                              ,[UpdatedAt]
                                              ,[ImageUrl]
                                              ,[BankCode]
                                              ,[HttpVerbs]
                                              ,[RawData]
                                              ,[CountryCode]
                                              ,[BillerName]
                                              ,[OperatorId]
                                          FROM [dbo].[WalletService]
                                          WHERE SubCategoryId=@SubCategoryId AND ServiceName=@ServiceName";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query, new
                    {
                        SubCategoryId = SubCategoryId,
                        ServiceName = serviceName
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query, new
                {
                    SubCategoryId = SubCategoryId,
                    ServiceName = serviceName
                })).FirstOrDefault();
            }
        }
    }
}
