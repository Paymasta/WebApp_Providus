using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private string connectionString;

        public DashboardRepository()
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
        public async Task<DashboardResponse> GetDashboardData(long userId, DateTime? fromDate = null, DateTime? toDate = null, int month = 0, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT
                                                        TotalEmployees=(SELECT 
                                                                            COUNT(Id) 
                                                                         FROM [dbo].[UserMaster] 
                                                                         WHERE UserType  IN (4) 
                                                                         AND IsDeleted=0 AND IsActive=1 AND EmployerId=@EmployerId AND IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
														TotalActiveEmployees=(SELECT 
                                                                            COUNT(Id) 
                                                                         FROM [dbo].[UserMaster] 
                                                                         WHERE UserType  IN (4) 
                                                                         AND IsDeleted=0 AND IsActive=1 AND Status=1 AND EmployerId=@EmployerId AND IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
														TotalWithdrawRequest=(SELECT 
                                                                            COUNT(AAR.Id) 
                                                                         FROM [dbo].[AccessAmountRequest]  AAR
																		 INNER JOIN UserMaster UM on UM.Id=AAR.UserId
                                                                         WHERE AAR.IsDeleted=0 AND AAR.IsActive=1  AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
														TotalPendingWithdrawRequest=(SELECT 
                                                                            COUNT(AAR.Id) 
                                                                         FROM [dbo].[AccessAmountRequest]  AAR
																		 INNER JOIN UserMaster UM on UM.Id=AAR.UserId
                                                                         WHERE AAR.IsDeleted=0 AND AAR.IsActive=1 and AAR.Status in (0,2)  AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
													   TotalTransactions=(SELECT 
                                                                            COUNT(WT.WalletTransactionId) 
                                                                         FROM [dbo].[WalletTransaction]  WT
																		 INNER JOIN UserMaster UM on UM.Id=WT.SenderId
                                                                         WHERE WT.IsDeleted=0 AND WT.IsActive=1  AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,WT.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(WT.CreatedAt)=@month AND YEAR(WT.CreatedAt)=YEAR(GETDATE()))
											                               )),
														TotalWithdrawlValue=(SELECT 
                                                                            SUM(TotalAmountWithCommission) 
                                                                         FROM [dbo].[AccessAmountRequest]  AAR
																		 INNER JOIN UserMaster UM on UM.Id=AAR.UserId
                                                                         WHERE AAR.IsDeleted=0 AND AAR.IsActive=1 AND AAR.AdminStatus=1  AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE()))
											                               ));";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<DashboardResponse>(query,
                        new
                        {

                            fromDate = fromDate,
                            todate = toDate,
                            month = month,
                            EmployerId = userId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<DashboardResponse>(query,
                        new
                        {
                            fromDate = fromDate,
                            todate = toDate,
                            month = month,
                            EmployerId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<PendingEWRRequestResponse>> GetDashboardEWARequestData(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"select UM.FirstName
										,um.LastName
										,AAR.TotalAmountWithCommission AccessAmount
										,AAR.CreatedAt
										,AAR.Id
										,AAR.UserId
										from AccessAmountRequest AAR
										inner join UserMaster UM on UM.Id=AAR.UserId
										WHERE UM.EmployerId=@EmployerId AND AAR.Status in(0,2) ORDER by AAR.Id desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<PendingEWRRequestResponse>(query,
                        new
                        {

                            EmployerId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<PendingEWRRequestResponse>(query,
                        new
                        {

                            EmployerId = userId
                        })).ToList();
            }
        }

        public async Task<GetDashboardGraphResponse> GetDashboardGraphData(long userId, int week, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT SundayData=(
														select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														AND( DATENAME(dw,WT.CreatedAt)='Sunday' 
														)
														and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														),
									 MondayData=(
														 select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Monday')
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 TuesdayData=(
														select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Tuesday')
					                                     and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 WednesdayData=(
														select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Wednesday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 ThursdayData=(
														select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND( DATENAME(dw,WT.CreatedAt)='Thursday')
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 FridayData=(
														 select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Friday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 SaturdayData=(
														select COUNT(WT.Id) from AccessAmountRequest WT
														INNER JOIN UserMaster UM ON UM.Id=WT.UserId
														where WT.AdminStatus=1 AND UM.EmployerId=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Saturday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE())));";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardGraphResponse>(query,
                        new
                        {

                            Week = week,
                            EmployerId = userId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardGraphResponse>(query,
                        new
                        {
                            Week = week,
                            EmployerId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardMonthlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null)
        {
            //old
            //    string query = @"select DATENAME(MONTH,WT.CreatedAt) DataName,COUNT(DATENAME(MONTH,WT.CreatedAt)) Total from WalletTransaction WT
            //inner join UserMaster UM on UM.Id=WT.SenderId
            //where DATEDIFF(MONTH,WT.CreatedAt,GETDATE())<@filterDate AND UM.EmployerId=@EmployerId
            //GROUP BY  DATENAME(MONTH,WT.CreatedAt);";

            //new
            string query = @"select DATENAME(MONTH,WT.CreatedAt) DataName,COUNT(DATENAME(MONTH,WT.CreatedAt)) Total,MONTH(WT.CreatedAt) from AccessAmountRequest WT
								inner join UserMaster UM on UM.Id=WT.UserId
								where DATEDIFF(MONTH,WT.CreatedAt,GETDATE())<@filterDate AND UM.EmployerId=@EmployerId AND WT.AdminStatus=1
								GROUP BY  DATENAME(MONTH,WT.CreatedAt),MONTH(WT.CreatedAt)
								ORDER BY MONTH(WT.CreatedAt) asc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {

                            filterDate = filterDate,
                            EmployerId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            EmployerId = userId
                        })).ToList();
            }
        }

        public async Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardYearlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null)
        {

            string query = @"select DATENAME(YEAR,WT.CreatedAt)+' Year' DataName,COUNT(DATENAME(YEAR,WT.CreatedAt)) Total from AccessAmountRequest WT
                                        inner join UserMaster UM on UM.Id=WT.UserId
                                        where DATEDIFF(YEAR,WT.CreatedAt,GETDATE())<@filterDate AND UM.EmployerId=@EmployerId AND WT.AdminStatus=1
                                        GROUP BY  DATENAME(YEAR,WT.CreatedAt);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {

                            filterDate = filterDate,
                            EmployerId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            EmployerId = userId
                        })).ToList();
            }
        }

        public async Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardWeeklyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null)
        {

            string query = @"select DATENAME(WEEK,WT.CreatedAt)+' Week' DataName,COUNT(DATENAME(WEEK,WT.CreatedAt)) Total from AccessAmountRequest WT
                                        inner join UserMaster UM on UM.Id=WT.UserId
                                        where DATEDIFF(WEEK,WT.CreatedAt,GETDATE())<@filterDate AND UM.EmployerId=@EmployerId AND WT.AdminStatus=1
                                        GROUP BY  DATENAME(WEEK,WT.CreatedAt);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {

                            filterDate = filterDate,
                            EmployerId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            EmployerId = userId
                        })).ToList();
            }
        }

        public async Task<GetDashboardPayCycleResponse> GetDashboardPayCycleData(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"select top 1 CASE WHEN ed.StartDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.StartDate)+' '+datename(MONTH,GETDATE()) END PayCycleFrom 
									 , CASE WHEN ed.EndDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.EndDate)+' '+datename(MONTH,GETDATE()) END PayCycleTo  from EmployerDetail ed where ed.Id=@EmployerId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardPayCycleResponse>(query,
                        new
                        {

                            EmployerId = userId,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardPayCycleResponse>(query,
                        new
                        {
                            EmployerId = userId
                        })).FirstOrDefault();
            }
        }
    }
}
