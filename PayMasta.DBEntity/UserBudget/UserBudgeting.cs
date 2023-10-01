using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.UserBudget
{
    public class UserBudgeting : BaseEntity
    {
        public long UserId { get; set; }
        public long CategoryId { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal UsedPercentage { get; set; }
    }
}
