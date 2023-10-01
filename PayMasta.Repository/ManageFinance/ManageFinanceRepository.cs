using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.ManageFinanceVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageFinance
{
    public class ManageFinanceRepository: IManageFinanceRepository
    {
        private string connectionString;

        public ManageFinanceRepository()
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
        public async Task<GetManageFinance> GetPiChartData(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"declare @TotalCost decimal(12,2);
                                                         select @TotalCost=SUM(Cast(wt.TotalAmount as decimal(12,2))) from WalletTransaction wt
                                                        inner join SubCategory sc on sc.Id=wt.SubCategoryId
                                                        where
                                                        MONTH(wt.CreatedAt) = MONTH(GETDATE())
                                                        AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
                                                        SELECT
                                                        AIRTIME=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2)))*100/@TotalCost as decimal(5,2))  from WalletTransaction wt
																			inner join SubCategory sc on sc.Id=wt.SubCategoryId
																			WHERE wt.SubCategoryId=1 and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             ),
														CABLE=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2)))*100/@TotalCost as decimal(5,2))  from WalletTransaction wt
																				inner join SubCategory sc on sc.Id=wt.SubCategoryId
																				WHERE wt.SubCategoryId=3 and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																				AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             ),
														INTERNET=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2)))*100/@TotalCost as decimal(5,2)) from WalletTransaction wt
																				inner join SubCategory sc on sc.Id=wt.SubCategoryId
																				WHERE wt.SubCategoryId=2 and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																				AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             ),
														Bills=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2)))*100/@TotalCost as decimal(5,2)) from WalletTransaction wt
																					inner join SubCategory sc on sc.Id=wt.SubCategoryId
																					INNER join MainCategory mc on mc.Id=sc.MainCategoryId
																					WHERE mc.Id=1 and MONTH(wt.CreatedAt) = MONTH(GETDATE())
																					AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             ),
														Ecom=(select 0
											                             ),
														Travel=(select 0
											                             ),
														AvgPerDaySpend=(select CAST(SUM(Cast(wt.TotalAmount as decimal(12,2)))/DAY(EOMONTH(GETDATE())) as decimal(5,2)) from WalletTransaction wt
																			inner join SubCategory sc on sc.Id=wt.SubCategoryId
																			where
																			MONTH(wt.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             ),
														OkToSpend=(select SUM(Cast(wt.TotalAmount as decimal(12,2))) from WalletTransaction wt
																			inner join SubCategory sc on sc.Id=wt.SubCategoryId
																			where
																			MONTH(wt.CreatedAt) = MONTH(GETDATE())
																			AND YEAR(wt.CreatedAt) = YEAR(GETDATE()) AND WT.SenderId=@userId
											                             );";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetManageFinance>(query,
                        new
                        {
                            userId = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetManageFinance>(query,
                        new
                        {
                            userId = userId
                        })).FirstOrDefault();
            }
        }
    }
}
