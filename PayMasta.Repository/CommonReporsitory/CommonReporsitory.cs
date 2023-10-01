using Dapper;
using PayMasta.DBEntity.Earning;
using PayMasta.DBEntity.ErrorLog;
using PayMasta.DBEntity.RandomInvoiceNumber;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.Utilities;
using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.FlutterWaveVM;
using PayMasta.ViewModel.NotificationsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.CommonReporsitory
{
    public class CommonReporsitory : ICommonReporsitory
    {
        private string connectionString;

        public CommonReporsitory()
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
        public async Task<RandomInvoiceNumber> GetInvoiceNumber(IDbConnection dbConnection)
        {
            string query = @"usp_GetInvoiceNumber";
            // DynamicParameters parameter = new DynamicParameters();
            return (await dbConnection.QueryAsync<RandomInvoiceNumber>(query,
                      commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
        public RandomInvoiceNumber GetInvoiceNumberForBulkPayment(IDbConnection dbConnection)
        {
            string query = @"usp_GetInvoiceNumber";
            // DynamicParameters parameter = new DynamicParameters();
            return (dbConnection.Query<RandomInvoiceNumber>(query,
                      commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
        public async Task<WalletService> GetWalletServices(long BankCode, IDbConnection exdbConnection = null)
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
                                      ,[HttpVerbs],[OperatorId]
                                  FROM [dbo].[WalletService]
                                  WHERE OperatorId=@BankCode";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode
                        })).FirstOrDefault();
            }
        }

        public async Task<WalletService> IsWalletServicesExists(string service, string name, IDbConnection exdbConnection = null)
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
                                      ,[HttpVerbs],[OperatorId]
                                  FROM [dbo].[WalletService]
                                  WHERE BankCode=@BankCode AND ServiceName=@ServiceName";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = service,
                            ServiceName = name,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = service,
                            ServiceName = name,
                        })).FirstOrDefault();
            }
        }

        public async Task<int> InsertWalletServices(WalletService walletService, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[WalletService]
                                               ([ServiceName]
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
                                               ,[BillerName],[OperatorId],[AccountType])
                                         VALUES
                                               (@ServiceName
                                               ,@SubCategoryId
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt
                                               ,@ImageUrl
                                               ,@BankCode
                                               ,@HttpVerbs,@RawData,@CountryCode,@BillerName,@OperatorId,@AccountType);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, walletService));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, walletService));
            }
        }

        public async Task<List<WalletServiceResponse>> GetWalletServicesListBySubcategoryId(int SubCategoryId, IDbConnection exdbConnection = null)
        {

            string query = @"usp_GetOperatorList";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@SubCategoryId", SubCategoryId);


            //string query = @"SELECT  [Id]
            //                          ,[Guid]
            //                          ,[ServiceName]
            //                          ,[SubCategoryId]
            //                          ,[IsActive]
            //                          ,[IsDeleted]
            //                          ,[CreatedAt]
            //                          ,[UpdatedAt]
            //                          ,[ImageUrl]
            //                          ,[BankCode]
            //                          ,[HttpVerbs],[CountryCode],[BillerName],[RawData],[OperatorId]
            //                      FROM [dbo].[WalletService]
            //                      WHERE SubCategoryId=@SubCategoryId AND CountryCode='NG'";
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

        public async Task<int> InsertWalletTransactions(WalletTransaction walletTransaction, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[WalletTransaction]
                                                   ([TotalAmount]
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
                                                   ,[DisplayContent],[IsUpcomingBillShow],[SubCategoryId])
                                             VALUES
                                                   (@TotalAmount
                                                   ,@CommisionId
                                                   ,@CommisionAmount
                                                   ,@WalletAmount
                                                   ,@ServiceTaxRate
                                                   ,@ServiceTax
                                                   ,@ServiceCategoryId
                                                   ,@SenderId
                                                   ,@ReceiverId
                                                   ,@AccountNo
                                                   ,@TransactionId
                                                   ,@IsAdminTransaction
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt
                                                   ,@Comments
                                                   ,@InvoiceNo
                                                   ,@TransactionStatus
                                                   ,@TransactionType
                                                   ,@TransactionTypeInfo
                                                   ,@IsBankTransaction
                                                   ,@BankBranchCode
                                                   ,@BankTransactionId
                                                   ,@VoucherCode
                                                   ,@FlatCharges
                                                   ,@BenchmarkCharges
                                                   ,@CommisionPercent
                                                   ,@DisplayContent,@IsUpcomingBillShow,@SubCategoryId);";
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

        public async Task<List<CategoryResponse>> GetCategories(bool isactive, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT sc.Id,sc.CategoryName,sc.MainCategoryId from SubCategory SC
                                    INNER JOIN MainCategory MC on MC.Id=SC.MainCategoryId
                                    WHERE sc.IsActive=@IsActive AND SC.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CategoryResponse>(query,
                        new
                        {
                            IsActive = isactive
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CategoryResponse>(query,
                        new
                        {
                            IsActive = isactive
                        })).ToList();
            }
        }

        public async Task<WalletService> GetWalletServices(string BankCode, int OperatorId, IDbConnection exdbConnection = null)
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
                                      ,[HttpVerbs],[OperatorId]
                                  FROM [dbo].[WalletService]
                                  WHERE BankCode=@BankCode AND OperatorId=@OperatorId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode,
                            OperatorId = OperatorId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode,
                            OperatorId = OperatorId,
                        })).FirstOrDefault();
            }
        }

        public async Task<WalletService> GetWalletServicesOperator(string BankCode, IDbConnection exdbConnection = null)
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
                                      ,[HttpVerbs],[OperatorId]
                                  FROM [dbo].[WalletService]
                                  WHERE BankCode=@BankCode";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<WalletService>(query,
                        new
                        {
                            BankCode = BankCode,
                        })).FirstOrDefault();
            }
        }
        public async Task<List<NotificationsResponse>> GetNotifications(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT   CASE WHEN DATEDIFF(mi, createdat ,getdate()) <= 1   
                                        THEN'1 min ago'  
                                        WHEN DATEDIFF(mi, createdat, getdate()) > 1  AND DATEDIFF(mi, createdat,  getdate()) <= 60  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(mi,createdat,  getdate()))  + ' mins ago'WHEN DATEDIFF(hh, createdat,  getdate()) <= 1  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(hh,createdat,  getdate()))  + ' hour ago'WHEN DATEDIFF(hh, createdat,  getdate()) > 1  AND DATEDIFF(hh, createdat,  getdate()) <= 24  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(hh,createdat,  getdate()))  + ' hrs ago'WHEN DATEDIFF(dd,  createdat,  getdate()) <= 1  THEN  			 
                                        CONVERT(VARCHAR, DATEDIFF(dd,createdat,  getdate()))  + ' day ago'WHEN DATEDIFF(dd,  createdat,  getdate()) > 1  AND DATEDIFF(dd,createdat,  getdate()) <= 7  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(dd,createdat,  getdate()))  + ' days ago'WHEN DATEDIFF(ww, createdat,  getdate()) <= 1  THEN  			 
                                        CONVERT(VARCHAR, DATEDIFF(ww,createdat,  getdate()))  + ' week ago'WHEN DATEDIFF(ww, createdat,  getdate()) > 1  AND DATEDIFF(ww,createdat,  getdate()) <= 4  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(ww,createdat,  getdate()))  + ' weeks ago'WHEN DATEDIFF(mm,createdat,  getdate()) <= 1  THEN  			 
                                        CONVERT(VARCHAR, DATEDIFF(mm,createdat,  getdate()))  + ' month ago'WHEN DATEDIFF(mm,createdat,  getdate()) > 1  AND DATEDIFF(mm, createdat,  getdate()) <= 12  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(mm,createdat,  getdate()))  + ' mnths ago'WHEN DATEDIFF(yy,createdat,  getdate()) <= 1  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(yy,createdat,  getdate()))  + ' year ago'WHEN DATEDIFF(yy, createdat,  getdate()) > 1  THEN  
                                        CONVERT(VARCHAR, DATEDIFF(yy,createdat,  getdate()))  + ' yrs ago'END NotificationTime,COUNT(Id) OVER() as TotalCount,*
                                        from Notifications
                                        WHERE ReceiverId=@UserId  ORDER BY Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<NotificationsResponse>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<NotificationsResponse>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
            }
        }
        public async Task<int> GetNotificationsCount(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select top 1 COUNT(Id) OVER() as TotalCount
                                        from Notifications
                                        WHERE IsRead=0 AND ReceiverId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<int>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<int>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateUserNotificationIsRead(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE Notifications SET IsRead=1 WHERE ReceiverId=@UserId
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new
                    {
                        UserId = userId
                    }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new
                {
                    UserId = userId
                }));
            }
        }

        public async Task<int> InsertDailyEarningByScheduler(IDbConnection exdbConnection = null)
        {
            string query = @"
                            SET NOCOUNT ON;

                                            DECLARE @Id bigint;
                                            DECLARE @UserId bigint;
                                            DECLARE @EmployeeId bigint;
                                            DECLARE @GrossPayMonthly decimal(10,2);
                                            DECLARE @NetPayMonthly decimal(10,2);
                                            DECLARE Earning_cursor CURSOR FOR
                                            SELECT distinct ED.Id,ED.UserId from EmployerDetail ED
                                            inner join UserMaster UM ON UM.Id=ED.UserId
                                            where UM.IsActive=1 AND UM.IsDeleted=0;

                                            OPEN Earning_cursor

                                            FETCH NEXT FROM Earning_cursor
                                            INTO @Id,@UserId

                                            WHILE @@FETCH_STATUS = 0
                                            BEGIN
                                            ;WITH cte AS(
	                                            select um.NetPayMonthly/(ed.EndDate-ed.StartDate) NetPayDaily,um.GrossPayMonthly,um.Id,um.EmployerId,MONTH(GETDATE()) CurrentMonth,YEAR(GETDATE()) CurrentYear
	                                            from UserMaster um
	                                            inner join EmployerDetail ed on ed.Id=um.EmployerId
	                                            where um.EmployerId=@Id and day(GETDATE())>=ed.StartDate and day(GETDATE())<=day(ed.EndDate)
                                            )

                                            MERGE EarningMaster AS Target
                                            USING cte AS Source
                                            ON Source.Id = Target.UserId AND Source.CurrentMonth = Target.EarningMonth AND Source.CurrentYear = Target.EarningYear
                                            WHEN NOT MATCHED BY Target THEN
	                                            INSERT (UserId,EarnedAmount,AccessedAmount,AvailableAmount,EarningMonth,EarningYear,PayCycle,IsActive,IsDeleted,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,UsableAmount) 
	                                            VALUES (Source.Id,Source.NetPayDaily, 0,Source.NetPayDaily,CurrentMonth,CurrentYear,GETDATE(),1,0,GETDATE(),GETDATE(),Source.EmployerId,Source.EmployerId,ISNULL(Source.NetPayDaily,0)*50/100)
                                            -- For Updates
                                            WHEN MATCHED THEN UPDATE SET
                                                Target.EarnedAmount		= ISNULL(Target.EarnedAmount,0)+ISNULL(Source.NetPayDaily,0),
                                                Target.AvailableAmount	= ISNULL(Target.AvailableAmount,0)+ISNULL(Source.NetPayDaily,0),
	                                            Target.UpdatedAt		= GETDATE(),
	                                            Target.UpdatedBy		= Source.EmployerId,
												Target.UsableAmount     =ISNULL(Target.UsableAmount,0)+ISNULL(Source.NetPayDaily,0)*50/100;

                                            FETCH NEXT FROM Earning_cursor INTO @Id,@UserId
                                            END
                                            CLOSE Earning_cursor;
                                            DEALLOCATE Earning_cursor;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query));
            }
        }


        public async Task<GetCms> GetFaq(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1* FROM FAQ ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
            }
        }
        public async Task<GetCms> GetTandC(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1* FROM TermAndCondition ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
            }
        }
        public async Task<GetCms> GetPrivacyPolicy(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1 * FROM PrivacyPolicy ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetCms>(query)).FirstOrDefault();
            }
        }

        public async Task<int> InsertProvidusBankResponse(ErrorLog errorLog, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[ErrorLog]
                                               ([ErrorMessage]
                                               ,[ClassName]
                                               ,[MethodName]
                                               ,[JsonData]
                                               ,[CreatedDate])
                                         VALUES
                                               (@ErrorMessage
                                               ,@ClassName
                                               ,@MethodName
                                               ,@JsonData
                                               ,@CreatedDate);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, errorLog));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, errorLog));
            }
        }
        public async Task<ErrorLog> GetProvidusBankResponseBySessionId(string id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [ErrorId]
                                      ,[ErrorMessage]
                                      ,[ClassName]
                                      ,[MethodName]
                                      ,[JsonData]
                                      ,[CreatedDate]
                                  FROM [dbo].[ErrorLog]
                                  WHERE ClassName=@SessionId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ErrorLog>(query, new
                    {
                        SessionId = id
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ErrorLog>(query, new
                {
                    SessionId = id
                })).FirstOrDefault();
            }
        }

        public async Task<EarningMaster> GetSchedulerCurrentDate(IDbConnection exdbConnection = null)
        {
            string query = @"select top 1* from EarningMaster order by id desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EarningMaster>(query)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EarningMaster>(query)).FirstOrDefault();
            }
        }


        public async Task<List<FAQResponse>> FAQ(IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                Id,
                                QuestionText,
                                Detail
                                from FAQMaster
                                 order by Id Asc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FAQResponse>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FAQResponse>(query)).ToList();
            }
        }
        public async Task<List<FaqDetailResponse>> FAQAnswers(int FaqId, IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                    Id,
                                    Detail,FaqId
                                    from [FaqDetailMaster] where FaqId=@FaqId And IsActive=1 And IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<FaqDetailResponse>(query, new
                    {
                        FaqId = FaqId
                    })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<FaqDetailResponse>(query, new
                {
                    FaqId = FaqId
                })).ToList();
            }
        }

    }
}
