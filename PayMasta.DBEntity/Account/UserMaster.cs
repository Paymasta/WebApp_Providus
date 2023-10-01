using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.Account
{
    public class UserMaster : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string NinNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double WalletBalance { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsVerified { get; set; }
        public bool IsGuestUser { get; set; }
        public int Status { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Gender { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string StaffId { get; set; }
        public int UserType { get; set; }
        public bool IsvertualAccountCreated { get; set; }
        public bool IsProfileCompleted { get; set; }
        public bool IsEmployerRegister { get; set; }
        public bool IslinkToOkra { get; set; }
        public Guid CountryGuid { get; set; }
        public Guid StateGuid { get; set; }
        public decimal NetPayMonthly { get; set; }
        public decimal GrossPayMonthly { get; set; }
        public bool IsPayMastaCardApplied { get; set; }
        public bool IsBulkUpload { get; set; }
        public bool IsKycVerified { get; set; }
        public int WalletPin { get; set; }
        public string VerificationType { get; set; }
        public int Passcode { get; set; }
        public bool IsD2CUser { get; set; }

        public bool IsverifiedByEmployer { get; set; }
    }
}
