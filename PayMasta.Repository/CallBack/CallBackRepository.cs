using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.OkraCallBack;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.CallBack
{
    public class CallBackRepository : ICallBackRepository
    {
        private string connectionString;

        public CallBackRepository()
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
        public async Task<int> InsertOkraCallBackDetail(OkraCallBackResponse okraCallBackResponse, IDbConnection exdbConnection = null)
        {

            string query = @"usp_InsertOkraCallBackResponse";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@UserId", okraCallBackResponse.UserId, DbType.Int64, ParameterDirection.Input);
            parameter.Add("@CustomerId", okraCallBackResponse.CustomerId, DbType.String, ParameterDirection.Input);
            parameter.Add("@CallBackType", okraCallBackResponse.CallBackType, DbType.String, ParameterDirection.Input);
            parameter.Add("@CallBackUrl", okraCallBackResponse.CallBackUrl, DbType.String, ParameterDirection.Input);
            parameter.Add("@RawContent", okraCallBackResponse.RawContent, DbType.String, ParameterDirection.Input);
            parameter.Add("@IsActive", okraCallBackResponse.IsActive, DbType.Boolean, ParameterDirection.Input);
            parameter.Add("@IsDeleted", okraCallBackResponse.IsDeleted, DbType.Boolean, ParameterDirection.Input);
            parameter.Add("@CreatedAt", okraCallBackResponse.CreatedAt, DbType.DateTime, ParameterDirection.Input);
            parameter.Add("@UpdatedAt", okraCallBackResponse.UpdatedAt, DbType.DateTime, ParameterDirection.Input);
            parameter.Add("@BankId", okraCallBackResponse.BankCodeOrBankId, DbType.String, ParameterDirection.Input);
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

        public async Task<BankDetail> GetBankDetailByCustomerId(string customerid, IDbConnection exdbConnection = null)
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
                                  WHERE CustomerId=@CustomerId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            CustomerId = customerid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            CustomerId = customerid
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdateOkraLinkStatus(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [IslinkToOkra] = @IslinkToOkra                                  
                                  
                             WHERE Id=@id
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
    }
}
