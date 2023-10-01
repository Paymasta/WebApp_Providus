using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.CommisionMaster
{
    public class CommisionMaster : BaseEntity
    {
        public int AmountFrom { get; set; }
        public int AmountTo { get; set; }
        public decimal CommisionPercent { get; set; }
        public decimal FlatCharges { get; set; }
        public decimal BenchmarkCharges { get; set; }
    }
}
