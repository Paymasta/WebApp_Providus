using Dapper;
using PayMasta.DBEntity.NonRegisterEmployerDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.CommonEmployerRepository
{
    public class CommonEmployerRepository : ICommonEmployerRepository
    {
        private string connectionString;

        public CommonEmployerRepository()
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
        public async Task<List<EmployerResponse>> GetEmployerList(bool IsActive, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT ed.[Id]
                                      ,ed.[Guid]
                                      ,ed.[UserId]
                                      ,ed.[OrganisationName]
                                      ,ed.[WorkingHoursOrDays]
                                      ,ed.[WorkingDaysInWeek]
                                      ,ed.[Status]
                                      ,ed.[IsActive]
                                      ,ed.[IsDeleted]
                                      ,ed.[CreatedAt]
                                      ,ed.[UpdatedAt]
                                      ,ed.[CreatedBy]
                                      ,ed.[UpdatedBy]
                                  FROM [dbo].[EmployerDetail] ed
                                  INNER JOIN UserMaster um on um.Id=ed.UserId
                                  WHERE um.IsActive=1 and um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerResponse>(query, new { IsActive = IsActive })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployerResponse>(query, new { IsActive = IsActive })).ToList();
            }
        }
        public async Task<List<NonRegisterEmployerResponse>> GetNonRegisteredEmployerList(string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT TOP 5  [Id]
                                      ,[Guid]
                                      ,[OrganisationName]
                                      ,[OrganisationCode]
                                  FROM [dbo].[NonRegisterEmployerDetail]
                                  WHERE isactive=1 and isdeleted=0 AND  Id not in (SELECT ISNULL(NonRegisterEmployerDetailId,0) FROM EmployerDetail) 
                                  AND (@searchText='' OR OrganisationName LIKE('%'+@searchText+'%'))";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<NonRegisterEmployerResponse>(query, new { searchText = searchText })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<NonRegisterEmployerResponse>(query, new { searchText = searchText })).ToList();
            }
        }

        public async Task<NonRegisterEmployerDetail> GetNonRegisteredEmployerByGuid(long? Id, Guid? userGuid, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                              ,[Guid]
                                              ,[OrganisationName]
                                              ,[OrganisationCode]
                                              ,[Status]
                                              ,[IsActive]
                                              ,[IsDeleted]
                                              ,[CreatedAt]
                                              ,[UpdatedAt]
                                              ,[CreatedBy]
                                              ,[UpdatedBy]
                                              ,[Email]
                                              ,[MobileNumber]
                                          FROM [dbo].[NonRegisterEmployerDetail]
                                          WHERE Guid=@EmployerGuid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<NonRegisterEmployerDetail>(query, new {EmployerGuid = userGuid })).FirstOrDefault();
                }
            }
            else

                return (await exdbConnection.QueryAsync<NonRegisterEmployerDetail>(query, new {  EmployerGuid = userGuid })).FirstOrDefault();
        }
    }

}
