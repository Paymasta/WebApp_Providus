using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.WidgetLinkMaster
{
    public class WidgetLinkMaster : BaseEntity
    {
        public long UserId { get; set; }
        public string RawContent { get; set; }
        public string WidgetLink { get; set; }

    }
}
