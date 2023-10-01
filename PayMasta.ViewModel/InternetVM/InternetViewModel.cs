using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.InternetVM
{
    public class InternetBundlesRequest
    {
        public string service { get; set; }
        public string channel { get; set; }
        public string type { get; set; }
        public string account { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Bundle
    {
        public string name { get; set; }
        public string code { get; set; }
        public double displayPrice { get; set; }
        public double price { get; set; }
        public double fee { get; set; }
        public string validity { get; set; }
    }

    public class Data
    {
        public bool error { get; set; }
        public string message { get; set; }
        public List<Bundle> bundles { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
    }

    public class InternetBundlesResponse
    {
        public InternetBundlesResponse()
        {
            data = new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }

    public class GetInternetBundlesResponse
    {
        public GetInternetBundlesResponse()
        {
            internetBundlesResponse = new InternetBundlesResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        //public string productCode { get; set; }
        public InternetBundlesResponse internetBundlesResponse { get; set; }
    }

    public class GetInternetBundlesRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
    }
}
