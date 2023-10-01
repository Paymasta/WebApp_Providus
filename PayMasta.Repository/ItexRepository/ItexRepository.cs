using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.DBEntity.WalletService;
using PayMasta.Utilities;
using PayMasta.ViewModel.ItexVM;
namespace PayMasta.Repository.ItexRepository
{
    public class ItexRepository : IItexRepository
    {
        private string connectionString;

        public ItexRepository()
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
        public async Task<List<WalletServiceResponse>> GetWalletServicesListBySubcategoryId(int SubCategoryId, IDbConnection exdbConnection = null)
        {

            string query = @"usp_GetOperatorList";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@SubCategoryId", SubCategoryId);

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletServiceResponse>(query, parameter, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletServiceResponse>(query, parameter, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[VirtualAccountId]
                                      ,[ProfileID]
                                      ,[Pin]
                                      ,[deviceNotificationToken]
                                      ,[PhoneNumber]
                                      ,[Gender]
                                      ,[DateOfBirth]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[Address]
                                      ,[Bvn]
                                      ,[AccountName]
                                      ,[AccountNumber]
                                      ,[CurrentBalance]
                                      ,[JsonData]
                                      ,[UserId]
                                      ,[AuthToken]
                                      ,[AuthJson]
                                  FROM [dbo].[VirtualAccountDetail]
                                  Where UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<VirtualAccountDetail>(query, new
                    {
                        UserId = userId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<VirtualAccountDetail>(query, new
                {
                    UserId = userId
                })).FirstOrDefault();
            }
        }
        public async Task<VirtualAccountDetail> GetVirtualAccountDetailByVirtualAccountNumber(string accountNumber, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[VirtualAccountId]
                                      ,[ProfileID]
                                      ,[Pin]
                                      ,[deviceNotificationToken]
                                      ,[PhoneNumber]
                                      ,[Gender]
                                      ,[DateOfBirth]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[Address]
                                      ,[Bvn]
                                      ,[AccountName]
                                      ,[AccountNumber]
                                      ,[CurrentBalance]
                                      ,[JsonData]
                                      ,[UserId]
                                      ,[AuthToken]
                                      ,[AuthJson]
                                  FROM [dbo].[VirtualAccountDetail]
                                  Where AccountNumber=@AccountNumber";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<VirtualAccountDetail>(query, new
                    {
                        AccountNumber = accountNumber
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<VirtualAccountDetail>(query, new
                {
                    AccountNumber = accountNumber
                })).FirstOrDefault();
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

        public async Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity(int SubCategoryId, string serviceName,string accountType, IDbConnection exdbConnection = null)
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
                                          WHERE SubCategoryId=@SubCategoryId AND ServiceName=@ServiceName OR AccountType=@AccountType";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query, new
                    {
                        SubCategoryId = SubCategoryId,
                        ServiceName = serviceName,
                        AccountType = accountType,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query, new
                {
                    SubCategoryId = SubCategoryId,
                    ServiceName = serviceName,
                    AccountType = accountType,
                })).FirstOrDefault();
            }
        }
        public async Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null)
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
        public async Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForData(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null)
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

        public async Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity1(int SubCategoryId, string serviceName, string accountType, IDbConnection exdbConnection = null)
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
                                          WHERE SubCategoryId=@SubCategoryId AND ServiceName=@ServiceName AND AccountType=@AccountType";

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query, new
                    {
                        SubCategoryId = SubCategoryId,
                        ServiceName = serviceName,
                        AccountType = accountType,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query, new
                {
                    SubCategoryId = SubCategoryId,
                    ServiceName = serviceName,
                    AccountType = accountType,
                })).FirstOrDefault();
            }
        }
    }
}
