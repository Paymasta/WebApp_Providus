using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.Notifications
{
    public class Notifications:BaseEntity
    {
        public long ReceiverId { get; set; }
		public long SenderId { get; set; }
		public long AlterMessage { get; set; }
		public long NotificationJson { get; set; }
		public long NotificationType { get; set; }
		public long DeviceToken { get; set; }
		public long DeviceType { get; set; }
		public long IsRead { get; set; }
		public long IsDelivered { get; set; }
    }
}
