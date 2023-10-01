using PayMasta.DBEntity.Account;
using PayMasta.DBEntity.EmployerDetail;
using PayMasta.Repository.Employer.EmployerProfile;
using PayMasta.Utilities;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EmployerProfile
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly IEmployerProfileRepository _employerProfileRepository;

        public EmployerProfileService()
        {
            _employerProfileRepository = new EmployerProfileRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<GetEmployerProfile> GetEmployerProfileForUpdate(string guid)
        {
            var res = new GetEmployerProfile();
            var result = new GetEmployerProfileDetailResponse();
            try
            {
                var id = Guid.Parse(guid.ToString());
                result = await _employerProfileRepository.GetEmployerProfileForUpdate(id);
                if (result != null)
                {
                    if (result.Id > 0)
                    {
                        res.getEmployerProfileDetailResponse = result;
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.DATA_RECEIVED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<EmployerUpdateProfileResponse> UpdateEmployerProfile(UpdateEmployerRequest request)
        {
            var res = new EmployerUpdateProfileResponse();
            var employerResult = new EmployerDetail();
            var employerUserResult = new UserMaster();
            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                employerUserResult = await _employerProfileRepository.GetEmployerUserMasterDetail(id);
                employerResult = await _employerProfileRepository.GetEmployerDetail(employerUserResult.Id);
                if (employerResult.Id > 0 && employerUserResult.Id > 0)
                {
                    employerUserResult.FirstName = request.OrganisationName;
                    employerUserResult.Email = request.Email;
                    employerUserResult.CountryCode = request.CountryCode;
                    employerUserResult.PhoneNumber = request.PhoneNumber;
                    employerUserResult.State = request.State;
                    employerUserResult.Address = request.Address;
                    employerUserResult.PostalCode = request.PostalCode;
                    employerUserResult.UpdatedAt = DateTime.UtcNow;
                    employerUserResult.CountryName = request.Country;
                    employerUserResult.CountryGuid = request.CountryGuid;
                    employerUserResult.StateGuid=request.StateGuid;
                    employerResult.OrganisationName = request.OrganisationName;
                    employerResult.WorkingHoursOrDays = request.WorkingHoursOrDays;
                    employerResult.WorkingDaysInWeek = request.WorkingDaysInWeek;
                    employerResult.UpdatedAt = DateTime.UtcNow;
                    employerResult.UpdatedBy = employerUserResult.Id;
                   

                    using (var dbConnection = Connection)
                    {
                        //dbConnection.Open();
                        //using (var tran = dbConnection.BeginTransaction())
                        //{
                        try
                        {
                            var userRes = await _employerProfileRepository.UpdateEmployerUser(employerUserResult, dbConnection);
                            var userDetailRes = await _employerProfileRepository.UpdateDetail(employerResult, dbConnection);
                            if (userRes > 0 && userDetailRes > 0)
                            {
                                //tran.Commit();
                                //dbConnection.Close();
                                res.IsSuccess = true;
                                res.RstKey = 1;
                                res.Message = ResponseMessages.DATA_SAVED;

                            }
                            else
                            {
                                // dbConnection.BeginTransaction().Rollback();
                                res.IsSuccess = false;
                                res.RstKey = 2;
                                res.Message = ResponseMessages.DATA_NOT_SAVED;
                            }

                        }
                        catch (Exception ex)
                        {
                            //tran.Rollback();
                            // dbConnection.BeginTransaction().Rollback();
                        }
                        // }
                    }


                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
