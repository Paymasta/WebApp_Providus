using PayMasta.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity
{
    public class ArticleUserAnswer : BaseEntity
    {
        public long UserId { get; set; }
        public long ArticleId { get; set; }
        public int AnswerOption { get; set; }
        public double PriceMoney { get; set; }
    }
}
