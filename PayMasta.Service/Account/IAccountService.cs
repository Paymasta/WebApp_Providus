using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Account
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<LoginResponse> AppLogin(LoginRequest request);
        Task<SignupResponse> SignUp(SignUpRequest request);
        Task<int> ResendOTP(ResendOTPRequest request);
        Task<LoginResponse> VerifyOTP(VerifyOTPRequest request);
        Task<int> Logout(LogoutRequest request);
        Task<UserModel> GetProfile(Guid userGuid);
        Task<EmployeeUpdateProfileResponse> UpdateProfile(UpdateUserRequest request);
        Task<string> ForgotPassword(ForgotPasswordRequest request);
        Task<OtpResponse> ResetPassword(ResetPasswordRequest request);
        Task<int> ChangePassword(ChangePasswordRequest request);
        ////Task<bool> UpdateEmailStatus(bool request);
        //Task<int> VerifyEmail(Guid userGuid);
        bool CheckInvalidUser(Guid guid);
        //Task<AdminProfileResponse> GetAdminProfile(Guid userGuid);
        //Task<LoginResponse> GuestLogin(GuestLoginRequest request, int roleId);
        //Task<int> ChangeLanguage(ChangeLanguageRequest request);
        //Task<List<UserModel>> GetEndUserForDropdown(int roleId);
        //Task<int> GetNotificationCount(Guid guid);
        Task<bool> CredentialsExistanceForMobileNumber(UserExistanceRequest request);
        Task<NinResponse> NinVerify(NINNumberVerifyRequest request);
        Task<OtpResponse> SendOTP(OtpRequest request);
        Task<int> GetNotificationCount(Guid guid);
        Task<GetCountryResponse> GetCountry();
        Task<GetSateResponse> GetState(Guid guid);
        Task<GetCityResponse> GetCityByStateGuid(Guid guid);
        Task<LoginResponse> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request);
        Task<LoginResponse> VerifyOTPWeb(VerifyOTPRequest request);
        Task<EmployeeUpdateProfileResponse> UpdateEmployerProfile(UpdateEmployerRequest request);
        Task<LoginResponse> VerifyForgetPasswordOTPWeb(VerifyForgetPasswordOTPRequest request);
        Task<AddBankListResponse> InsertBank(AddBankListRequest request);
        Task<bool> IsBankExists(Guid userGuid, string accountNumber);
        Task<GetBanksListResponse> GetBankListByUserGuid(Guid guid);
        Task<EmployeeUpdateProfileResponse> UpdateUserProfile(UpdateUserProfileRequest request);
        Task<bool> CredentialsExistanceForMobileNumberOrEmail(UserExistanceRequest request);
        Task<EmployeeUpdateProfileResponse> UpdateUserProfileRequest(UpdateUserProfileRequest request);
        Task<EmployeeUpdateProfileResponse> DeleteBankByBankDetailId(DeleteBankRequest request);
        Task<UploadProfileImageResponse> UploadProfileImage(UploadProfileImageRequest request);
        Task<IsPasswordValidResponse> IsPasswordValid(IsPasswordValidRequest request);
        Task<int> VerifyEmail(Guid userGuid);
        bool IsSessionValid(string deviceId);
        Task<OtpResponse> Invite(InviteRequest request);
        Task<EmployeeUpdateProfileResponse> UpdateEmployer(UpdateEmployerByUserGuidRequest request);
        Task<PasscodeResponse> GeneratePasscode(PasscodeRequest request);
        Task<PasscodeResponse> VerifyPasscode(PasscodeRequest request);
        Task<D2CEmployerResponse> CreateD2CEmployer(D2CEmployerRequest request);
    }
}
