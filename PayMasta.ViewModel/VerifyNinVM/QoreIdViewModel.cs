using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VerifyNinVM
{
    #region QoreIdAuthRequest
    public class QoreIdAuthRequest
    {
        public string clientId { get; set; }
        public string secret { get; set; }
    }
    public class QoreIdAuthResponse
    {
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string tokenType { get; set; }
    }
    #endregion QoreIdAuthRequest

    #region QoreIdBvnNubanRequest
    public class QoreIdBvnNubanRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string accountNumber { get; set; }
        public string bankCode { get; set; }
    }

    public class RegisterByNubanRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string BankName { get; set; }
        public string CountryCode { get; set; }
        public bool IsCardChecked { get; set; }
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string OtpCode { get; set; }
    }
  

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string accountNumber { get; set; }
        public string bankCode { get; set; }
    }

    public class BvnNuban
    {
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string bvn { get; set; }
        public string photo { get; set; }
    }

    public class BvnNubanCheck
    {
        public BvnNubanCheck()
        {
            fieldMatches = new FieldMatches();
        }
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class QoreIdBvnNubanResponse
    {
        public QoreIdBvnNubanResponse()
        {
            applicant = new Applicant();
            summary = new Summary();
            status = new Status();
            bvn_nuban = new BvnNuban();
        }
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public BvnNuban bvn_nuban { get; set; }

       // public int status { get; set; }
        //public int statusCode { get; set; }
        //public string message { get; set; }
        //public string error { get; set; }
    }

    public class errorQoreId
    {
        public int status { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public string error { get; set; }
    }

    public class Status
    {
        public string state { get; set; }
        public string status { get; set; }
    }

    public class Summary
    {
        public Summary()
        {
            bvn_nuban_check = new BvnNubanCheck();
        }
        public BvnNubanCheck bvn_nuban_check { get; set; }
    }
    public class QoreIdBvnNubanResult
    {
        public QoreIdBvnNubanResult()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
            // vninVerifyResponses = new QoreIdBvnNubanResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public long UserId { get; set; }
        public Guid UserGuid { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public bool IsProfileCompleted { get; set; }
        // public QoreIdBvnNubanResponse vninVerifyResponses { get; set; }
    }
    #endregion QoreIdBvnNubanRequest

    #region QoreIdVninRequest
    public class QoreIdVninRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant2
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class FieldMatches2
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class QoreIdVninResponse
    {
        public QoreIdVninResponse()
        {
            applicant = new Applicant2();
            summary = new Summary2();
            status = new Status2();
            v_nin = new VNin();
        }
        public int id { get; set; }
        public Applicant2 applicant { get; set; }
        public Summary2 summary { get; set; }
        public Status2 status { get; set; }
        public VNin v_nin { get; set; }
    }

    public class Status2
    {
        public string state { get; set; }
        public string status { get; set; }
    }

    public class Summary2
    {
        public Summary2()
        {
            v_nin_check = new VNinCheck();
        }
        public VNinCheck v_nin_check { get; set; }
    }

    public class VNin
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string vNin { get; set; }
        public string photo { get; set; }
    }

    public class VNinCheck
    {
        public VNinCheck()
        {
            fieldMatches = new FieldMatches2();
        }
        public string status { get; set; }
        public FieldMatches2 fieldMatches { get; set; }
    }


    public class RegisterByVNinRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string BankName { get; set; }
        public string CountryCode { get; set; }
        public bool IsCardChecked { get; set; }
        [Required]
        public int DeviceType { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string Vnin { get; set; }
    }

    #endregion QoreIdVninRequest

    #region Bank list
    public class Datum
    {
        public string bankCode { get; set; }
        public string bankAccronym { get; set; }
        public string type { get; set; }
        public string bankName { get; set; }
        public string shortCode { get; set; }
        public string QoreIdBankCode { get; set; }
    }

    public class Metadata
    {
        public int size { get; set; }
    }

    public class BankListResponse
    {
        public BankListResponse()
        {
            data = new List<Datum>();
            metadata = new Metadata();
        }
        public string code { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public Metadata metadata { get; set; }
    }

    public class BankListResult
    {
        public BankListResult()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
            bankListResponse = new BankListResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public BankListResponse bankListResponse { get; set; }
    }
    #endregion

    public class IsOtpVerifiedResponse
    {
        public IsOtpVerifiedResponse()
        {
            // Permissions = new List<PermissionModel>();
            //  RelationData = new RelationDataVM();
        }

        public int RstKey { get; set; }
        public bool IsPhoneVerified { get; set; }
    }

    #region Add other detail
    public class AddOtherDetailPRequest
    {
        public Guid UserGuid { get; set; }
        // [Required]
        //[Phone]
        //public string NinNo { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string Countryname { get; set; }
        public Guid CountryGuid { get; set; }
        public Guid StateGuid { get; set; }
        public string PostalCode { get; set; }
        public string UserVerificationType { get; set; }
        public string VerificationId { get; set; }
    }
    public class AddOtherDetailResponse
    {
        public AddOtherDetailResponse()
        {
        }

        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    #endregion

}
