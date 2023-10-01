using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.TokenRepository
{
    public class TokenRepository : ITokenRepository
    {
        private string connectionString;

        public TokenRepository()
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
        public UserSession GetUserSessionByToken(string token, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT TOP 1 [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[DeviceId]
                                      ,[DeviceType]
                                      ,[DeviceToken]
                                      ,[SessionKey]
                                      ,[SessionTimeout]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[JwtToken]
                                  FROM [dbo].[UserSession]
                                  WHERE JwtToken=@JwtToken AND IsActive=1 AND IsDeleted=0 ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<UserSession>(query, new { JwtToken = token })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<UserSession>(query, new { JwtToken = token })).FirstOrDefault();
            }
        }
    }
}
