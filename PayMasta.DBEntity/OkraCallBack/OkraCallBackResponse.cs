using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.OkraCallBack
{
    public class OkraCallBackResponse : BaseEntity
    {
        public long UserId { get; set; }
        public string CustomerId { get; set; }
        public string CallBackType { get; set; }
        public string CallBackUrl { get; set; }
        public string RawContent { get; set; }
        public string WidgetLink { get; set; }
        public string BankCodeOrBankId { get; set; }
    }
}
