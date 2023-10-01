using Dapper;
using PayMasta.DBEntity.Support;
using PayMasta.Utilities;
using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.SupportRepository
{
    public class SupportRepository : ISupportRepository
    {
        private string connectionString;

        public SupportRepository()
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

        public async Task<int> InsertSupportDetail(SupportMaster supporEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[SupportMaster]
                                               ([UserId]
                                               ,[TicketNumber]
                                               ,[Title]
                                               ,[DescriptionText]
                                               ,[Status]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[CreatedBy])
                                         VALUES
                                               (@UserId
                                               ,@TicketNumber
                                               ,@Title
                                               ,@DescriptionText
                                               ,@Status
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@CreatedBy)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, supporEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, supporEntity));
            }
        }



        public async Task<List<SupportMasterTicketResponse>> GetSupportDetailList(long userId,int pagenumber,int pagesize, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT  COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
                                      ,[Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[TicketNumber]
                                      ,[Title]
                                      ,[DescriptionText]
                                      ,CASE WHEN [Status]=0 THEN 'Pending' WHEN [Status]=2 THEN 'Inprogress' WHEN [Status]=3 THEN 'Approved'  WHEN [Status]=4 THEN 'Hold' WHEN [Status]=5 THEN 'Rejected' ELSE 'Failed' END Status
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,CONVERT(varchar, [CreatedAt])[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[Status] StatusId
                                  FROM [dbo].[SupportMaster]
                                  where UserId=@UserId ORDER BY Id DESC
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportMasterTicketResponse>(query,
                        new
                        {
                            UserId = userId,
                            pageSize=pagesize,
                            pageNumber=pagenumber
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportMasterTicketResponse>(query, new
                {
                    UserId = userId,
                    pageSize = pagesize,
                    pageNumber = pagenumber
                })).ToList();
            }
        }

        public async Task<List<SupportMasterTicketResponse>> GetSupportDetailList(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT  COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
                                      ,[Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[TicketNumber]
                                      ,[Title]
                                      ,[DescriptionText]
                                      ,CASE WHEN [Status]=0 THEN 'Pending' WHEN [Status]=2 THEN 'Inprogress' WHEN [Status]=3 THEN 'Approved'  WHEN [Status]=4 THEN 'Hold' WHEN [Status]=5 THEN 'Rejected' ELSE 'Failed' END Status
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,CONVERT(varchar, [CreatedAt])[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[Status] StatusId
                                  FROM [dbo].[SupportMaster]
                                  where UserId=@UserId ORDER BY Id DESC";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<SupportMasterTicketResponse>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<SupportMasterTicketResponse>(query, new
                {
                    UserId = userId
                })).ToList();
            }
        }
    }
}
