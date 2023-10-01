using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.RandomInvoiceNumber
{
    public class RandomInvoiceNumber : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public string Pattern { get; set; }

    }
}
