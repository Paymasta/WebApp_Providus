using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.EmployerVM
{
    public class GetEmployerProfileDetailResponse
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long UserId { get; set; }
        public string OrganisationName { get; set; }
        public int WorkingHoursOrDays { get; set; }
        public int WorkingDaysInWeek { get; set; }
        public int Status { get; set; }
        public DateTime PayCycleFrom { get; set; }
        public DateTime PayCycleTo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public Guid StateGuid { get; set; }
        public Guid CountryGuid { get; set; }
    }
    public class GetEmployerProfile
    {
        public GetEmployerProfile()
        {
            getEmployerProfileDetailResponse = new GetEmployerProfileDetailResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public GetEmployerProfileDetailResponse getEmployerProfileDetailResponse { get; set; }
    }
    public class EmployerUpdateProfileResponse
    {
        public EmployerUpdateProfileResponse()
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

}
