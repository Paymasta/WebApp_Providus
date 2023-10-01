using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse()
        {
            // Permissions = new List<PermissionModel>();
            //  RelationData = new RelationDataVM();
        }
        [IgnoreDataMember]
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RstKey { get; set; }
        public string Token { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public bool IsProfileCompleted { get; set; }
        public int RoleId { get; set; }
        public bool IsBulkUpload { get; set; }
    }

    public class SignupResponse
    {
        public SignupResponse()
        {
            // Permissions = new List<PermissionModel>();
            //  RelationData = new RelationDataVM();
        }
        [IgnoreDataMember]
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }

        public string Email { get; set; }
        public int RstKey { get; set; }
        public string Token { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public bool IsProfileCompleted { get; set; }
    }


    public class UserModel
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string NINNumber { get; set; }
        public string Gender { get; set; }
        public double WalletBalance { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsKycVerified { get; set; }
    }
    public class RequestModel
    {
        public string Value { get; set; }
    }
    public class GetUserProfileRequest
    {
        public Guid UserGuid { get; set; }
    }
    public class SignUpRequest
    {
        public Guid? UserGuid { get; set; }
        //[Required]
        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string NinNo { get; set; }
        //public DateTime DateOfBirth { get; set; }
        // [Required]
        // public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string CountryCode { get; set; }
        [Phone(ErrorMessage = "Invalid PhoneNumber Address")]
        public string PhoneNumber { get; set; }
        //public string ProfileImage { get; set; }
        //public string State { get; set; }
        //public string City { get; set; }
        //public string Address { get; set; }
        //public string PostalCode { get; set; }
        //public string Gender { get; set; }
        //public string EmployerName { get; set; }
        //public long EmployerId { get; set; }
        //public string StaffId { get; set; }
        public bool IsCardChecked { get; set; }
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }

        [Required]
        public string OtpCode { get; set; }

        public int RoleId { get; set; }
    }

    public class ResendOTPRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Range(1, 2)]
        public int Type { get; set; }
    }

    public class VerifyOTPRequest
    {
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
        [Required]
        public string OtpCode { get; set; }
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }
    public class VerifyForgetPasswordOTPRequest
    {


        public string EmailorPhone { get; set; }
        [Required]
        public string OtpCode { get; set; }
        public int Type { get; set; }
    }
    public class LogoutRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string DeviceId { get; set; }
    }

    public class UpdateUserRequest
    {
        public UpdateUserRequest()
        {
            EmployerId = 0;
            NonRegisterEmployerGuid = null;
        }
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string ProfileImage { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string MiddleName { get; set; }
        [Required]

        public string NinNo { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

        public string PostalCode { get; set; }
        [Required]
        public string Gender { get; set; }

        public string EmployerName { get; set; }
        //  [Required]
        public long? EmployerId { get; set; }
        public string StaffId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string CustomerId { get; set; }
        public string CountryName { get; set; }
        public string BankCode { get; set; }
        public Guid? NonRegisterEmployerGuid { get; set; }
    }


    public class UpdateUserProfileRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Comment { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

    }
    public class UpdateEmployerRequest
    {
        [Required]
        public Guid UserGuid { get; set; }

        public bool IsEwaApprovalAccess { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PostalCode { get; set; }

        public string OrganisationName { get; set; }
        public int WorkingHoursOrDays { get; set; }
        public int WorkingDaysInWeek { get; set; }
        public string CountryCode { get; set; }
        public DateTime PayCycleFrom { get; set; }
        public DateTime PayCycleTo { get; set; }
        public string Email { get; set; }
        public Guid CountryGuid { get; set; }
        public Guid StateGuid { get; set; }
        public int EndDate { get; set; }
        public int StartDate { get; set; }
    }
    public class ChangeLanguageRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Range(1, 2)]
        public int LanguageId { get; set; }
    }

    public class ForgotPasswordRequest
    {
        [Required]
        public string EmailorPhone { get; set; }
        /// <summary>
        /// 1-Email,2-Phone
        /// </summary>
        [Range(1, 2)]
        public int Type { get; set; }
    }

    public class ResetPasswordRequest
    {


        public string EmailorPhone { get; set; }
        [Required]
        public string OtpCode { get; set; }
        [Required]
        public string Password { get; set; }

    }
    public class UpdateEmployerByUserGuidRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string StaffId { get; set; }
        [Required]
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
    }
    public class ChangePasswordRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class UploadProfileImageRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string ImageUrl { get; set; }

    }
    public class UploadProfileImageResponse
    {

        public int RstKey { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

    }
    public class AdminProfileResponse
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class GuestLoginRequest
    {
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }

    public class SelectViewModel
    {
        public Guid UserGuid { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
    }

    public class DriverLoginDetail
    {
        public Guid DriverGuid { get; set; }
        public bool InStoreDriver { get; set; }
    }

    public class NINNumberVerifyRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string dob { get; set; }
        public string NINNumber { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class VBNVerifyRequest
    {
        public string bvn { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }


    public class IsNINNumberVerifyRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        //public string phone { get; set; }
        public string dob { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class FieldMatches
    {
        public bool lastname { get; set; }
    }

    public class Data
    {
        public Data()
        {
            fieldMatches = new FieldMatches();
            residence = new Residence();
        }
        //public string nin { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string birthdate { get; set; }
        //public string phone { get; set; }
        //public string middlename { get; set; }
        //public string nationality { get; set; }
        //public string photo { get; set; }
        //public string gender { get; set; }
        //public FieldMatches fieldMatches { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string birthdate { get; set; }
        public string nationality { get; set; }
        public string photo { get; set; }
        public string nin { get; set; }
        public Residence residence { get; set; }
        public string trackingId { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }
    public class Residence
    {
        public string address1 { get; set; }
        public string lga { get; set; }
    }
    public class NinThirdPartyResponse
    {
        public NinThirdPartyResponse()
        {
            data = new PayMasta.ViewModel.Data();
        }
        public string status { get; set; }
        public PayMasta.ViewModel.Data data { get; set; }
    }
    public class OtpRequest
    {
        public OtpRequest()
        {
            this.MobileNo = string.Empty;
        }
        [Required]
        [Phone]
        public string MobileNo { get; set; }
        [Required]
        public string IsdCode { get; set; }
        [Required]
        public string Email { get; set; }
    }
    public class OtpResponse : OtpCommonRequest
    {
        public OtpResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        // public long OtpId { get; set; }


    }
    public class OtpCommonRequest
    {
        public OtpCommonRequest()
        {
            this.Otp = string.Empty;
        }
        public string Otp { get; set; }
    }

    public class UserExistanceRequest
    {
        public UserExistanceRequest()
        {

            this.MobileNo = string.Empty;
        }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class CountryResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
    }

    public class GetCountryResponse
    {
        public GetCountryResponse()
        {
            countryResponses = new List<CountryResponse>();
        }
        public bool Status { get; set; }
        public int RstKey { get; set; }

        public List<CountryResponse> countryResponses { get; set; }
    }

    public class StateResponse
    {
        public string Name { get; set; }
        public long CountryId { get; set; }

        public long Id { get; set; }
        public Guid Guid { get; set; }
    }

    public class GetSateResponse
    {
        public GetSateResponse()
        {
            stateResponses = new List<StateResponse>();
        }
        public bool Status { get; set; }
        public int RstKey { get; set; }

        public List<StateResponse> stateResponses { get; set; }
    }
    public class CityResponse
    {
        public string Name { get; set; }
        public long StateId { get; set; }

        public long Id { get; set; }
        public Guid Guid { get; set; }
    }

    public class GetCityResponse
    {
        public GetCityResponse()
        {
            cityResponses = new List<CityResponse>();
        }
        public bool Status { get; set; }
        public int RstKey { get; set; }

        public List<CityResponse> cityResponses { get; set; }
    }

    public class EmployeeUpdateProfileResponse
    {
        public EmployeeUpdateProfileResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        // public long OtpId { get; set; }


    }

    public class AddBankListRequest
    {
        public Guid UserGuid { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string CustomerId { get; set; }
        public string BankCode { get; set; }
        public string ImageUrl { get; set; }
    }
    public class AddBankListResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class GetBanksListResponse
    {
        public GetBanksListResponse()
        {
            getBankList = new List<GetBankList>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public List<GetBankList> getBankList { get; set; }
    }
    public class GetBankListRequest
    {
        public Guid UserGuid { get; set; }
    }
    public class GetBankList
    {
        public long UserId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string CustomerId { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DeleteBankRequest
    {
        public Guid UserGuid { get; set; }
        public long BankId { get; set; }
    }

    public class IsPasswordValidRequest
    {
        public Guid UserGuid { get; set; }
        public string Password { get; set; }
    }
    public class IsPasswordValidResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

    }
    public class NinResponse
    {
        public NinResponse()
        {
            ninThirdPartyResponse = new NinThirdPartyResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public NinThirdPartyResponse ninThirdPartyResponse { get; set; }

    }
    public class InviteRequest
    {
        public string IsdCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class PasscodeRequest
    {
        public Guid UserGuid { get; set; }
        public int Passcode { get; set; }
        public int OtpCode { get; set; }
    }

    public class PasscodeResponse
    {
        public PasscodeResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class D2CEmployerRequest
    {
        public Guid UserGuid { get; set; }
        public decimal NetSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public string EmployerName { get; set; }
        public string StaffId { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
    }

    public class D2CEmployerResponse
    {
        public D2CEmployerResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }


    }
}
