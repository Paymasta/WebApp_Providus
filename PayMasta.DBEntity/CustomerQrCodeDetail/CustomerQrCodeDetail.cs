using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.CustomerQrCodeDetail
{
    public class CustomerQrCodeDetail : BaseEntity
    {
        public long UserId { get; set; }
        public string ImageUrl { get; set; }
    }
}
