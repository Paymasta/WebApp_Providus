using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.ServiceCategory;
using PayMasta.ViewModel.BudgetVM;
using PayMasta.DBEntity.UserBudget;

namespace PayMasta.Repository.Budget
{
    public class BudgetRepository : IBudgetRepository
    {
        private string connectionString;

        public BudgetRepository()
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

        public async Task<List<BudgetServiceCategory>> GetServiceList(IDbConnection exdbConnection = null)
        {
            string query = @"SELECT Id
                            ,Guid
                            ,CategoryName 
                            FROM SubCategory
                            WHERE IsActive=1 AND IsDeleted=0 AND MainCategoryId<>7";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BudgetServiceCategory>(query)).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BudgetServiceCategory>(query)).ToList();
            }
        }

        public async Task<UserBudgeting> SaveBudget(UserBudgeting userBudgeting, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[UserBudgeting]
                                               ([UserId]
                                               ,[CategoryId]
                                               ,[BudgetAmount]
                                               ,[UsedPercentage]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[UpdatedAt])
                                         VALUES
                                               (@UserId
                                               ,@CategoryId
                                               ,@BudgetAmount
                                               ,@UsedPercentage
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt);
                              SELECT * from UserBudgeting WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)
                                ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserBudgeting>(query, userBudgeting)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserBudgeting>(query, userBudgeting)).FirstOrDefault();
            }
        }

        public async Task<UserBudgeting> GetCurrentMonthBudget(long userId, long categoryId, IDbConnection exdbConnection = null)
        {
            string query = @" SELECT * from UserBudgeting 
                                WHERE UserId=@UserId 
                                AND  MONTH(CreatedAt) = MONTH(GETDATE())
                                AND YEAR(CreatedAt) = YEAR(GETDATE())  
                                AND CategoryId=@CategoryId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserBudgeting>(query, new
                    {
                        UserId = userId,
                        CategoryId = categoryId,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserBudgeting>(query, new
                {
                    UserId = userId,
                    CategoryId = categoryId,
                })).FirstOrDefault();
            }
        }
        public async Task<ExpenseTrack> GetExpenses(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"  SELECT
                                                        AIRTIME=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2))) as decimal(12,2))  from WalletTransaction wt
																			inner join SubCategory sc on sc.Id=wt.SubCategoryId
																			WHERE wt.SubCategoryId=1 
																			--and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																			--AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) 
																			AND WT.SenderId=@userId
											                             ),
														CABLE=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2))) as decimal(12,2))  from WalletTransaction wt
																				inner join SubCategory sc on sc.Id=wt.SubCategoryId
																				WHERE wt.SubCategoryId=3 
																				--and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																				--AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) 
																				AND WT.SenderId=@userId
											                             ),
														INTERNET=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2))) as decimal(12,2)) from WalletTransaction wt
																				inner join SubCategory sc on sc.Id=wt.SubCategoryId
																				WHERE wt.SubCategoryId=2 
																				--and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																				--AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) 
																				AND WT.SenderId=@userId
											                             ),
														ELECTICITY=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2))) as decimal(12,2)) from WalletTransaction wt
															inner join SubCategory sc on sc.Id=wt.SubCategoryId
															WHERE wt.SubCategoryId=4
															--and MONTH(wt.CreatedAt) = MONTH(GETDATE())
															--AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) 
															AND WT.SenderId=@userId
														),
														DATABUNDLE=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2))) as decimal(12,2)) from WalletTransaction wt
															inner join SubCategory sc on sc.Id=wt.SubCategoryId
															WHERE wt.SubCategoryId=5
															--and MONTH(wt.CreatedAt) = MONTH(GETDATE())
															--AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) 
															AND WT.SenderId=@userId
														);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ExpenseTrack>(query, new
                    {
                        UserId = userId,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ExpenseTrack>(query, new
                {
                    UserId = userId,
                })).FirstOrDefault();
            }
        }

        public async Task<ExpenseTrack> GetBudget(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT
                                                        AIRTIME=(select TOP 1 CAST(UB.BudgetAmount as decimal(12,2))  from UserBudgeting UB
																			inner join SubCategory sc on sc.Id=UB.CategoryId
																			WHERE UB.CategoryId=1 
																			and MONTH(UB.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(UB.CreatedAt) = YEAR(GETDATE()) 
																			AND UB.UserId=@userId
											                             ),
														CABLE=(select TOP 1 CAST(UB.BudgetAmount as decimal(12,2))  from UserBudgeting UB
																			inner join SubCategory sc on sc.Id=UB.CategoryId
																			WHERE UB.CategoryId=3 
																			and MONTH(UB.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(UB.CreatedAt) = YEAR(GETDATE()) 
																			AND UB.UserId=@userId
											                             ),
														INTERNET=(select TOP 1 CAST(UB.BudgetAmount as decimal(12,2))  from UserBudgeting UB
																			inner join SubCategory sc on sc.Id=UB.CategoryId
																			WHERE UB.CategoryId=2
																			and MONTH(UB.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(UB.CreatedAt) = YEAR(GETDATE()) 
																			AND UB.UserId=@userId
											                             ),
														ELECTICITY=(select TOP 1 CAST(UB.BudgetAmount as decimal(12,2))  from UserBudgeting UB
																			inner join SubCategory sc on sc.Id=UB.CategoryId
																			WHERE UB.CategoryId=4
																			and MONTH(UB.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(UB.CreatedAt) = YEAR(GETDATE()) 
																			AND UB.UserId=@userId
														),
														DATABUNDLE=(select TOP 1 CAST(UB.BudgetAmount as decimal(12,2))  from UserBudgeting UB
																			inner join SubCategory sc on sc.Id=UB.CategoryId
																			WHERE UB.CategoryId=5
																			and MONTH(UB.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(UB.CreatedAt) = YEAR(GETDATE()) 
																			AND UB.UserId=@userId
														);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<ExpenseTrack>(query, new
                    {
                        UserId = userId,
                    })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<ExpenseTrack>(query, new
                {
                    UserId = userId,
                })).FirstOrDefault();
            }
        }

        public async Task<List<DailyExpenseAmount>> GetDailyExpense(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select TotalAmount from WalletTransaction where SenderId=@UserId AND SubCategoryId in(1,2,3,4,5) AND CONVERT(date,CreatedAt)=CONVERT(date,GETDATE())";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<DailyExpenseAmount>(query, new
                    {
                        UserId = userId,
                    })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<DailyExpenseAmount>(query, new
                {
                    UserId = userId,
                })).ToList();
            }
        }
    }
}
