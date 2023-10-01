using Dapper;
using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.EmployerDetail;
using PayMasta.DBEntity.NonRegisterEmployerDetail;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.Utilities;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Employer.EmployeesVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private string connectionString;

        public AccountRepository()
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
        public async Task<UserMaster> Login(LoginRequest request)
        {
            using (var dbConnection = Connection)
            {
                string query = @"
                            SELECT TOP 1 [Id]
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
                                      ,[CountryGuid]
                                      ,[StateGuid]
                                      ,[NetPayMonthly]
                                      ,[GrossPayMonthly]
                                      ,[IsPayMastaCardApplied]
                                      ,[IsBulkUpload],[WalletPin]
                                  FROM [dbo].[UserMaster]
                                   WHERE (Email=@email OR PhoneNumber=@email) AND Password=@password                          
                                                            ORDER BY Id DESC";
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        email = request.Email,
                        password = request.Password,

                    })).FirstOrDefault();
            }
        }
        public async Task<UserMaster> CheckPassword(LoginRequest request)
        {
            using (var dbConnection = Connection)
            {
                string query = @"
                            SELECT TOP 1 [Id]
                                  ,[Guid]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[Email]
                                  ,[ProfileImage]
                                  ,[IsEmailVerified]
                                  ,[IsPhoneVerified]
                                  ,[IsVerified]
                                  ,[IsGuestUser]
                                  ,[IsActive],[Status]
                                  ,[IsDeleted],[CountryCode],[PhoneNumber],[IsProfileCompleted],[UserType],[Gender]
                            FROM [dbo].[UserMaster]
                            WHERE Password=@password                          
                            ORDER BY Id DESC
";
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        email = request.Email,
                        password = request.Password,

                    })).FirstOrDefault();
            }
        }
        public bool IsEmailExist(string email, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE LOWER(email)=LOWER(@email)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            email = email
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            email = email
                        })).FirstOrDefault() > 0;
            }
        }

        public bool IsStaffIdExist(string staffId, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE StaffId=@StaffId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            StaffId = staffId
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            StaffId = staffId
                        })).FirstOrDefault() > 0;
            }
        }
        public bool IsNinExist(string NIn, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE NinNo=@NinNo";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            NinNo = NIn
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            NinNo = NIn
                        })).FirstOrDefault() > 0;
            }
        }
        public bool IsUserExists(string email, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE (Email=@email OR PhoneNumber=@email) AND IsDeleted=0 AND Status=1";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            email = email
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            email = email
                        })).FirstOrDefault() > 0;
            }
        }
        public bool IsPhoneNumberExist(string phoneNumber, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE PhoneNumber=@phoneNumber AND IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            phoneNumber = phoneNumber
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            phoneNumber = phoneNumber
                        })).FirstOrDefault() > 0;
            }
        }
        public bool IsAccountNumberExist(string accountNumber, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[BankDetail]
                            WHERE AccountNumber=@AccountNumber AND IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            AccountNumber = accountNumber
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            AccountNumber = accountNumber
                        })).FirstOrDefault() > 0;
            }
        }

        public bool IsNINNumberExist(string ninNumber, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE NinNo=@NinNo AND IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            NinNo = ninNumber
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            NinNo = ninNumber
                        })).FirstOrDefault() > 0;
            }
        }

        public async Task<UserMaster> InsertUser(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[UserMaster]
                                   ([Email]
                                   ,[Password]
                                   ,[ProfileImage]
                                   ,[CountryCode]
                                   ,[PhoneNumber]     
                                   ,[IsEmailVerified]
                                   ,[IsPhoneVerified]
                                   ,[IsVerified]                                  
                                   ,[IsGuestUser]
                                   ,[WalletBalance]
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
                                   ,[GrossPayMonthly]
                                   ,[NetPayMonthly],[IsPayMastaCardApplied],[IsBulkUpload])
                             VALUES
                                   (@Email
                                   ,@Password
                                   ,@ProfileImage
                                   ,@CountryCode
                                   ,@PhoneNumber              
                                   ,@IsEmailVerified
                                   ,@IsPhoneVerified
                                   ,@IsVerified                                  
                                   ,@IsGuestUser
                                   ,@WalletBalance
                                   ,@Status                                 
                                   ,@IsActive
                                   ,@IsDeleted
                                   ,@CreatedAt
                                   ,@UpdatedAt
                                   ,@CreatedBy
                                   ,@UpdatedBy
                                   ,@UserType
                                   ,@IsvertualAccountCreated
                                   ,@IsProfileCompleted
                                   ,@GrossPayMonthly
                                   ,@NetPayMonthly,@IsPayMastaCardApplied,@IsBulkUpload);
                              SELECT * from UserMaster WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)
                                ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query, userEntity)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query, userEntity)).FirstOrDefault();
            }
        }

        public async Task<UserMaster> InsertUserFromNubanRegister(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[UserMaster]
                                                       ([FirstName]
                                                       ,[MiddleName]
                                                       ,[LastName]
                                                       ,[DateOfBirth]
                                                       ,[Email]
                                                       ,[Password]
                                                       ,[CountryCode]
                                                       ,[PhoneNumber]
                                                       ,[Gender]
                                                       ,[WalletBalance]
                                                       ,[IsEmailVerified]
                                                       ,[IsPhoneVerified]
                                                       ,[IsVerified]
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
                                                       ,[IsPayMastaCardApplied],[IsGuestUser],[IsverifiedByEmployer])
                                                 VALUES
                                                       (@FirstName
                                                       ,@MiddleName
                                                       ,@LastName
                                                       ,@DateOfBirth
                                                       ,@Email
                                                       ,@Password
                                                       ,@CountryCode
                                                       ,@PhoneNumber
                                                       ,@Gender
                                                       ,@WalletBalance
                                                       ,@IsEmailVerified
                                                       ,@IsPhoneVerified
                                                       ,@IsVerified
                                                       ,@Status
                                                       ,@IsActive
                                                       ,@IsDeleted
                                                       ,@CreatedAt
                                                       ,@UpdatedAt
                                                       ,@CreatedBy
                                                       ,@UpdatedBy
                                                       ,@UserType
                                                       ,@IsvertualAccountCreated
                                                       ,@IsProfileCompleted
                                                       ,@IsPayMastaCardApplied,@IsGuestUser,@IsverifiedByEmployer);
                              SELECT * from UserMaster WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)
                                ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query, userEntity)).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query, userEntity)).FirstOrDefault();
            }
        }
        public async Task<UserMaster> GetUserByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,ISNULL([FirstName],'') [FirstName]
								  ,ISNULL([MiddleName],'') [MiddleName]
                                  ,ISNULL([LastName],'')   [LastName]                          
                                  ,[Email]
								  ,ISNULL([NinNo],'')[NinNo]
	                              ,[DateOfBirth]
								  ,ISNULL([Gender] ,'')[Gender]
								  ,ISNULL([State] ,'')[State]
								  ,ISNULL([City],'')[City]
								  ,ISNULL([Address] ,'')[Address]
								  ,ISNULL([PostalCode],'')[PostalCode]
								  ,ISNULL([EmployerName] ,'')[EmployerName]
								  ,ISNULL([EmployerId] ,0)[EmployerId]
							   	  ,ISNULL([StaffId],'')[StaffId]
                                  ,[Password]
                                  ,ISNULL([ProfileImage],'')[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                                  ,[NetPayMonthly]
                                  ,[GrossPayMonthly],[IsEmployerRegister],[WalletPin],[IsKycVerified]
                                  ,[Passcode],[IsverifiedByEmployer]
                              FROM [dbo].[UserMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<GetEmployerDetailResponse> GetEmployerDetailById(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select  ed.Id
                                    ,um.FirstName
                                    ,um.LastName
                                    ,um.Address
                                    ,um.CountryCode
                                    ,um.PhoneNumber
                                    ,um.Gender
                                    ,um.UserType,um.Email
                                    ,ed.OrganisationName,ed.StartDate,ed.EndDate
                                    from EmployerDetail ed
                                    inner join UserMaster um on um.Id=ed.UserId
                                    where ed.Id=@id and um.IsActive=1 and um.IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            id = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetEmployerDetailResponse>(query,
                        new
                        {
                            id = userId
                        })).FirstOrDefault();
            }
        }

        public async Task<UserMaster> GetUserByGuidError(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,ISNULL([FirstName],'') [FirstName]
								  ,ISNULL([MiddleName],'') [MiddleName]
                                  ,ISNULL([LastName],'')   [LastName]                          
                                  ,[Email]
								  ,ISNULL([NinNo],'')[NinNo]
	                              ,[DateOfBirth]
								  ,ISNULL([Gender] ,'')[Gender]
								  ,ISNULL([State] ,'')[State]
								  ,ISNULL([City],'')[City]
								  ,ISNULL([Address] ,'')[Address]
								  ,ISNULL([PostalCode],'')[PostalCode]
								  ,ISNULL([EmployerName] ,'')[EmployerName]
								  ,ISNULL([EmployerId] ,0)[EmployerId]
							   	  ,ISNULL([StaffId],'')[StaffId]
                                  ,[Password]
                                  ,ISNULL([ProfileImage],'')[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                                  ,[NetPayMonthly]
                                  ,[GrossPayMonthly],[IsEmployerRegister],[WalletPin]
                              FROM [dbo].[UserMasterError]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }
        public async Task<string> GetVirtualAccountDetailByUserId(long UserId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT 
                                      [AccountNumber]
                                  FROM [dbo].[VirtualAccountDetail]
                                  WHERE UserId=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<string>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<string>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }

        public async Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId1(long UserId, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[VirtualAccountId]
                                      ,[ProfileID]
                                      ,[Pin]
                                      ,[deviceNotificationToken]
                                      ,[PhoneNumber]
                                      ,[Gender]
                                      ,[DateOfBirth]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[Address]
                                      ,[Bvn]
                                      ,[AccountName]
                                      ,[AccountNumber]
                                      ,[CurrentBalance]
                                      ,[JsonData]
                                      ,[UserId]
                                      ,[AuthToken]
                                      ,[AuthJson]
                                  FROM [dbo].[VirtualAccountDetail]
                                  WHERE UserId=@UserId;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<VirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<VirtualAccountDetail>(query,
                        new
                        {
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }
        public async Task<int> IsStaffIdExists(long employerId, string staffId, IDbConnection exdbConnection = null)
        {
            string query = @"select Id from UserMaster WHERE EmployerId=@EmployerId AND StaffId=@StaffId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<int>(query,
                        new
                        {
                            EmployerId = employerId,
                            StaffId = staffId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<int>(query,
                        new
                        {
                            EmployerId = employerId,
                            StaffId = staffId
                        })).FirstOrDefault();
            }
        }
        public async Task<UserMaster> GetUserById(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,ISNULL([FirstName],'') [FirstName]
								  ,ISNULL([MiddleName],'') [MiddleName]
                                  ,ISNULL([LastName],'')   [LastName]                          
                                  ,[Email]
								  ,ISNULL([NinNo],'')[NinNo]
	                              ,[DateOfBirth]
								  ,ISNULL([Gender] ,'')[Gender]
								  ,ISNULL([State] ,'')[State]
								  ,ISNULL([City],'')[City]
								  ,ISNULL([Address] ,'')[Address]
								  ,ISNULL([PostalCode],'')[PostalCode]
								  ,ISNULL([EmployerName] ,'')[EmployerName]
								  ,ISNULL([EmployerId] ,0)[EmployerId]
							   	  ,ISNULL([StaffId],'')[StaffId]
                                  ,[Password]
                                  ,ISNULL([ProfileImage],'')[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                                  ,[UpdatedBy],[UserType],[IsvertualAccountCreated],[IsProfileCompleted],[CountryName]
                                  ,[IslinkToOkra]
                              FROM [dbo].[UserMaster]
                            WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
            }
        }

        public UserMaster GetUserByIdForSession(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,ISNULL([FirstName],'') [FirstName]
								  ,ISNULL([MiddleName],'') [MiddleName]
                                  ,ISNULL([LastName],'')   [LastName]                          
                                  ,[Email]
								  ,ISNULL([NinNo],'')[NinNo]
	                              ,[DateOfBirth]
								  ,ISNULL([Gender] ,'')[Gender]
								  ,ISNULL([State] ,'')[State]
								  ,ISNULL([City],'')[City]
								  ,ISNULL([Address] ,'')[Address]
								  ,ISNULL([PostalCode],'')[PostalCode]
								  ,ISNULL([EmployerName] ,'')[EmployerName]
								  ,ISNULL([EmployerId] ,0)[EmployerId]
							   	  ,ISNULL([StaffId],'')[StaffId]
                                  ,[Password]
                                  ,ISNULL([ProfileImage],'')[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                                  ,[UpdatedBy],[UserType],[IsvertualAccountCreated],[IsProfileCompleted],[CountryName]
                                  ,[IslinkToOkra]
                              FROM [dbo].[UserMaster]
                            WHERE Id=@Id";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<UserMaster>(query,
                        new
                        {
                            Id = userId
                        })).FirstOrDefault();
            }
        }
        public async Task<NonRegisterEmployerDetail> GetNonRegisteredEmployerByEmailOrMobile(string email, string mobile, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                      ,[Guid]
                                      ,[OrganisationName]
                                      ,[OrganisationCode]
                                      ,[Status]
                                      ,[IsActive]
                                      ,[IsDeleted]
                                      ,[CreatedAt]
                                      ,[UpdatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedBy]
                                      ,[Email]
                                      ,[MobileNumber]
                                  FROM [dbo].[NonRegisterEmployerDetail]
                                  WHERE Email=@Email AND MobileNumber=@MobileNumber";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<NonRegisterEmployerDetail>(query,
                        new
                        {
                            Email = email,
                            MobileNumber = mobile
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<NonRegisterEmployerDetail>(query,
                        new
                        {
                            Email = email,
                            MobileNumber = mobile
                        })).FirstOrDefault();
            }
        }
        public async Task<int> UpdateUser(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [FirstName] = @FirstName                                  
                                  ,[LastName] = @LastName  
								  ,[MiddleName]  = @MiddleName 
								  ,[NinNo] =@NinNo
                                  ,[Email] = @Email
								  ,[DateOfBirth]=@DateOfBirth
                                  ,[Password] = @Password
                                  ,[ProfileImage] = @ProfileImage
                                  ,[CountryCode] = @CountryCode
                                  ,[PhoneNumber] = @PhoneNumber 
                                  ,[IsEmailVerified] = @IsEmailVerified
                                  ,[IsPhoneVerified] = @IsPhoneVerified
                                  ,[IsVerified] = @IsVerified                                 
                                  ,[IsGuestUser]=@IsGuestUser,[Status]=@status
                                  ,[IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
								  ,[Gender] = @Gender
								  ,[State] = @State
								  ,[City] = @City
								  ,[Address] = @Address
								  ,[PostalCode] = @PostalCode
								  ,[EmployerName] = @EmployerName
								  ,[EmployerId] = @EmployerId
                                  ,[IsEmployerRegister]=@IsEmployerRegister
								  ,[StaffId] = @StaffId
                                 ,[IsvertualAccountCreated]=@IsvertualAccountCreated
                                 ,[IsProfileCompleted]=@IsProfileCompleted
                                 ,[CountryGuid]=@CountryGuid
                                 ,[StateGuid]=@StateGuid
                                 ,[CountryName]=@CountryName
                                 ,[WalletPin]=@WalletPin
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
        public async Task<int> UpdateUserOtherDetail(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                           UPDATE [dbo].[UserMaster]
                                           SET
                                               [NinNo] = @NinNo
                                              ,[State] = @State
                                              ,[City] = @City
                                              ,[Address] = @Address
                                              ,[PostalCode] = @PostalCode
                                              ,[CountryName] = @CountryName
                                              ,[CountryGuid] = @CountryGuid
                                              ,[StateGuid] = @StateGuid
                                              ,[IsProfileCompleted]=@IsProfileCompleted
                                              ,[VerificationType]=@VerificationType
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
        public async Task<int> UpdateVirtualAccountStatus(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [IsvertualAccountCreated]=@IsvertualAccountCreated
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
        public async Task<int> UpdateVirtualAccountPin(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [WalletPin]=@WalletPin
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
        public async Task<int> UpdateUsersEmployer(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"UPDATE UserMaster SET 
                                    EmployerId=@EmployerId,
                                    EmployerName=@EmployerName,
                                    StaffId=@StaffId,
                                    IsEmployerRegister=@IsEmployerRegister,
                                    IsverifiedByEmployer=@IsverifiedByEmployer
                                    WHERE Id=@Id";
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
        public async Task<int> VerifyEmail(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [IsEmailVerified] = @IsEmailVerified
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
        public async Task<int> ChangeUserStatus(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET
                                   [IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
								  ,[Status]=@Status
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
        public async Task<int> ChangeUserStatusError(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMasterError]
                               SET
                                   [IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
								  ,[Status]=@Status
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
        public async Task<int> UpdateUserProfile(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET [FirstName] = @FirstName                                  
                                  ,[LastName] = @LastName  
								  ,[MiddleName]  = @MiddleName 
                                  ,[Email] = @Email
                                  ,[CountryCode] = @CountryCode
                                  ,[PhoneNumber] = @PhoneNumber 
                                  ,[IsEmailVerified] = @IsEmailVerified
                                  ,[IsPhoneVerified] = @IsPhoneVerified
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
								 
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
        public async Task<int> UpdateUserPassword(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [Password] = @Password,
                                  [IsBulkUpload]=@IsBulkUpload
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
        public async Task<int> InsertBankDetail(BankDetail bankEntity, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[BankDetail]
                                           ([UserId]
                                           ,[BankName]
                                           ,[AccountNumber]
                                           ,[BVN]
                                           ,[BankAccountHolderName]
                                           ,[CustomerId]
                                           ,[IsActive]
                                           ,[IsDeleted]
                                           ,[CreatedAt]
                                           ,[UpdatedAt],[BankCode])
                                     VALUES
                                           (@UserId
                                           ,@BankName
                                           ,@AccountNumber
                                           ,@BVN
                                           ,@BankAccountHolderName
                                           ,@CustomerId
                                           ,@IsActive
                                           ,@IsDeleted
                                           ,@CreatedAt
                                           ,@UpdatedAt,@BankCode)";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, bankEntity));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, bankEntity));
            }
        }

        public async Task<BankDetail> GetBankDetailByUserId(long userId, IDbConnection exdbConnection = null)
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
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            UserId = userId
                        })).FirstOrDefault();
            }
        }
        public async Task<UserMaster> GetUserByEmailOrPhone(int type, string emailorPhone)
        {
            string query = @"SELECT [Id],[Guid]
                                  ,[Email]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[CountryCode]
                                  ,[PhoneNumber],[Status]
                             FROM [dbo].[UserMaster]
                            WHERE IsActive=1 AND IsDeleted=0 AND ( Email=@emailorPhone OR  PhoneNumber=@emailorPhone)
                                 ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        type = type,
                        emailorPhone = emailorPhone
                    })).FirstOrDefault();
            }
        }
        public async Task<UserMaster> GetUserByEmailOrPhone(string emailorPhone)
        {
            string query = @"SELECT [Id],[Guid]
                                  ,[Email]
                                  ,[FirstName]
                                  ,[LastName]
                                  ,[CountryCode]
                                  ,[PhoneNumber],[Password]
                             FROM [dbo].[UserMaster]
                            WHERE IsActive=1 AND IsDeleted=0 AND (Email=@emailorPhone OR PhoneNumber=@emailorPhone)
                                 ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserMaster>(query,
                    new
                    {
                        emailorPhone = emailorPhone
                    })).FirstOrDefault();
            }
        }

        public async Task<int> InsertOtpInfo(OtpInfo otpInfoEntity)
        {
            using (var dbConnection = Connection)
            {
                string query = @"INSERT INTO [dbo].[OtpInfo]
                                   ([UserId]
                                   ,[CountryCode]
                                   ,[PhoneNumber]
                                   ,[Email]
                                   ,[OtpCode]
                                   ,[Type]
                                   ,[IsActive]
                                   ,[IsDeleted]
                                   ,[CreatedAt]
                                   ,[UpdatedAt]
                                   ,[CreatedBy]
                                   ,[UpdatedBy])
                             VALUES
                                   (@UserId
                                   ,@CountryCode
                                   ,@PhoneNumber
                                   ,@Email
                                   ,@OtpCode
                                   ,@type
                                   ,@IsActive
                                   ,@IsDeleted
                                   ,@CreatedAt
                                   ,@UpdatedAt
                                   ,@CreatedBy
                                   ,@UpdatedBy);   
                                    SELECT OtpCode from OtpInfo where UserId=@UserId
                                ";
                return (await dbConnection.ExecuteAsync(query, otpInfoEntity));
                //SELECT OtpCode from OtpInfo WHERE UserId=@UserId)
            }
        }

        public async Task<OtpInfo> GetOtpInfoByUserId(long userId)
        {
            string query = @"SELECT TOP 1 [Id]
                                  ,[OtpCode]
                              FROM [dbo].[OtpInfo]
                            WHERE UserId=@userId
                            ORDER BY Id DESC
                            ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<OtpInfo>(query,
                    new
                    {
                        userId = userId
                    })).FirstOrDefault();
            }

        }
        public async Task<OtpInfo> GetOtpInfoByUserId(string mobile, string otp)
        {
            string query = @"SELECT TOP 1 [Id]
                                  ,[OtpCode]
                              FROM [dbo].[OtpInfo]
                            WHERE PhoneNumber=@PhoneNumber and OtpCode=@OtpCode
                            ORDER BY Id DESC
                            ";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<OtpInfo>(query,
                    new
                    {
                        PhoneNumber = mobile,
                        OtpCode = otp
                    })).FirstOrDefault();
            }

        }
        public long GetUserIdByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<UserSession> GetSessionByDeviceId(long userId, string deviceId)
        {
            string query = @"SELECT [Id]
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
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE UserId=@userId AND DeviceId=@deviceId AND IsActive=1 AND IsDeleted=0";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        userId = @userId,
                        deviceId = deviceId
                    })).FirstOrDefault();
            }

        }
        public async Task<UserSession> GetSessionByDeviceId(string deviceId)
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
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE DeviceId=@deviceId AND IsActive=1 AND IsDeleted=0 ORDER BY Id Desc";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        deviceId = deviceId
                    })).FirstOrDefault();
            }

        }
        public async Task<int> UpdateSession(UserSession session)
        {
            using (var dbConnection = Connection)
            {
                string query = @" UPDATE [dbo].[UserSession]
                               SET [DeviceId] = @DeviceId
                                  ,[DeviceType] = @DeviceType
                                  ,[DeviceToken] = @DeviceToken
                                  ,[SessionKey] = @SessionKey
                                  ,[SessionTimeout] = @SessionTimeout
                                  ,[IsActive] = @IsActive
                                  ,[IsDeleted] = @IsDeleted
                                  ,[UpdatedAt] = @UpdatedAt
                                  ,[UpdatedBy] = @UpdatedBy
                                  ,[JwtToken]=@JwtToken
                             WHERE Id=@id;
                                ";
                return (await dbConnection.ExecuteAsync(query, session)); ;
            }
        }
        public async Task<int> DeleteSession(long userId)
        {
            using (var dbConnection = Connection)
            {
                string query = @"DELETE FROM UserSession WHERE UserId=@UserId;";
                return (await dbConnection.ExecuteAsync(query, new { UserId = userId })); ;
            }
        }

        public async Task<UserSession> GetSessionByUserId(long userId)
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
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE UserId=@userId  AND IsActive=1 AND IsDeleted=0 ORDER BY Id DESC";

            using (var dbConnection = Connection)
            {
                return (await dbConnection.QueryAsync<UserSession>(query,
                    new
                    {
                        userId = @userId,

                    })).FirstOrDefault();
            }

        }
        public async Task<int> CreateSession(UserSession userSessionEntity)
        {
            using (var dbConnection = Connection)
            {
                string query = @"INSERT INTO [dbo].[UserSession]
                                   ([UserId]
                                   ,[DeviceId]
                                   ,[DeviceType]
                                   ,[DeviceToken]
                                   ,[SessionKey]
                                   ,[SessionTimeout]
                                   ,[IsActive]
                                   ,[IsDeleted]
                                   ,[CreatedAt]
                                   ,[UpdatedAt],[JwtToken]
                                  )
                             VALUES
                                   (@UserId
                                   ,@DeviceId
                                   ,@DeviceType
                                   ,@DeviceToken
                                   ,@SessionKey
                                   ,@SessionTimeout
                                   ,@IsActive
                                   ,@IsDeleted
                                   ,@CreatedAt
                                   ,@UpdatedAt,@JwtToken
                                  )
                                ";
                return (await dbConnection.ExecuteAsync(query, userSessionEntity)); ;
            }
        }
        public UserMaster GetUserValidationInfo(Guid guid)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[IsEmailVerified]
                                  ,[IsPhoneVerified]
                                  ,[IsVerified]
                                  ,[IsActive]
                                  ,[IsDeleted]
                              FROM [dbo].[UserMaster]
                            WHERE Guid=@guid";
            using (var dbConnection = Connection)
            {
                return (dbConnection.Query<UserMaster>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }

        }
        public async Task<int> GetNotificationCount(long userId, IDbConnection dbConnection)
        {
            string query = @"
                            SELECT COUNT(Id)
                            FROM [dbo].[Notification] 
                            WHERE [UserId]=@userId AND IsActive=1 AND IsDeleted=0 AND [Status]=0
                            ";
            return (await dbConnection.QueryAsync<int>(query, new { UserId = userId })).FirstOrDefault();
        }

        public async Task<List<CountryResponse>> GetCountry(bool IsActive, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                              ,[Guid]
                              ,[Name]
                              ,[Code]
                              ,[ImageUrl]
                            
                          FROM [dbo].[CountryMaster] where IsActive=@IsActive";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CountryResponse>(query, new { IsActive = IsActive })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CountryResponse>(query, new { IsActive = IsActive })).ToList();
            }
        }

        public async Task<int> GetCountryIdByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                 
                              FROM [dbo].[CountryMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<int>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<int>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<List<StateResponse>> GetState(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[Name]
                                  ,[CountryId]
                              FROM [dbo].[StateMaster] where CountryId=@CountryId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<StateResponse>(query, new { CountryId = id })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<StateResponse>(query, new { CountryId = id })).ToList();
            }
        }

        public async Task<int> GetStateIdByGuid(Guid guid, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                 
                              FROM [dbo].[StateMaster]
                            WHERE Guid=@guid";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<int>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<int>(query,
                        new
                        {
                            guid = guid
                        })).FirstOrDefault();
            }
        }

        public async Task<List<CityResponse>> GetCity(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]
                                  ,[Name]
                                  ,[StateId]
                              FROM [dbo].[CityMaster] where StateId=@StateId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<CityResponse>(query, new { StateId = id })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<CityResponse>(query, new { StateId = id })).ToList();
            }
        }

        public async Task<int> UpdateUserWalletBalance(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET WalletBalance=@WalletBalance
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

        public async Task<UserMaster> GetUserByMobile(string mobile, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,[FirstName] 
								  ,[MiddleName]
                                  ,[LastName]                                
                                  ,[Email]
								  ,[NinNo]
	                              ,[DateOfBirth]
								  ,[Gender] 
								  ,[State] 
								  ,[City]
								  ,[Address] 
								  ,[PostalCode]
								  ,[EmployerName] 
								  ,[EmployerId] 
							   	  ,[StaffId] 
                                  ,[Password]
                                  ,[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                              FROM [dbo].[UserMaster]
                            WHERE PhoneNumber=@PhoneNumber";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            PhoneNumber = mobile
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            PhoneNumber = mobile
                        })).FirstOrDefault();
            }
        }

        public async Task<int> VerifyUserPhoneNumber(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [IsPhoneVerified] = @IsPhoneVerified
                                  
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

        public async Task<int> InsertEmployerDetail(EmployerDetail employerDetail, IDbConnection exdbConnection = null)
        {
            string query = @"INSERT INTO [dbo].[EmployerDetail]
                                               ([UserId]
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
                                                ,[StartDate],[EndDate],[IsEwaApprovalAccess])
                                         VALUES
                                               (@UserId
                                               ,@OrganisationName
                                               ,@WorkingHoursOrDays
                                               ,@WorkingDaysInWeek
                                               ,@Status
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@UpdatedAt
                                               ,@CreatedBy
                                               ,@UpdatedBy,@StartDate,@EndDate,@IsEwaApprovalAccess)
                                SELECT Id from EmployerDetail WHERE ID=CAST(SCOPE_IDENTITY() as BIGINT)";
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

        public async Task<int> InsertBank(BankDetail otpInfoEntity, IDbConnection exdbConnection = null)
        {
            using (var dbConnection = Connection)
            {
                string query = @"INSERT INTO [dbo].[BankDetail]
                                                   ([UserId]
                                                   ,[BankName]
                                                   ,[AccountNumber]
                                                   ,[BVN]
                                                   ,[BankAccountHolderName]
                                                   ,[CustomerId]
                                                   ,[IsActive]
                                                   ,[IsDeleted]
                                                   ,[CreatedAt]
                                                   ,[UpdatedAt]
                                                   ,[BankCode],[ImageUrl])
                                             VALUES
                                                   (@UserId
                                                   ,@BankName
                                                   ,@AccountNumber
                                                   ,@BVN
                                                   ,@BankAccountHolderName
                                                   ,@CustomerId
                                                   ,@IsActive
                                                   ,@IsDeleted
                                                   ,@CreatedAt
                                                   ,@UpdatedAt
                                                   ,@BankCode,@ImageUrl)";
                return (await dbConnection.ExecuteAsync(query, otpInfoEntity));
                //SELECT OtpCode from OtpInfo WHERE UserId=@UserId)
            }
        }

        public bool IsBankExists(string accountNumber, long userId, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                                  FROM [dbo].[BankDetail]
                                  WHERE AccountNumber=@AccountNumber and UserId=@UserId and IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            AccountNumber = accountNumber,
                            UserId = userId,
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            AccountNumber = accountNumber,
                            UserId = userId,
                        })).FirstOrDefault() > 0;
            }
        }

        public async Task<List<GetBankList>> GetBankListByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT   [Id]
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
                                      ,[BankCode],[ImageUrl]
                                  FROM [dbo].[BankDetail]
                                  where UserId=@UserId AND IsActive=1 And IsDeleted=0 ORDER by Id Desc";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<GetBankList>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<GetBankList>(query,
                        new
                        {
                            UserId = userId
                        })).ToList();
            }
        }

        public bool IsPhoneNumberOrEmailExist(string phoneNumber, string email, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                            FROM [dbo].[UserMaster]
                            WHERE (PhoneNumber=@phoneNumber OR Email=@Email) AND IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            phoneNumber = phoneNumber,
                            Email = email
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            phoneNumber = phoneNumber,
                            Email = email
                        })).FirstOrDefault() > 0;
            }
        }

        public async Task<int> UpdateUserProfileRequest(PayMasta.DBEntity.UpdateUserProfileRequest.UpdateUserProfileRequest userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            INSERT INTO [dbo].[UpdateUserProfileRequest]
                                               ([UserId]
                                               ,[FirstName]
                                               ,[LastName]
                                               ,[MiddleName]
                                               ,[Address]
                                               ,[CountryCode]
                                               ,[PhoneNumber]
                                               ,[Email]
                                               ,[IsActive]
                                               ,[IsDeleted]
                                               ,[CreatedAt]
                                               ,[CreatedBy],[Status],[Comment]
                                               )
                                         VALUES
                                               (@UserId
                                               ,@FirstName
                                               ,@LastName
                                               ,@MiddleName
                                               ,@Address
                                               ,@CountryCode
                                               ,@PhoneNumber
                                               ,@Email
                                               ,@IsActive
                                               ,@IsDeleted
                                               ,@CreatedAt
                                               ,@CreatedBy,@Status,@Comment
                                              )";
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

        public async Task<BankDetail> GetBankDetailById(long id, long UserId, IDbConnection exdbConnection = null)
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
                                      ,[BankCode]
                                      ,[ImageUrl]
                                  FROM [dbo].[BankDetail]
                                  WHERE Id=@Id AND UserId=@UserId";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            Id = id,
                            UserId = UserId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            Id = id,
                            UserId = UserId,
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UploadProfileImage(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [ProfileImage] = @ProfileImage
                                  
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

        public bool IsPasswordValid(Guid userGuid, string password, IDbConnection exdbConnection = null)
        {
            string query = @"
                            SELECT [Id]
                                FROM [dbo].[UserMaster] where Guid=@Guid and Password=@Password and IsActive=1 and IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (dbConnection.Query<long>(query,
                        new
                        {
                            Guid = userGuid,
                            Password = password,
                        })).FirstOrDefault() > 0;
                }
            }
            else
            {
                return (exdbConnection.Query<long>(query,
                        new
                        {
                            Guid = userGuid,
                            Password = password,
                        })).FirstOrDefault() > 0;
            }
        }

        public async Task<List<UserMaster>> GetEmployeesByNonRegisterEmployerId(long id, IDbConnection exdbConnection = null)
        {
            string query = @"SELECT [Id]
                                  ,[Guid]                                  
                                  ,[FirstName] 
								  ,[MiddleName]
                                  ,[LastName]                                
                                  ,[Email]
								  ,[NinNo]
	                              ,[DateOfBirth]
								  ,[Gender] 
								  ,[State] 
								  ,[City]
								  ,[Address] 
								  ,[PostalCode]
								  ,[EmployerName] 
								  ,[EmployerId] 
							   	  ,[StaffId] 
                                  ,[Password]
                                  ,[ProfileImage]
                                  ,[CountryCode]
                                  ,[PhoneNumber]                                                           
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
                              FROM [dbo].[UserMaster]
                            WHERE EmployerId=@EmployerId AND IsEmployerRegister=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            EmployerId = id
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<UserMaster>(query,
                        new
                        {
                            EmployerId = id
                        })).ToList();
            }
        }

        public async Task<int> BulkUpdateEmployeesEmployer(List<UserMaster> userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET EmployerId=@EmployerId,
							   EmployerName=@EmployerName
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

        public async Task<BankDetail> GetBankByBankCode(string bankCode, long userid, IDbConnection exdbConnection = null)
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
                                      ,[BankCode]
                                      ,[ImageUrl]
                                  FROM [dbo].[BankDetail]
                                  WHERE BankCode=@BankCode AND UserId=@UserId and IsActive=1 and IsDeleted=0";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            BankCode = bankCode,
                            UserId = userid,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            BankCode = bankCode,
                            UserId = userid,
                        })).FirstOrDefault();
            }
        }

        public UserSession GetSessionByDeviceId1(string deviceId)
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
                                  ,[UpdatedBy],[JwtToken]
                              FROM [dbo].[UserSession]
                            WHERE DeviceId=@deviceId AND IsActive=1 AND IsDeleted=0 ORDER BY Id Desc";

            using (var dbConnection = Connection)
            {
                return (dbConnection.Query<UserSession>(query,
                    new
                    {
                        deviceId = deviceId
                    })).FirstOrDefault();
            }

        }

        public async Task<BankDetail> IsAccountNumberExists(string accountNumber, IDbConnection exdbConnection = null)
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
                                      ,[BankCode]
                                      ,[ImageUrl]
                                  FROM [dbo].[BankDetail]
                                  WHERE AccountNumber=@AccountNumber AND IsActive=1";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            AccountNumber = accountNumber
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<BankDetail>(query,
                        new
                        {
                            AccountNumber = accountNumber
                        })).FirstOrDefault();
            }
        }

        public async Task<int> UpdatePasscode(long userId, int passcode, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                  [Passcode] = @Passcode
                             WHERE Id=@id
                            ";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.ExecuteAsync(query, new { Id = userId, Passcode = passcode }));
                }
            }
            else
            {
                return (await exdbConnection.ExecuteAsync(query, new { Id = userId, Passcode = passcode }));
            }
        }

        public async Task<int> CreateD2CEmployer(UserMaster userEntity, IDbConnection exdbConnection = null)
        {
            string query = @"
                            UPDATE [dbo].[UserMaster]
                               SET 
                                 [EmployerId] = @EmployerId
                                ,[EmployerName] = @EmployerName
                                ,[StaffId] = @StaffId
                                ,[NetPayMonthly] = @NetPayMonthly
                                ,[GrossPayMonthly] = @GrossPayMonthly
                                ,[IsD2CUser] = @IsD2CUser
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
