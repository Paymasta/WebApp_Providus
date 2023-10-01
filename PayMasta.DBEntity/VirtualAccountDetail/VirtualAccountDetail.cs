using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.VirtualAccountDetail
{
  //  public class VirtualAccountDetail:BaseEntity
  //  {
  //      public string VirtualAccountId { get; set; }
		//public string BankName { get; set; }
		//public string AccountReference { get; set; }
		//public string Nuban { get; set; }
		//public string AccountName { get; set; }
		//public string BarterId { get; set; }
		//public string BankCode { get; set; }
		//public string email { get; set; }
		//public string MobileNumber { get; set; }
		//public string Country { get; set; }
		//public string Status { get; set; }
		//public long UserId { get; set; }
  //  }


	public class VirtualAccountDetail : BaseEntity
	{
		public string VirtualAccountId { get; set; }
		public string ProfileID { get; set; }
		public string Pin { get; set; }
		public string deviceNotificationToken { get; set; }
		public string PhoneNumber { get; set; }
		public string Gender { get; set; }
		public string DateOfBirth { get; set; }
		public string Address { get; set; }
		public string Bvn { get; set; }
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }
		public string CurrentBalance { get; set; }
		public string JsonData { get; set; }
        public long UserId { get; set; }
		public string AuthToken { get; set; }
		public string AuthJson { get; set; }

		
	}
}
