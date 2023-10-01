using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.Account
{
    public class StateMaster : BaseEntity
    {
        public string Name { get; set; }
        public long CountryId { get; set; }
    }
}
