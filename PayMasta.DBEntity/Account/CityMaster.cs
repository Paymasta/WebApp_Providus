using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.Account
{
    public class CityMaster : BaseEntity
    {
        public string Name { get; set; }
        public long StateId { get; set; }

    }
}
