using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity
{
    public class Notification : BaseEntity
    {
        public long UserId { get; set; }
        public int TagId { get; set; }
        public long DataId { get; set; }
        public string NotificationText { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }

    }
}
