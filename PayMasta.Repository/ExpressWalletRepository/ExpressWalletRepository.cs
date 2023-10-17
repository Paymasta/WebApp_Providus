using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.DBEntity.ExpressWallet;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.DBEntity.CustomerQrCodeDetail;

namespace PayMasta.Repository.ExpressWalletRepository
{
    public class ExpressWalletRepository : IExpressWalletRepository
    {
        private string connectionString;

        public ExpressWalletRepository()
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
        public async Task<int> InsertVirtualAccountDetail(ExpressVirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[ExpressVirtualAccountDetail]
                                                   ([VirtualAccountId]
                                                   ,[CustomerId]
                                                   ,[Email]
                                                   ,[BankName]
                                                   ,[BankCode]
                                                   ,[WalletId]
                                                   ,[PhoneNumber]
                                                   ,[WalletType]
                                                   ,[AccountName]
                                                   ,[AccountNumber]
                                                   ,[CurrentBalance]
                                                   ,[AccountReference]
                                                   ,[MerchantId]
                                                   ,[NameMatch]
                                                   ,[DateOfBirth]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[Address]
                                                   ,[Bvn]
                                                   ,[JsonData]
                                                   ,[UserId]
                                                   ,[AuthJson])
                                             VALUES
                                                   (@VirtualAccountId
                                                   ,@CustomerId
                                                   ,@Email
                                                   ,@BankName
                                                   ,@BankCode
                                                   ,@WalletId
                                                   ,@PhoneNumber
                                                   ,@WalletType
                                                   ,@AccountName
                                                   ,@AccountNumber
                                                   ,@CurrentBalance
                                                   ,@AccountReference
                                                   ,@MerchantId
                                                   ,@NameMatch
                                                   ,@DateOfBirth
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@Address
                                                   ,@Bvn
                                                   ,@JsonData
                                                   ,@UserId
                                                   ,@AuthJson)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, virtualAccountDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, virtualAccountDetail));
            }
        }

        public async Task<ExpressVirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT *
                                  FROM [dbo].[ExpressVirtualAccountDetail]
                                  Where UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ExpressVirtualAccountDetail>(query, new
                    {
                        UserId = userId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ExpressVirtualAccountDetail>(query, new
                {
                    UserId = userId
                })).FirstOrDefault();
            }
        }
        public async Task<ExpressVirtualAccountDetail> GetVirtualAccountDetailByWalletAccount(string walletAccount, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT *
                                  FROM [dbo].[ExpressVirtualAccountDetail]
                                  Where AccountNumber=@AccountNumber";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ExpressVirtualAccountDetail>(query, new
                    {
                        AccountNumber = walletAccount
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ExpressVirtualAccountDetail>(query, new
                {
                    AccountNumber = walletAccount
                })).FirstOrDefault();
            }
        }
        public async Task<WalletService> GetWalletServices(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null)
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
                                              ,[OperatorId],[AccountType]
                                          FROM [dbo].[WalletService]
                                          WHERE SubCategoryId=@SubCategoryId AND ServiceName=@ServiceName";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query, new
                    {
                        SubCategoryId = SubCategoryId,
                        ServiceName = serviceName,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query, new
                {
                    SubCategoryId = SubCategoryId,
                    ServiceName = serviceName,
                })).FirstOrDefault();
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

        public async Task<int> InsertQRCodeDetail(CustomerQrCodeDetail customerQrCodeDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[CustomerQrCodeDetail]
                                               ([UserId]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[UpdatedAt]
                                               ,[ImageUrl])
                                         VALUES
                                               (@UserId
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt
                                               ,@ImageUrl)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, customerQrCodeDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, customerQrCodeDetail));
            }
        }

        public async Task<CustomerQrCodeDetail> GetQRCodeDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[ImageUrl]
                                  FROM [dbo].[CustomerQrCodeDetail]
                                  Where UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CustomerQrCodeDetail>(query, new
                    {
                        UserId = userId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CustomerQrCodeDetail>(query, new
                {
                    UserId = userId
                })).FirstOrDefault();
            }
        }
    }
}
