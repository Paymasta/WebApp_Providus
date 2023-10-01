using Dapper;
using PayMasta.DBEntity.OkraCallBack;
using PayMasta.DBEntity.WidgetLinkMaster;
using PayMasta.Utilities;
using PayMasta.ViewModel.OkraVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Okra
{
    public class OkraRepository : IOkraRepository
    {
        private string connectionString;

        public OkraRepository()
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
        public async Task<OkraCallBackResponse> GetIncomeUrlByUserId(long userId, string CallBackType,string bankId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT TOP 1 [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[CustomerId]
                                      ,[CallBackType]
                                      ,[CallBackUrl]
                                      ,[RawContent]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
									  ,[BankCodeOrBankId]
                                  FROM [dbo].[OkraCallBackResponse]
                                  WHERE UserId=@UserId and CallBackType=@CallBackType AND BankCodeOrBankId=@BankCodeOrBankId ORDER by Id DESC";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<OkraCallBackResponse>(query,
                        new
                        {
                            UserId = userId,
                            CallBackType = CallBackType,
                            BankCodeOrBankId=bankId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<OkraCallBackResponse>(query,
                        new
                        {
                            UserId = userId,
                            CallBackType = CallBackType,
                            BankCodeOrBankId = bankId
                        })).FirstOrDefault();
            }
        }

        public async Task<WidgetLinkMaster> GetWidgetLinkByUserId(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[RawContent]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[WidgetLink]
                                  FROM [dbo].[WidgetLinkMaster]
                                  WHERE UserId=@UserId AND ISActive=1 And IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WidgetLinkMaster>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WidgetLinkMaster>(query,
                        new
                        {
                            UserId = userId,
                        })).FirstOrDefault();
            }
        }

        public async Task<WidgetLinkMaster> InsertWidgetLinkDetail(WidgetLinkMaster userEntity, IDbConnection exdbConnection = null)
        {
            string delquery = @"DELETE FROM [dbo].[WidgetLinkMaster] WHERE [UserId]=@userId";


            string query = @"
                              INSERT INTO [dbo].[WidgetLinkMaster]
                                               ([UserId]
                                               ,[RawContent]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[WidgetLink])
                                         VALUES
                                               (@UserId
                                               ,@RawContent
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@WidgetLink);
                              SELECT * from WidgetLinkMaster WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)
                                ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    await dbConnection.ExecuteAsync(delquery, new { userId = userEntity.UserId });
                    return (await dbConnection.QueryAsync<WidgetLinkMaster>(query, userEntity)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WidgetLinkMaster>(query, userEntity)).FirstOrDefault();
            }
        }

        public async Task<List<LinkedOrUnlinkedBank>> GetLinkedOrUnLinkedBank(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"select distinct 
                                            bd.BankName,bd.UserId
                                            ,obr.CallBackType
                                            ,obr.CallBackUrl
                                            ,obr.BankCodeOrBankId
                                            ,CASE WHEN obr.CallBackUrl IS NULL THEN 0 WHEN obr.CallBackUrl IS NOT NULL THEN 1 ELSE 0 END IsLinked
                                            ,bd.ImageUrl
                                            from BankDetail bd
                                            left join OkraCallBackResponse obr on obr.BankCodeOrBankId=bd.BankCode
											and obr.CallBackType='BALANCE' and obr.UserId=bd.UserId 
                                        where bd.UserId=@UserId and bd.IsActive=1 and bd.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<LinkedOrUnlinkedBank>(query,
                        new
                        {
                            UserId = userId,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<LinkedOrUnlinkedBank>(query,
                        new
                        {
                            UserId = userId,
                        })).ToList();
            }
        }
    }
}
