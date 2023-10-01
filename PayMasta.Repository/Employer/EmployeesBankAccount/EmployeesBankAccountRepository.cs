using Dapper;
using PayMasta.DBEntity.BankDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.Employer.EmployeeBankDetailVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.EmployeesBankAccount
{
    public class EmployeesBankAccountRepository : IEmployeesBankAccountRepository
    {
        private string connectionString;

        public EmployeesBankAccountRepository()
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

        public async Task<List<BankDetailResponse>> GetEmployeesBankListByEmployerId(long employerId, long userId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT 
                                       COUNT(BD.Id) OVER() as TotalCount
                                      ,ROW_NUMBER() OVER(ORDER BY BD.Id DESC) AS RowNumber
                                      ,BD.[Id]
                                      ,BD.[Guid]
                                      ,BD.[UserId]
                                      ,ISNULL(BD.[BankName],'NA')BankName
                                      ,ISNULL(BD.[AccountNumber],'NA')AccountNumber
                                      ,ISNULL(BD.[BVN],'NA')BVN
                                      ,ISNULL(BD.[BankAccountHolderName],'NA')BankAccountHolderName
                                      ,BD.[CustomerId]
                                      ,BD.[IsActive]
                                      ,BD.[IsDeleted]
                                      ,BD.[CreatedAt]
                                      ,BD.[UpdatedAt]
                                      ,ISNULL(BD.[BankCode],'NA')BankCode
                                      ,BD.[ImageUrl]
                                      ,BD.[IsSalaryAccount]
                                      ,BD.[IsPayMastaCardApplied]
                                FROM [dbo].[BankDetail] BD
                                  INNER JOIN UserMaster UM ON UM.Id=BD.UserId
                                  WHERE UM.Id=@UserId AND UM.EmployerId=@EmployerId AND BD.IsActive=1 AND BD.IsDeleted=0
                                   AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,BD.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND (
                                @searchText='' 
                                OR UM.FirstName LIKE('%'+@searchText+'%') OR UM.LastName LIKE('%'+@searchText+'%'))
								 AND (
												     (@status IS NULL OR @status<0) OR (UM.Status=@status)
											        )
                            ORDER BY BD.Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetailResponse>(query,
                        new
                        {
                            EmployerId = employerId,
                            UserId = userId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetailResponse>(query,
                        new
                        {
                            EmployerId = employerId,
                            UserId = userId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }

        public async Task<int> SetSalaryAccount(long userId, long BankdetailId, IDbConnection exdbConnection = null)
        {

            string query = @"usp_SetPrimaryOrSalaryAccount";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            parameter.Add("@BankdetailId", BankdetailId);

            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, parameter, commandType: CommandType.StoredProcedure));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, parameter, commandType: CommandType.StoredProcedure));
            }
        }
    }
}
