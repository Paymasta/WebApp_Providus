using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.ServiceCategory
{
    public class ServiceCategory : BaseEntity
    {
        public int ServiceCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
