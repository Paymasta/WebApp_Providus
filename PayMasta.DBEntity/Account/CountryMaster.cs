using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.Account
{
    public class CountryMaster : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }

    }
}
