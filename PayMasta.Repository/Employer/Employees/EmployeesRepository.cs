using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.Employer.EmployeesVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.Employees
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private string connectionString;

        public EmployeesRepository()
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

        public async Task<GetEmployerDetailResponse> GetEmployerDetailByGuid(Guid userGuid, IDbConnection exdbConnection = null)
        {
            string query = @"select  ed.Id
                                    ,um.FirstName
                                    ,um.LastName
                                    ,um.Address
                                    ,um.CountryCode
                                    ,um.PhoneNumber
                                    ,um.Gender
                                    ,um.UserType
                                    ,ed.OrganisationName,ed.StartDate,ed.EndDate
                                    from EmployerDetail ed
                                    inner join UserMaster um on um.Id=ed.UserId
                                    where um.Guid=@Guid and um.IsActive=1 and um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            Guid = userGuid
                        })).FirstOrDefault();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerId(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,[FirstName]
                                          ,[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,[StaffId]
                                          ,CASE WHEN Status=1 and IsActive=1 THEN 'Active' WHEN Status=0 and IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted],CreatedAt--CONVERT(varchar,[CreatedAt],3)CreatedAt
                                          ,CASE WHEN  [IsverifiedByEmployer] IS Null OR [IsverifiedByEmployer]=1 THEN 1 ELSE 0 END [IsverifiedByEmployer]
                                      FROM [dbo].[UserMaster]
                                      where EmployerId=@EmployerId and IsActive=1 and IsDeleted=0 AND IsEmployerRegister=1 and UserType=4
									  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR LastName LIKE('%'+@searchText+'%'))
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
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
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }

        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdError(long employerId, int pageNumber, int pageSize, string searchText, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {
            if (string.IsNullOrEmpty(searchText)) { searchText = ""; }
            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,[FirstName]
                                          ,[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,[StaffId]
                                          ,CASE WHEN Status=1 and IsActive=1 THEN 'Active' WHEN Status=0 and IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted],CreatedAt--CONVERT(varchar,[CreatedAt],3)CreatedAt
                                      FROM [dbo].[UserMasterError]
                                      where EmployerId=@EmployerId and IsActive=1 and IsDeleted=0 AND IsEmployerRegister=1 and UserType=4
									  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
									  AND (
                                @searchText='' 
                                OR FirstName LIKE('%'+@searchText+'%') OR LastName LIKE('%'+@searchText+'%'))
								 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY Id DESC 
                            OFFSET @pageSize * (@pageNumber - 1) ROWS 
                            FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
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
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            searchText = searchText,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }
        public async Task<int> UpdateUserNetAndGrossPay(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                           UPDATE [dbo].[UserMaster]
                                       SET 
                                          [NetPayMonthly] = @NetPayMonthly
                                          ,[GrossPayMonthly] = @GrossPayMonthly
                                          ,[UpdatedBy]=@UpdatedBy
                                          ,[UpdatedAt]=@UpdatedAt
                                     WHERE Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, userEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, userEntity));
            }
        }
        public async Task<int> ApproveUserProfileByEmployer(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                           UPDATE [dbo].[UserMaster]
                                       SET 
                                          [IsverifiedByEmployer] = @IsverifiedByEmployer
                                         
                                     WHERE Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, userEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, userEntity));
            }
        }
        public async Task<List<EmployeesListViewModel>> GetEmployeesListByEmployerIdCSV(long employerId, int status, DateTime? fromDate, DateTime? toDate, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT   COUNT(Id) OVER() as TotalCount
                                          ,ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowNumber
										  ,[Id] UserId
                                          ,[Guid] UserGuid
                                          ,[FirstName]
                                          ,[LastName]
                                          ,[Email]
                                          ,[CountryCode]
                                          ,[PhoneNumber]
                                          ,[StaffId]
                                          ,CASE WHEN Status=1 and IsActive=1 THEN 'Active' WHEN Status=0 and IsActive=1 THEN 'Inactive' ELSE 'NA' END  [Status]
                                          ,[IsActive]
                                          ,[IsDeleted],CreatedAt--CONVERT(varchar,[CreatedAt],3)CreatedAt
                                      FROM [dbo].[UserMaster]
                                      where EmployerId=@EmployerId and IsActive=1 and IsDeleted=0 AND IsEmployerRegister=1 and UserType=4
									  AND (
												        (@fromDate IS NULL OR @todate is null) 
												            OR 
												        (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											            )
								                 AND (
												     (@status IS NULL OR @status<0) OR (Status=@status)
											        )
                            ORDER BY Id DESC;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeesListViewModel>(query,
                        new
                        {
                            EmployerId = employerId,
                            fromDate = fromDate,
                            toDate = toDate,
                            status = status,
                        })).ToList();
            }
        }


        public async Task<int> BulkUploadUsersList(DataTable dt, string employerName, long empId)
        {
            using (var dbConnection = Connection)
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(dbConnection.ConnectionString))
                {

                    copy.ColumnMappings.Add(0, "FirstName");//0
                    copy.ColumnMappings.Add(1, "LastName");//1
                    copy.ColumnMappings.Add(2, "BankName");//2
                    copy.ColumnMappings.Add(3, "BankCode");//4
                    copy.ColumnMappings.Add(4, "NubanAccountNumber");//5
                    copy.ColumnMappings.Add(5, "Email");//6
                    copy.ColumnMappings.Add(6, "Password");//8
                    copy.ColumnMappings.Add(7, "PhoneNumber");//9
                    copy.ColumnMappings.Add(8, "StaffId");//14
                    copy.ColumnMappings.Add(9, "NetPayMonthly");//15
                    copy.ColumnMappings.Add(10, "GrossPayMonthly");//16
                    copy.ColumnMappings.Add(11, "CountryCode");//7

                    copy.ColumnMappings.Add(12, "DateOfBirth");//7
                    copy.ColumnMappings.Add(13, "Gender");//7
                    copy.ColumnMappings.Add(14, "MiddleName");//7
                    copy.ColumnMappings.Add(15, "BVN");//7
                    copy.ColumnMappings.Add(16, "BankAccountHolderName");//7


                    copy.DestinationTableName = "BulkUploadUserCSV";
                    copy.WriteToServer(dt);
                }

                string query = @"usp_ImportUserMasterCSV";
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@EmployerName", employerName);
                parameter.Add("@EmployerId", empId);

                await dbConnection.ExecuteAsync(query, parameter,
                                      commandType: CommandType.StoredProcedure);
                return 1;
            }
        }

        public async Task<int> BulkUploadUsersListError(DataTable dt, string employerName, long empId)
        {
            using (var dbConnection = Connection)
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(dbConnection.ConnectionString))
                {

                    copy.ColumnMappings.Add(0, "FirstName");//0
                    copy.ColumnMappings.Add(1, "LastName");//1
                    copy.ColumnMappings.Add(2, "BankName");//2
                    copy.ColumnMappings.Add(3, "BankCode");//4
                    copy.ColumnMappings.Add(4, "NubanAccountNumber");//5
                    copy.ColumnMappings.Add(5, "Email");//6
                    copy.ColumnMappings.Add(6, "Password");//8
                    copy.ColumnMappings.Add(7, "PhoneNumber");//9
                    copy.ColumnMappings.Add(8, "StaffId");//14
                    copy.ColumnMappings.Add(9, "NetPayMonthly");//15
                    copy.ColumnMappings.Add(10, "GrossPayMonthly");//16
                    copy.ColumnMappings.Add(11, "CountryCode");//7
                    copy.ColumnMappings.Add(12, "DateOfBirth");//7
                    copy.ColumnMappings.Add(13, "Gender");//7
                    copy.ColumnMappings.Add(14, "MiddleName");//7
                    copy.ColumnMappings.Add(15, "BVN");//7
                    copy.ColumnMappings.Add(16, "BankAccountHolderName");//7
                    copy.DestinationTableName = "BulkUploadUserCSVError";
                    copy.WriteToServer(dt);
                }

                string query = @"usp_ImportUserMasterCSVError";
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@EmployerName", employerName);
                parameter.Add("@EmployerId", empId);

                await dbConnection.ExecuteAsync(query, parameter,
                                      commandType: CommandType.StoredProcedure);
                return 1;
            }
        }
        public async Task<int> BulkUploadUsersList1(DataTable dt, string employerName, long empId)
        {
            using (var dbConnection = Connection)
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(dbConnection.ConnectionString))
                {
                    copy.ColumnMappings.Add(0, "FirstName");//0
                    copy.ColumnMappings.Add(1, "LastName");//1
                    copy.ColumnMappings.Add(2, "MiddleName");//2
                    copy.ColumnMappings.Add(3, "NinNo");//3
                    copy.ColumnMappings.Add(4, "DateOfBirth");//4
                    copy.ColumnMappings.Add(5, "Email");//5
                    copy.ColumnMappings.Add(6, "Password");//6
                    copy.ColumnMappings.Add(7, "CountryCode");//7
                    copy.ColumnMappings.Add(8, "PhoneNumber");//8
                    copy.ColumnMappings.Add(9, "Gender");//9
                    copy.ColumnMappings.Add(10, "State");//10
                    copy.ColumnMappings.Add(11, "City");//11
                    copy.ColumnMappings.Add(12, "Address");//12
                    copy.ColumnMappings.Add(13, "PostalCode");//13
                    copy.ColumnMappings.Add(14, "StaffId");//14
                    copy.ColumnMappings.Add(15, "NetPayMonthly");//15
                    copy.ColumnMappings.Add(16, "GrossPayMonthly");//16
                    copy.ColumnMappings.Add(17, "CountryName");//17
                    copy.DestinationTableName = "UserMasterCSV";
                    copy.WriteToServer(dt);
                }

                string query = @"usp_ImportUserMasterCSV";
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@EmployerName", employerName);
                parameter.Add("@EmployerId", empId);

                await dbConnection.ExecuteAsync(query, parameter,
                                      commandType: CommandType.StoredProcedure);
                return 1;
            }
        }

        public async Task<List<BankDetail>> GetBankDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[BankName]
                                      ,[AccountNumber]
                                      ,[BVN]
                                      ,[BankAccountHolderName]
                                      ,[CustomerId]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                  FROM [dbo].[BankDetail]
                                  WHERE UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
            }
        }

        public async Task<int> DeleteBankByBankDetailId(BankDetail bankDetail, IDbConnection exdbConnection = null)
        {
            string query = @"
                           Update BankDetail set IsActive=0,IsDeleted=1,UpdatedAt=@UpdatedAt where Id=@Id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, bankDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, bankDetail));
            }
        }
    }
}
