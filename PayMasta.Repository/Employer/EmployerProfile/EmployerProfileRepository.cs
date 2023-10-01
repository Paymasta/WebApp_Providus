using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.EmployerDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PayMasta.Repository.Employer.EmployerProfile
{
    public class EmployerProfileRepository : IEmployerProfileRepository
    {
        private string connectionString;

        public EmployerProfileRepository()
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

        public async Task<GetEmployerProfileDetailResponse> GetEmployerProfileForUpdate(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT ed.[Id]
                                      ,ed.[Guid]
                                      ,ed.[UserId]
                                      ,ed.[OrganisationName]
                                      ,ed.[WorkingHoursOrDays]
                                      ,ed.[WorkingDaysInWeek]
                                      ,ed.[Status]
                                      ,ed.[PayCycleFrom]
                                      ,ed.[PayCycleTo]
	                                  ,um.Email
	                                  ,um.PhoneNumber
	                                  ,um.Address
	                                  ,um.PostalCode
	                                  ,um.CountryCode
									  ,ISNULL(um.CountryName,'')CountryName
									  ,ISNULL(um.State,'')StateName
                                      ,um.CountryGuid
									  ,um.StateGuid
                                  FROM [dbo].[EmployerDetail] ed
                                  INNER JOIN UserMaster  um on um.Id=ed.UserId
                                  WHERE um.Guid=@Guid AND um.IsActive=1 AND um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetEmployerProfileDetailResponse>(query, new { Guid = guid })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetEmployerProfileDetailResponse>(query, new { Guid = guid })).FirstOrDefault();
            }
        }

        public async Task<EmployerDetail> GetEmployerDetail(long employerId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[UserId]
                                      ,[OrganisationName]
                                      ,[WorkingHoursOrDays]
                                      ,[WorkingDaysInWeek]
                                      ,[Status]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[PayCycleFrom]
                                      ,[PayCycleTo]
                                      ,[NonRegisterEmployerDetailId]
                                  FROM [dbo].[EmployerDetail]
                                  WHERE UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployerDetail>(query, new { UserId = employerId })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployerDetail>(query, new { UserId = employerId })).FirstOrDefault();
            }
        }

        public async Task<UserMaster> GetEmployerUserMasterDetail(Guid employerId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[FirstName]
                                      ,[MiddleName]
                                      ,[LastName]
                                      ,[NinNo]
                                      ,[DateOfBirth]
                                      ,[Email]
                                      ,[Password]
                                      ,[ProfileImage]
                                      ,[CountryCode]
                                      ,[PhoneNumber]
                                      ,[Gender]
                                      ,[State]
                                      ,[City]
                                      ,[Address]
                                      ,[PostalCode]
                                      ,[EmployerName]
                                      ,[EmployerId]
                                      ,[StaffId]
                                      ,[WalletBalance]
                                      ,[IsEmailVerified]
                                      ,[IsPhoneVerified]
                                      ,[IsVerified]
                                      ,[IsGuestUser]
                                      ,[Status]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[UserType]
                                      ,[IsvertualAccountCreated]
                                      ,[IsProfileCompleted]
                                      ,[CountryName]
                                      ,[IsEmployerRegister]
                                      ,[IslinkToOkra]
                                  FROM [dbo].[UserMaster]
                                  WHERE Guid=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query, new { Id = employerId })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query, new { Id = employerId })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateEmployerUser(UserMaster userEntity, IDbConnection exdbConnection = null)
        {

            string query = @"
                               UPDATE [dbo].[UserMaster]
                               SET [FirstName] = @FirstName
                                  ,[Email] = @Email
                                  ,[CountryCode] = @CountryCode
                                  ,[PhoneNumber] = @PhoneNumber
                                  ,[State] = @State
                                  ,[CountryGuid] = @CountryGuid
                                  ,[StateGuid] = @StateGuid
                                  ,[CountryName] = @CountryName
                                  ,[Address] = @Address
                                  ,[PostalCode] = @PostalCode
                                  ,[UpdatedAt] = @UpdatedAt
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

                //  exdbConnection.exc
                return (await exdbConnection.ExecuteAsync(query, userEntity, commandType: CommandType.Text));
            }
        }
        public async Task<int> UpdateDetail(EmployerDetail employerDetail, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[EmployerDetail]
                           SET [OrganisationName] = @OrganisationName
                              ,[WorkingHoursOrDays] = @WorkingHoursOrDays
                              ,[WorkingDaysInWeek] = @WorkingDaysInWeek
                              ,[UpdatedAt] = @UpdatedAt
                              ,[UpdatedBy] = @UpdatedBy
                         WHERE Id=@id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, employerDetail));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, employerDetail));
            }
        }
    }
}
