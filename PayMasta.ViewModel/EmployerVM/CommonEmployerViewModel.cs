using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.EmployerVM
{
    public class GetEmployerResponse
    {
        public GetEmployerResponse()
        {
            employerResponses = new List<EmployerResponse>();
        }
        public bool Status { get; set; }
        public int RstKey { get; set; }

        public List<EmployerResponse> employerResponses { get; set; }
    }
    public class EmployerResponse
    {
        public long UserId { get; set; }
        public string OrganisationName { get; set; }
        public int WorkingHoursOrDays { get; set; }
        public int WorkingDaysInWeek { get; set; }
        public int Status { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }


    public class NonRegisterEmployerResponse
    {
        public string OrganisationName { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }

        public string OrganisationCode { get; set; }

    }

    public class GetNonRegisterEmployerResponse
    {
        public GetNonRegisterEmployerResponse()
        {
            nonRegisterEmployerResponses = new List<NonRegisterEmployerResponse>();
        }
        public bool Status { get; set; }
        public int RstKey { get; set; }

        public List<NonRegisterEmployerResponse> nonRegisterEmployerResponses { get; set; }
    }
}
