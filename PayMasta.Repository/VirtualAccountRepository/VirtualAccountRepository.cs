using Dapper;
using PayMasta.DBEntity.ProvidusVirtualAccountDetail;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.VirtualAccountRepository
{
    public class VirtualAccountRepository : IVirtualAccountRepository
    {
        private string connectionString;

        public VirtualAccountRepository()
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

        public async Task<int> InsertVirtualAccountDetail(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[VirtualAccountDetail]
                                               ([VirtualAccountId]
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
                                               ,[AuthJson])
                                         VALUES
                                               (@VirtualAccountId
                                               ,@ProfileID
                                               ,@Pin
                                               ,@deviceNotificationToken
                                               ,@PhoneNumber
                                               ,@Gender
                                               ,@DateOfBirth
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt
                                               ,@Address
                                               ,@Bvn
                                               ,@AccountName
                                               ,@AccountNumber
                                               ,@CurrentBalance
                                               ,@JsonData
                                               ,@UserId
                                               ,@AuthToken
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


        public async Task<int> InsertProvidusVirtualAccountDetail(ProvidusVirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[ProvidusVirtualAccountDetail]
                                                   ([UserId]
                                                   ,[AccountNumber]
                                                   ,[AccountName]
                                                   ,[IsRequestSuccessful]
                                                   ,[ResponseMessage]
                                                   ,[ResponseCode]
                                                   ,[Bvn]
                                                   ,[InitiationTranRef]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt])
                                             VALUES
                                                   (@UserId
                                                   ,@AccountNumber
                                                   ,@AccountName
                                                   ,@IsRequestSuccessful
                                                   ,@ResponseMessage
                                                   ,@ResponseCode
                                                   ,@Bvn
                                                   ,@InitiationTranRef
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt);";
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

        public async Task<ProvidusVirtualAccountDetail> GetProvidusVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                                  ,[Guid]
                                                  ,[UserId]
                                                  ,[AccountNumber]
                                                  ,[AccountName]
                                                  ,[IsRequestSuccessful]
                                                  ,[ResponseMessage]
                                                  ,[ResponseCode]
                                                  ,[Bvn]
                                                  ,[InitiationTranRef]
                                                  ,[IsActive]
                                                  ,[IsDeleted]
                                                  ,[CreatedAt]
                                                  ,[UpdatedAt]
                                              FROM [dbo].[ProvidusVirtualAccountDetail]
                                              WHERE UserId=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ProvidusVirtualAccountDetail>(query, new
                    {
                        UserId = userId
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ProvidusVirtualAccountDetail>(query, new
                {
                    UserId = userId
                })).FirstOrDefault();
            }
        }


        public async Task<int> UpdateVirtualAccountDetailByUserId(VirtualAccountDetail virtualAccountDetail, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE [dbo].[VirtualAccountDetail]
                                           SET
                                              [AuthToken] = @AuthToken
                                              ,[AuthJson] = @AuthJson
                                              ,[Pin] = @Pin
                                              ,[ProfileID] = @ProfileID
                                              ,[Address] = @Address
                                              ,[Bvn] = @Bvn
                                         WHERE Id=@Id";
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
    }
}
