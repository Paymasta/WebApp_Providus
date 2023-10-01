using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.NotificationsVM
{
    public class NotificationsResponse
	{
		public long ReceiverId { get; set; }
		public long SenderId { get; set; }
		public string AlterMessage { get; set; }
		public string NotificationJson { get; set; }
		public int NotificationType { get; set; }
		public string DeviceToken { get; set; }
		public int DeviceType { get; set; }
		public bool IsRead { get; set; }
		public bool IsDelivered { get; set; }
		public long Id { get; set; }
		public Guid Guid { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
        public string NotificationTime { get; set; }
        public int TotalCount { get; set; }
    }
	public class NotificationsRequest
	{
		public Guid UserGuid { get; set; }
		
	}

	public class GetNotificationsResponse
	{
		public GetNotificationsResponse()
		{
			notificationsResponses = new List<NotificationsResponse>();
		}
		public List<NotificationsResponse> notificationsResponses { get; set; }
		public bool IsSuccess { get; set; }
		public int RstKey { get; set; }
		public string Message { get; set; }
        public int TotalCount { get; set; }
    }
	public class UpdateNotificationsResponse
	{
	
		public bool IsSuccess { get; set; }
		public int RstKey { get; set; }
		public string Message { get; set; }
	}

	public class EncryptedParamsRequestModel
	{
		public string EncryptedParams { get; set; }
	}
}
