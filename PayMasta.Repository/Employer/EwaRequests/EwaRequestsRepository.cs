using Dapper;
using PayMasta.DBEntity.AccessAmountRequest;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EwaRequests
{
    public class EwaRequestsRepository : IEwaRequestsRepository
    {
        private string connectionString;

        public EwaRequestsRepository()
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
        public async Task<List<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,CASE WHEN AAR.AdminStatus=0   THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
												,AAR.TotalAmountWithCommission AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt--CONVERT(varchar,AAR.[CreatedAt],3)CreatedAt
												,AAR.AdminStatus AdminStatusId
                                                ,AAR.Status StatusId
                                                ,UM.StaffId
                                                ,ISNULL(ED.IsEwaApprovalAccess,0) IsEwaApprovalAccess
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                INNER JOIN EmployerDetail ED ON ED.Id=UM.EmployerId
                                                 where UM.IsActive=1 and UM.IsDeleted=0 and UM.EmployerId=@EmployerId
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                AND (
                                                @searchText='' 
                                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.Email LIKE('%'+@searchText+'%'))
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.Status=@status)
											        )
                                                ORDER BY AAR.Id DESC 
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
            }
        }

        public async Task<AccessAmountRequest> GetAccessAmountRequestById(long accessAmountId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[AccessAmount]
                                      ,[AccessedPercentage]
                                      ,[AvailableAmount]
                                      ,[PayCycle]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[Status]
                                      ,[AdminStatus]
                                  FROM [dbo].[AccessAmountRequest]
                                  WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountRequest>(query,
                        new
                        {
                            Id = accessAmountId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountRequest>(query,
                        new
                        {
                            Id = accessAmountId,

                        })).FirstOrDefault();
            }
        }

        public async Task<int> UPdateAccessAmountRequestById(AccessAmountRequest accessAmountRequest, IDbConnection exdbConnection = null)
        {
            string query = @"
                              UPDATE [dbo].[AccessAmountRequest]
                               SET 
                                  [UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
                                  ,[Status] = @Status
                             WHERE Id=@Id 
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, accessAmountRequest));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, accessAmountRequest));
            }
        }

        public async Task<List<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>> DownloadCsvEmployeesListByEmployerId(long employerId, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
      
            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                 ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
												,AAR.TotalAmountWithCommission AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt--CONVERT(varchar,AAR.[CreatedAt],3)CreatedAt
												
                                                ,AAR.Status StatusId
                                                ,UM.StaffId
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 and UM.EmployerId=@EmployerId
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.Status=@status)
											        )
                                                ORDER BY AAR.Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            status = status,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PayMasta.ViewModel.Employer.EWAVM.EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            status = status,
                            fromDate = fromDate,
                            toDate = toDate,
                        })).ToList();
            }
        }
    }
}
