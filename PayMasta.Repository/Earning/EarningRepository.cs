using Dapper;
using PayMasta.DBEntity.AccessAmountRequest;
using PayMasta.DBEntity.CommisionMaster;
using PayMasta.DBEntity.Earning;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.Utilities;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Earning
{
    public class EarningRepository : IEarningRepository
    {
        private string connectionString;

        public EarningRepository()
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
        public async Task<EarningMasterResponse> GetEarnings(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT ISNull(em.[Id],0) Id
                                      ,em.[Guid]
                                      ,um.[Id] UserId
                                      ,ISNULL(em.[EarnedAmount],0)EarnedAmount
                                      ,ISNULL(em.[AccessedAmount],0) AccessedAmount
                                      --,CAST(em.AvailableAmount/100*50 as decimal(12,2)) AvailableAmount
                                      --,ISNULL(CAST(em.UsableAmount as decimal(12,2)),0) AvailableAmount
                                      ,ISNULL(em.UsableAmount,0) AvailableAmount
                                      ,em.[PayCycle]
                                      ,em.[IsActive]
                                      ,em.[IsDeleted]
                                      ,em.[CreatedAt]
                                      ,em.[UpdatedAt]
                                      ,em.[CreatedBy]
                                      ,em.[UpdatedBy],ed.OrganisationName
									 ,CASE WHEN ed.StartDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.StartDate)+' '+ convert(char(3), DATENAME(MONTH, GETDATE()), 0) END PayCycleFrom 
									 , CASE WHEN ed.EndDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.EndDate)+' '+convert(char(3), DATENAME(MONTH, GETDATE()), 0)  END PayCycleTo 
									 --,FORMAT(ed.PayCycleTo, 'dd MMM') as PayCycleTo
                                  FROM  usermaster um
								  LEFT join [dbo].[EarningMaster] em on em.UserId=um.id
								  INNER join EmployerDetail ed on ed.Id=um.EmployerId
								  WHERE um.id=@UserId AND ( MONTH(em.CreatedAt)=Month(GETDATE()) and year(em.CreatedAt)=year(GETDATE()))";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<EarningMasterResponse> GetPayCycleDate(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT CASE WHEN ed.StartDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.StartDate)+' '+ convert(char(3), DATENAME(MONTH, GETDATE()), 0) END PayCycleFrom 
									 , CASE WHEN ed.EndDate IS NULL THEN 'NA' ELSE CONVERT(nvarchar(50), ed.EndDate)+' '+convert(char(3), DATENAME(MONTH, GETDATE()), 0)  END PayCycleTo 
									 --,FORMAT(ed.PayCycleTo, 'dd MMM') as PayCycleTo
                                  FROM  usermaster um
								  INNER join EmployerDetail ed on ed.Id=um.EmployerId
								  WHERE um.id=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EarningMasterResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }
        public async Task<AccessdAmountPercentageResponse> GetAccessAmountPercentage(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT 
                                   UserId
                                   ,SUM([AccessedPercentage])[AccessedPercentage]
                              FROM [dbo].[AccessAmountRequest]
                              WHERE UserId=@UserId  AND ( MONTH(CreatedAt)=Month(GETDATE()) and year(CreatedAt)=year(GETDATE()))
                              group by UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessdAmountPercentageResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessdAmountPercentageResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<CommisionMaster>> GetCommisions(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                    ,[Guid]
                                    ,[AmountFrom]
                                    ,[AmountTo]
                                    ,[CommisionPercent]
                                    ,[CreatedBy]
                                    ,[IsActive]
                                    ,[IsDeleted]
                                    ,[CreatedAt]
                                    ,[UpdatedAt]
                                    ,[FlatCharges]
                                    ,[BenchmarkCharges]
                                FROM [dbo].[CommisionMaster];";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CommisionMaster>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CommisionMaster>(query)).ToList();
            }
        }
        public async Task<AccessAmountRequest> IsOldEwaRequestPending(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT top 1* FROM [dbo].[AccessAmountRequest] WHERE UserId=@UserId AND (AdminStatus IS NULL OR AdminStatus=0) ORDER by Id Desc;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AccessAmountRequest>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AccessAmountRequest>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<GetTransactionHistoryResponse>> GetTransactionHistory(long userId, int categoryId, int month, int pageNumber, int pageSize, IDbConnection exdbConnection = null)
        {
            string query = @"select DISTINCT      COUNT(WT.WalletTransactionId) OVER() as TotalCount
                                            ,ROW_NUMBER() OVER(ORDER BY WT.WalletTransactionId DESC) AS RowNumber
                                            ,WT.WalletTransactionId
                                            ,WT.[Guid]
                                            ,WT.TotalAmount
                                            ,WT.AccountNo
                                            ,WT.TransactionId
                                            ,WT.InvoiceNo
                                            ,WT.TransactionType
                                            ,WT.TransactionTypeInfo
                                            ,WT.BankBranchCode 
                                            ,WT.BankTransactionId 
                                            ,sc.servicename CategoryName
											,WT.CreatedAt
                                           ,CASE WHEN WT.SubCategoryId=7 AND WT.TransactionType='CREDIT' THEN 'Money received by using '+sc.servicename +' '+WT.Comments
											WHEN WT.SubCategoryId=7 AND WT.TransactionType='DEBIT' THEN 'Money sent by using '+sc.servicename +' '+WT.Comments
											ELSE 'Recharge of '+sc.servicename +' '+WT.AccountNo END  Description
											--,'Recharge of '+sc.servicename +' '+WT.AccountNo Description
                                            ,CASE WHEN WT.TransactionStatus=1 THEN 'Success' WHEN WT.TransactionStatus=2 THEN 'Pending' WHEN WT.TransactionStatus=3 THEN 'Rejected'
											WHEN WT.TransactionStatus=5 THEN 'Failed' ELSE 'FALIED' END TransactionStatus
                                            from WalletTransaction WT
                                            INNER JOIN walletservice sc on  sc.id=WT.ServiceCategoryId
                                            INNER JOIN UserMaster um on  um.Id=WT.SenderId
                                            WHERE um.Id=@UserId 
                                           AND ( (@month IS NULL OR @month=0) OR (MONTH(WT.CreatedAt)=@month AND YEAR(WT.CreatedAt)=YEAR(GETDATE())))
										   AND ( (@SubcategoryId IS NULL OR @SubcategoryId=0) OR (WT.SubCategoryId=@SubcategoryId))
                                        ORDER BY WT.WalletTransactionId DESC
                                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                             FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetTransactionHistoryResponse>(query,
                        new
                        {
                            UserId = userId,
                            SubcategoryId = categoryId,
                            month = month,
                            pageSize = pageSize,
                            pageNumber = pageNumber,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetTransactionHistoryResponse>(query,
                        new
                        {
                            UserId = userId,
                            SubcategoryId = categoryId,
                            month = month,
                        })).ToList();
            }
        }

        public async Task<int> InsertAccessAmountRequest(AccessAmountRequest accessAmountEntityt, IDbConnection exdbConnection = null)
        {
            string query = @"
                            INSERT INTO [dbo].[AccessAmountRequest]
                                               ([UserId]
                                               ,[AccessAmount]
                                               ,[AccessedPercentage]
                                               ,[AvailableAmount]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[CreatedBy],[Status],[IsPaidToPayMasta],[CommissionCharge],[TotalAmountWithCommission],[AdminStatus]
                                               )
                                         VALUES
                                               (@UserId
                                               ,@AccessAmount
                                               ,@AccessedPercentage
                                               ,@AvailableAmount
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@CreatedBy,@Status,@IsPaidToPayMasta,@CommissionCharge,@TotalAmountWithCommission,@AdminStatus
                                              )
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, accessAmountEntityt));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, accessAmountEntityt));
            }
        }

        public async Task<List<GetTodayTransactionHistoryResponse>> GetTodaysTransactionHistory(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select DISTINCT top 5 WT.WalletTransactionId
                                            ,WT.[Guid]
                                            ,WT.TotalAmount
                                            ,WT.AccountNo
                                            ,WT.TransactionId
                                            ,WT.InvoiceNo
                                            ,WT.TransactionType
                                            ,WT.TransactionTypeInfo
                                            ,WT.BankBranchCode 
                                            ,WT.BankTransactionId 
                                            ,sc.servicename CategoryName
											,WT.CreatedAt
											,'Recharge of '+sc.servicename +' '+WT.AccountNo Description
                                            ,CASE WHEN WT.TransactionStatus=1 THEN 'Success' WHEN WT.TransactionStatus=2 THEN 'FAILED' ELSE 'FALIED' END TransactionStatus
                                            from WalletTransaction WT
                                            INNER JOIN walletservice sc on  sc.id=WT.ServiceCategoryId
                                            INNER JOIN UserMaster um on  um.Id=WT.SenderId
                                            WHERE um.Id=@UserId and convert(date,WT.CreatedAt)=convert(date,getdate())
                            ORDER BY WT.WalletTransactionId DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetTodayTransactionHistoryResponse>(query,
                        new
                        {
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetTodayTransactionHistoryResponse>(query,
                        new
                        {
                            UserId = userId,

                        })).ToList();
            }
        }

        public async Task<List<UpComingBills>> GetUpcomingBillsHistory(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT DATEDIFF(DAY,GETDATE(),DATEADD(MONTH,1,LastRecharge)) OverDue
                                            ,*,DATEADD(MONTH,1,LastRecharge) RechargeOn FROM (
                                            SELECT WT.WalletTransactionId,WT.Guid,WT.TotalAmount,WT.AccountNo,MAX(WT.CreatedAt)LastRecharge,WT.SenderId,WS.ServiceName,ws.SubCategoryId,WS.BankCode,ws.BillerName
                                            ,WT.TransactionId,ISNULL(WT.IsUpcomingBillShow,1)IsUpcomingBillShow,WT.InvoiceNo ReferenceId
                                            FROM WalletTransaction WT
                                            INNER JOIN WalletService WS on ws.Id=WT.ServiceCategoryId
                                            GROUP BY WT.WalletTransactionId,WT.Guid,WT.TotalAmount,WT.AccountNo,WT.SenderId,WS.ServiceName,ws.SubCategoryId,WS.BankCode,ws.BillerName
											,WT.IsUpcomingBillShow
                                            ,WT.TransactionId,WT.InvoiceNo 
                                            ) AS T
                                            --WHERE DATEDIFF(DAY,GETDATE(),DATEADD(MONTH,1,LastRecharge))<=5
                                            WHERE  IsUpcomingBillShow=1 AND DATEDIFF(DAY,GETDATE(),DATEADD(MONTH,1,LastRecharge))<=5 and SenderId=@UserId AND SubCategoryId<>1 AND  SubCategoryId<>6;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UpComingBills>(query,
                        new
                        {
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UpComingBills>(query,
                        new
                        {
                            UserId = userId,

                        })).ToList();
            }
        }

        public async Task<WalletTransaction> GetTransactionsByTransactionId(long userId, long walletTransactionId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [WalletTransactionId]
                                      ,[Guid]
                                      ,[TotalAmount]
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
                                  FROM [dbo].[WalletTransaction]
                                  WHERE WalletTransactionId=@WalletTransactionId AND SenderId=@SenderId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletTransaction>(query,
                        new
                        {
                            SenderId = userId,
                            WalletTransactionId = walletTransactionId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletTransaction>(query,
                        new
                        {
                            SenderId = userId,
                            WalletTransactionId = walletTransactionId
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateTransactionStatusForUpcoingBills(WalletTransaction walletTransaction, IDbConnection exdbConnection = null)
        {
            string query = @"
                               UPDATE [dbo].[WalletTransaction]
                               SET  [UpdatedAt] = @UpdatedAt
                                   ,[IsUpcomingBillShow] = @IsUpcomingBillShow
                                  WHERE WalletTransactionId=@WalletTransactionId
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, walletTransaction));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, walletTransaction));
            }
        }
        public async Task<GetDashboardGraphResponse> GetDashboardGraphData(long userId, int week, IDbConnection exdbConnection = null)
        {
            //new change
            string query = @"SELECT SundayData=(
														select COUNT(WT.WalletTransactionId) from WalletTransaction WT
														INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														AND( DATENAME(dw,WT.CreatedAt)='Sunday' 
														)
														and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														),
									 MondayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction WT
														 INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Monday')
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 TuesdayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction WT
														 INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Tuesday')
					                                     and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 WednesdayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction WT
														  INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Wednesday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 ThursdayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction  WT
														 INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND( DATENAME(dw,WT.CreatedAt)='Thursday')
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 FridayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction WT
														  INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Friday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
														 ),
									 SaturdayData=(
														 select COUNT(WalletTransactionId) from WalletTransaction WT
														  INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
														 where WT.TransactionStatus=1 AND UM.Id=@EmployerId AND UM.IsEmployerRegister=1
														 AND (DATENAME(dw,WT.CreatedAt)='Saturday' )
														 and( WT.CreatedAt >= DATEADD(WEEK, @Week, GETDATE()))
					 );";
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
            //string query = @"select DATENAME(MONTH,WT.CreatedAt) DataName,COUNT(DATENAME(MONTH,WT.CreatedAt)) Total
            //                            from WalletTransaction WT
            //                            inner join UserMaster UM on UM.Id=WT.SenderId
            //                            where DATEDIFF(MONTH,WT.CreatedAt,GETDATE())<@filterDate AND UM.Id=@UserId
            //                            GROUP BY  DATENAME(MONTH,WT.CreatedAt);";

            //new
            string query = @"select DATENAME(MONTH,WT.CreatedAt) DataName,COUNT(DATENAME(MONTH,WT.CreatedAt)) Total,MONTH(WT.CreatedAt)
                                        from WalletTransaction WT
                                        inner join UserMaster UM on UM.Id=WT.SenderId
                                        where DATEDIFF(MONTH,WT.CreatedAt,GETDATE())<@filterDate AND UM.Id=@UserId
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
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            UserId = userId
                        })).ToList();
            }
        }

        public async Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardYearlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null)
        {

            string query = @"select DATENAME(YEAR,WT.CreatedAt)+' Year' DataName,COUNT(DATENAME(YEAR,WT.CreatedAt)) Total from WalletTransaction WT
                                        inner join UserMaster UM on UM.Id=WT.SenderId
                                        where DATEDIFF(YEAR,WT.CreatedAt,GETDATE())<@filterDate AND UM.Id=@UserId
                                        GROUP BY  DATENAME(YEAR,WT.CreatedAt);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {

                            filterDate = filterDate,
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            UserId = userId
                        })).ToList();
            }
        }

        public async Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardWeeklyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null)
        {

            string query = @"select DATENAME(WEEK,WT.CreatedAt)+' Week' DataName,COUNT(DATENAME(WEEK,WT.CreatedAt)) Total from WalletTransaction WT
                                        inner join UserMaster UM on UM.Id=WT.SenderId
                                        where DATEDIFF(WEEK,WT.CreatedAt,GETDATE())<@filterDate AND UM.Id=@UserId
                                        GROUP BY  DATENAME(WEEK,WT.CreatedAt);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {

                            filterDate = filterDate,
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetDashboardMonthlyGraphResponse>(query,
                        new
                        {
                            filterDate = filterDate,
                            UserId = userId
                        })).ToList();
            }
        }

        public async Task<GetTransactionResponse> GetTransactionsByTransactionIdAndUserId(long userId, long walletTransactionId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT    WT.[WalletTransactionId]
                                      ,WT.[Guid]
                                      ,WT.[TotalAmount]
                                      ,WT.[CommisionId]
                                      ,WT.[CommisionAmount]
                                      ,WT.[WalletAmount]
                                      ,WT.[ServiceTaxRate]
                                      ,WT.[ServiceTax]
                                      ,WT.[ServiceCategoryId]
                                      ,WT.[SenderId]
                                      ,WT.[ReceiverId]
                                      ,WT.[AccountNo]
                                      ,WT.[TransactionId]
                                      ,WT.[IsAdminTransaction]
                                      ,WT.[IsActive]
                                      ,WT.[IsDeleted]
                                      ,WT.[CreatedAt]
                                      ,WT.[UpdatedAt]
                                      ,WT.[Comments]
                                      ,WT.[InvoiceNo]
                                      ,WT.[TransactionStatus]
                                      ,WT.[TransactionType]
                                      ,WT.[TransactionTypeInfo]
                                      ,WT.[IsBankTransaction]
                                      ,WT.[BankBranchCode]
                                      ,WT.[BankTransactionId]
                                      ,WT.[VoucherCode]
                                      ,WT.[FlatCharges]
                                      ,WT.[BenchmarkCharges]
                                      ,WT.[CommisionPercent]
                                      ,WT.[DisplayContent]
                                      ,WT.[IsUpcomingBillShow],WS.SubCategoryId
                                  FROM [dbo].[WalletTransaction] WT
								  INNER JOIN WalletService WS ON WS.Id=WT.ServiceCategoryId
                                  WHERE WalletTransactionId=@WalletTransactionId AND SenderId=@SenderId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetTransactionResponse>(query,
                        new
                        {
                            SenderId = userId,
                            WalletTransactionId = walletTransactionId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetTransactionResponse>(query,
                        new
                        {
                            SenderId = userId,
                            WalletTransactionId = walletTransactionId
                        })).FirstOrDefault();
            }
        }

        public async Task<EmployerResponse> GetEmployerDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select UM.Id UserId
                                    ,UMM.Id EmployerId
                                    ,UMM.Status
                                    ,UMM.FirstName
                                    ,ED.StartDate
                                    ,ED.EndDate
                                    from EmployerDetail ED
                                    INNER JOIN UserMaster UM ON UM.EmployerId=ED.Id
                                    INNER JOIN UserMaster UMM ON UMM.Id=ED.UserId
                                    WHERE um.Id=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployerResponse>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<List<GetCommission>> GetCommisionsList(IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                                  ,[Guid]
                                  ,[AmountFrom]
                                  ,[AmountTo]
                                  ,[CommisionPercent]
                                  ,[CreatedBy]
                                  ,[IsActive]
                                  ,[IsDeleted]
                                  ,[CreatedAt]
                                  ,[UpdatedAt]
                                  ,[FlatCharges]
                                  ,[BenchmarkCharges]
                              FROM [dbo].[CommisionMaster];";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetCommission>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetCommission>(query)).ToList();
            }
        }

        public async Task<List<AddedBanListResponse>> GetAddedBankList(long userId, IDbConnection exdbConnection = null)
        {

            string query = @"select * from BankDetail where UserId=@UserId AND IsActive=1 AND Isdeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<AddedBanListResponse>(query,
                        new
                        {
                            UserId = userId,

                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<AddedBanListResponse>(query,
                        new
                        {
                            UserId = userId,
                        })).ToList();
            }
        }
    }
}
