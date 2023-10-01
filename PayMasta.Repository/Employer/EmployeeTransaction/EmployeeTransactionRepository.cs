using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EmployeeTransaction
{
    public class EmployeeTransactionRepository : IEmployeeTransactionRepository
    {
        private string connectionString;

        public EmployeeTransactionRepository()
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
        public async Task<List<EmployeesListForTransactions>> GetEmployeesList(long employerId, int pageNumber, int pageSize, int month, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A') StaffId
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  ,ISnull(ED.[OrganisationName],'N/A')[EmployerName]
										  ,UM.[CreatedAt],UM.[Status] StatusId
                                      FROM [dbo].[UserMaster] UM
									  INNER JOIN EmployerDetail ED on ED.Id=UM.EmployerId AND UM.IsEmployerRegister=1
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId AND  (SELECT count(*) AS totalTransation from WalletTransaction WHERE WalletTransaction.SenderId=um.Id)>0
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
                           
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR Email LIKE('%'+@searchText+'%'))
								  AND ( (@month IS NULL OR @month=0) OR (MONTH(UM.CreatedAt)=@month AND YEAR(UM.CreatedAt)=YEAR(GETDATE())))
                            ORDER BY UM.Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            month = month,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            month = month,
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListForTransactions>> GetEmployeesListWeb(long employerId, int pageNumber, int pageSize, int month, DateTime? fromDate, DateTime? toDate, string searchText, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A') StaffId
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  ,ISnull(ED.[OrganisationName],'N/A')[EmployerName]
										  ,UM.[CreatedAt],UM.[Status] StatusId
                                      FROM [dbo].[UserMaster] UM
									  INNER JOIN EmployerDetail ED on ED.Id=UM.EmployerId AND UM.IsEmployerRegister=1
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
                           
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR Email LIKE('%'+@searchText+'%'))
								  AND ( (@month IS NULL OR @month=0) OR (MONTH(UM.CreatedAt)=@month AND YEAR(UM.CreatedAt)=YEAR(GETDATE())))
                            ORDER BY UM.Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            month = month,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            month = month,
                        })).ToList();
            }
        }
        public async Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwals(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                            ,UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                            ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
                                            ,AAR.Status StatusId
                                            ,AAR.AdminStatus AdminStatusId
                                            ,AAR.TotalAmountWithCommission AccessAmount
                                            ,AAR.Id AccessAmountId
                                            ,AAR.Guid AccessAmountGuid
                                            ,AAR.CreatedAt
                                            FROM UserMaster UM 
                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                            INNER JOIN EarningMaster EM ON EM.UserId=UM.Id
                                            where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
                                            AND ( (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE())))
                                            ORDER BY AAR.Id DESC 
                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);	";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
            }
        }
        public async Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForEmployer(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null)
        {

            string query = @"												 ;WITH tblEarningMaster as(
select TOP 1 EarnedAmount,AvailableAmount,UserId from EarningMaster where UserId=@UserId AND MONTH(CreatedAt)=MONTH(GETDATE()) AND YEAR(CreatedAt)=YEAR(GETDATE())
)
												 SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                            ,UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                            ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
                                            ,AAR.Status StatusId
                                            ,AAR.AdminStatus AdminStatusId
                                            ,AAR.TotalAmountWithCommission AccessAmount
                                            ,AAR.Id AccessAmountId
                                            ,AAR.Guid AccessAmountGuid
                                            ,AAR.CreatedAt
                                            FROM UserMaster UM 
                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                            INNER JOIN tblEarningMaster EM ON EM.UserId=UM.Id
                                            where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
                                            AND ( (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE())))
                                            ORDER BY AAR.Id DESC 
                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);		";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
            }
        }
        public async Task<List<EmployeesWithdrawlsForApp>> GetEmployeesWithdrwalsForApp(long userid, int month, int pageSize, int pageNumber, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                            ,UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,UM.EmployerName 
                                            ,UM.FirstName 
                                            ,UM.LastName 
                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                            ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
                                            ,AAR.Status StatusId
                                            ,AAR.AdminStatus AdminStatusId
                                            ,AAR.TotalAmountWithCommission AccessAmount
                                            ,AAR.Id AccessAmountId
                                            ,AAR.Guid AccessAmountGuid
                                            ,AAR.CreatedAt
                                            FROM UserMaster UM 
                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                           -- INNER JOIN EarningMaster EM ON EM.UserId=UM.Id
                                            where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
                                            AND ( (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE())))
                                            ORDER BY AAR.Id DESC 
                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);	";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawlsForApp>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawlsForApp>(query,
                        new
                        {
                            UserId = userid,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
            }
        }
        public async Task<EmployeeEwaDetail> GetEmployeesEwaRequestDetail(long userid, IDbConnection exdbConnection = null)
        {

            string query = @";WITH tblEarningMaster as(
select TOP 1 EarnedAmount,AvailableAmount,UserId from EarningMaster where UserId=@UserId AND MONTH(CreatedAt)=MONTH(GETDATE()) AND YEAR(CreatedAt)=YEAR(GETDATE())
)
SELECT 
                                                UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                               , UM.EmployerName
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
											    ,ISNULL(EM.EarnedAmount,0)EarnedAmount
												,ED.EndDate-ED.StartDate [WorkiingDays]
												,UM.StaffId
												,ISNULL(EM.AvailableAmount,0) AvailableAmount
                                                FROM UserMaster UM 
												LEFT JOIN tblEarningMaster EM ON EM.UserId=UM.Id
												INNER JOIN EmployerDetail ED ON ED.Id=UM.EmployerId
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeeEwaDetail>(query,
                        new
                        {
                            UserId = userid,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeeEwaDetail>(query,
                        new
                        {
                            UserId = userid,

                        })).FirstOrDefault();
            }
        }

        public async Task<List<EmployeesListForTransactions>> GetEmployeesListForCsv(long employerId, int month, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT   COUNT(UM.Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY UM.Id DESC) AS RowNumber
										  ,UM.[Id] UserId
                                          ,UM.[Guid] UserGuid
                                          ,ISnull(UM.[FirstName],'')[FirstName]
                                          ,ISnull(UM.[LastName],'')[LastName]
                                          ,UM.[Email]
                                          ,UM.[CountryCode]
                                          ,UM.[PhoneNumber]
                                          ,ISnull(UM.[StaffId],'N/A') StaffId
                                          ,CASE WHEN UM.Status=1 and UM.IsActive=1 THEN 'Active' WHEN UM.Status=0 and UM.IsActive=1 THEN 'InActive' ELSE 'NA' END  [Status]
                                          ,UM.[IsActive]
                                          ,UM.[IsDeleted]
										  ,ISnull(ED.[OrganisationName],'N/A')[EmployerName]
										  ,UM.[CreatedAt]
                                      FROM [dbo].[UserMaster] UM
									  INNER JOIN EmployerDetail ED on ED.Id=UM.EmployerId AND UM.IsEmployerRegister=1
                                      where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId
									   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,UM.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
								  AND ( (@month IS NULL OR @month=0) OR (MONTH(UM.CreatedAt)=@month AND YEAR(UM.CreatedAt)=YEAR(GETDATE())))
                            ORDER BY UM.Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            month = month,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListForTransactions>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            month = month,
                        })).ToList();
            }
        }

        public async Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForDownload(long userid, int month, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                            ,UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                            ,UM.[IsActive]
                                            ,UM.[IsDeleted]
                                            ,AAR.Status StatusId
                                            ,AAR.AccessAmount
                                            ,AAR.Id AccessAmountId
                                            ,AAR.Guid AccessAmountGuid
                                            ,AAR.CreatedAt
                                            FROM UserMaster UM 
                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                            INNER JOIN EarningMaster EM ON EM.UserId=UM.Id
                                            where UM.IsActive=1 and UM.IsDeleted=0 AND UM.Id=@UserId
                                            AND ( (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE())))
                                            ORDER BY AAR.Id DESC ;
                                           ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            UserId = userid,
                            month = month,

                        })).ToList();
            }
        }

        public async Task<List<EmployeesWithdrawls>> GetEmployeesWithdrwalsForPayCycle(long userid, IDbConnection exdbConnection = null)
        {

            string query = @"DECLARE @fromDate int; 
                                DECLARE @todate int; 
                                select @fromDate=StartDate,@todate=EndDate from EmployerDetail where id=@EmployerId
                                print @todate
                                SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                                            ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                                            ,UM.Id UserId
											                                ,Um.firstname
                                                                            ,UM.Guid UserGuid
                                                                            ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' 
                                                                            WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' 
                                                                            WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                                            ,UM.[IsActive]
                                                                            ,UM.[IsDeleted]
                                                                            ,AAR.Status StatusId
                                                                            ,AAR.AccessAmount
                                                                            ,AAR.Id AccessAmountId
                                                                            ,AAR.Guid AccessAmountGuid
                                                                            ,AAR.CreatedAt
                                                                            FROM UserMaster UM 
                                                                            INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                                            INNER JOIN EarningMaster EM ON EM.UserId=UM.Id
											                                INNER jOIN EmployerDetail ED on ED.Id=UM.employerid
                                                                            where UM.IsActive=1 and UM.IsDeleted=0 and AAR.status=1
											                                AND UM.employerid=@EmployerId  and YEAR(AAR.createdAt)=YEAR(GETDATE()) and MONTH(AAR.createdAt)=MONTH(GETDATE())
											                                 AND (
												                                        (@fromDate IS NULL OR @todate is null) 
												                                            OR 
												                                        (DAY(AAR.CreatedAt) BETWEEN  @fromDate AND @todate)
											                                            );";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            EmployerId = userid,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesWithdrawls>(query,
                        new
                        {
                            EmployerId = userid,

                        })).ToList();
            }
        }
    }
}
