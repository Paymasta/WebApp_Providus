using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.SMSUtils
{
    public class SMSModel
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
    public class SendMessageRequest
    {
        public string MobileNo { get; set; }
        public string ISD { get; set; }
        public string Message { get; set; }
    }
    public class RouteMobileMessageRequest
    {
        public RouteMobileMessageRequest()
        {
            this.source = ConfigurationManager.AppSettings["RouteSource"];
            this.destination = string.Empty;
            this.message = string.Empty;
            this.username = ConfigurationManager.AppSettings["RouteUsername"];
            this.password = ConfigurationManager.AppSettings["RoutePassword"];
            this.type = "0";
            this.dlr = "1";
        }
        public string source { get; set; }
        public string destination { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; }
        public string dlr { get; set; }

    }
    public class HubtelMessageResponse
    {
        public int Status { get; set; }
        public string MessageId { get; set; }
        public decimal Rate { get; set; }
        public string NetworkId { get; set; }
    }

}
