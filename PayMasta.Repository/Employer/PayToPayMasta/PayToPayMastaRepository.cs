using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.PayToPayMasta
{
    public class PayToPayMastaRepository : IPayToPayMastaRepository
    {
        private string connectionString;

        public PayToPayMastaRepository()
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
        public async Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestList(int pageNumber, int pageSize, int status, DateTime? fromDate, DateTime? toDate, string searchText,long EmployerId, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.AdminStatus AdminStatusId
                                                ,AAR.TotalAmountWithCommission AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt ,UM.StaffId,AAR.CommissionCharge,AAR.TotalAmountWithCommission
                                                ,CASE WHEN AAR.IsPaidToPayMasta=0 OR AAR.IsPaidToPayMasta IS NULL THEN 'Unpaid' WHEN AAR.IsPaidToPayMasta=1 THEN 'Paid' ELSE 'NA' END IsPaidToPayMasta,AAR.IsPaidToPayMasta IsPaidToPayMastaId
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where  UM.EmployerId=@EmployerId AND AAR.AdminStatus=1
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                AND (
                                                @searchText='' 
                                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.Email LIKE('%'+@searchText+'%'))
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.IsPaidToPayMasta=@status)
											        )
                                                ORDER BY AAR.Id DESC 
                                                OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                                FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                            EmployerId= EmployerId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            status = status,
                            EmployerId = EmployerId
                        })).ToList();
            }
        }

        public async Task<PayableAmount> GetPayAbleAmount(long EmployerId, IDbConnection exdbConnection = null)
        {
           
            string query = @"SELECT TotalAmount=(SELECT  --COUNT(AAR.Id) OVER() as TotalCount
                                                -- ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
												SUM(AAR.TotalAmountWithCommission)
                                                --,UM.Id UserId
												--,UM.Guid UserGuid
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND AAR.IsPaidToPayMasta=0 AND UM.EmployerId=@EmployerId AND AAR.AdminStatus=1
                                                ),
									    TotalUser=(SELECT  TOP 1
												COUNT(AAR.Id) OVER() as TotalCount
                                                --,UM.Id UserId
												--,UM.Guid UserGuid
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND AAR.IsPaidToPayMasta=0 AND UM.EmployerId=@EmployerId AND AAR.AdminStatus=1
                                                ORDER BY AAR.Id DESC),
                                        TotalFee=(SELECT  --COUNT(AAR.Id) OVER() as TotalCount
                                                -- ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
												SUM(AAR.CommissionCharge)
                                                --,UM.Id UserId
												--,UM.Guid UserGuid
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND AAR.IsPaidToPayMasta=0 AND UM.EmployerId=@EmployerId AND AAR.AdminStatus=1);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PayableAmount>(query,
                        new
                        {
                             EmployerId= EmployerId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PayableAmount>(query,
                        new
                        {
                            EmployerId = EmployerId
                        })).FirstOrDefault();
            }
        }

        //public async Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestListForCsv(long EmployerId, IDbConnection exdbConnection = null)
        //{
        //    string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
        //                                        ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
        //                                        ,UM.Id UserId
        //				,UM.Guid UserGuid
        //				,UM.FirstName
        //				,UM.LastName
        //				,UM.EmployerName
        //				,UM.EmployerId
        //                                        ,UM.Email
        //                                        ,UM.PhoneNumber
        //                                        ,UM.CountryCode
        //                                        ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
        //                                        ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
        //                                        ,UM.[IsActive]
        //                                        ,UM.[IsDeleted]
        //                                        ,AAR.Status StatusId
        //                                        ,AAR.AdminStatus AdminStatusId
        //                                        ,AAR.AccessAmount
        //				,AAR.Id AccessAmountId
        //				,AAR.Guid AccessAmountGuid
        //                                        ,AAR.CreatedAt ,UM.StaffId
        //                                        FROM UserMaster UM 
        //                                        INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
        //                                         where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId --AND AAR.Status=1
        //                                        ORDER BY AAR.Id DESC;";
        //    if (exdbConnection == null)
        //    {
        //        using (var dbConnection = Connection)
        //        {
        //            return (await dbConnection.QueryAsync<AccessAmountViewModel>(query,
        //                new
        //                {
        //                    EmployerId = EmployerId
        //                })).ToList();
        //        }
        //    }
        //    else
        //    {
        //        return (await exdbConnection.QueryAsync<AccessAmountViewModel>(query,
        //                new
        //                {
        //                    EmployerId = EmployerId
        //                })).ToList();
        //    }
        //}

        public async Task<List<AccessAmountViewModel>> GetEmployeesEwaRequestListForCsv(int status, DateTime? fromDate, DateTime? toDate, string searchText, long EmployerId, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT  COUNT(AAR.Id) OVER() as TotalCount
                                                ,ROW_NUMBER() OVER(ORDER BY AAR.Id DESC) AS RowNumber
                                                ,UM.Id UserId
												,UM.Guid UserGuid
												,UM.FirstName
												,UM.LastName
												,UM.EmployerName
												,UM.EmployerId
                                                ,UM.Email
                                                ,UM.PhoneNumber
                                                ,UM.CountryCode
                                                ,CASE WHEN AAR.Status=0  THEN 'Pending' WHEN AAR.Status=1 THEN 'Approved' WHEN AAR.Status=2 THEN 'Pending' WHEN AAR.Status=3 THEN 'Rejected' WHEN AAR.Status=6 THEN 'Hold' ELSE 'NA' END  [Status]
                                                ,CASE WHEN AAR.AdminStatus=0 OR AAR.AdminStatus IS NULL  THEN 'Pending' WHEN AAR.AdminStatus=1 THEN 'Approved' WHEN AAR.AdminStatus=2 THEN 'Pending' WHEN AAR.AdminStatus=3 THEN 'Rejected' WHEN AAR.AdminStatus=6 THEN 'Hold' ELSE 'NA' END  [AdminStatus]
                                                ,UM.[IsActive]
                                                ,UM.[IsDeleted]
                                                ,AAR.Status StatusId
                                                ,AAR.AdminStatus AdminStatusId
                                                ,AAR.TotalAmountWithCommission AccessAmount
												,AAR.Id AccessAmountId
												,AAR.Guid AccessAmountGuid
                                                ,AAR.CreatedAt ,UM.StaffId
                                                ,CASE WHEN AAR.IsPaidToPayMasta=0 OR AAR.IsPaidToPayMasta IS NULL THEN 'Unpaid' WHEN AAR.IsPaidToPayMasta=1 THEN 'Paid' ELSE 'NA' END IsPaidToPayMasta,AAR.IsPaidToPayMasta IsPaidToPayMastaId
                                                FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId --AND AAR.IsPaidToPayMasta=1
												  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
		                                                AND (
                                                @searchText='' 
                                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.Email LIKE('%'+@searchText+'%'))
												 AND (
												     (@status IS NULL OR @status<0) OR (AAR.IsPaidToPayMasta=@status)
											        )
                                                ORDER BY AAR.Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            searchText = searchText,
                            status = status,
                            EmployerId = EmployerId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountViewModel>(query,
                        new
                        {
                            fromDate = fromDate,
                            toDate = toDate,
                            searchText = searchText,
                            status = status,
                            EmployerId = EmployerId
                        })).ToList();
            }
        }

        public async Task<int> UpdatePayToPayMastaFlag(long employerId, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE AAR SET AAR.IsPaidToPayMasta=1
                                     FROM UserMaster UM 
                                                INNER JOIN AccessAmountRequest AAR on AAR.UserId=UM.Id
                                                 where UM.IsActive=1 and UM.IsDeleted=0 AND UM.EmployerId=@EmployerId AND AAR.AdminStatus=1
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new { EmployerId = employerId }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new { EmployerId = employerId }));
            }
        }
    }
}
