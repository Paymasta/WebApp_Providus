using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.EmployerDetail
{
    public class EmployerDetail : BaseEntity
    {
        public long UserId { get; set; }
        public string OrganisationName { get; set; }
        public int WorkingHoursOrDays { get; set; }
        public int WorkingDaysInWeek { get; set; }
        public int Status { get; set; }
        public DateTime PayCycleFrom { get; set; }
        public DateTime PayCycleTo { get; set; }
        public long NonRegisterEmployerDetailId { get; set; }
        public bool IsEwaApprovalAccess { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
    }
}
