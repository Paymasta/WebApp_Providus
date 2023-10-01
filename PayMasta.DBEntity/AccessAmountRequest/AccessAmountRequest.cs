using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.AccessAmountRequest
{
    public class AccessAmountRequest : BaseEntity
    {
        public long UserId { get; set; }
        public decimal AccessAmount { get; set; }
        public decimal AccessedPercentage { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime PayCycle { get; set; }
        public int Status { get; set; }
        public int AdminStatus { get; set; }
        public bool IsPaidToPayMasta { get; set; }
        public decimal CommissionCharge { get; set; }
        public decimal TotalAmountWithCommission { get; set; }
    }
}
