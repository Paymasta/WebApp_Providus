using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.BouquetsVM
{
    public class BouquetsRequest
    {
        public string service { get; set; }
        public string type { get; set; }
    }

    public class AvailablePricingOption
    {
        public double monthsPaidFor { get; set; }
        public double price { get; set; }
        public double invoicePeriod { get; set; }
        public double totalAmount { get; set; }
    }

    public class Bouquet
    {
        public Bouquet()
        {
            availablePricingOptions = new List<AvailablePricingOption>();
        }
        public string code { get; set; }
        public string name { get; set; }
        public string amount { get; set; }
        public string primaryAmount { get; set; }
        public List<AvailablePricingOption> availablePricingOptions { get; set; }
    }

    public class Data
    {
        public Data()
        {
            bouquets = new List<Bouquet>()
;
        }
        public string responseCode { get; set; }
        public bool error { get; set; }
        public List<Bouquet> bouquets { get; set; }
    }

    public class BouquetsResponse
    {
        public BouquetsResponse()
        {
            data = new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }

    public class GetBouquetDataRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }

    }

    public class GetBouquestResponses
    {
        public GetBouquestResponses()
        {
            validateMeterResponse = new BouquetsResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public BouquetsResponse validateMeterResponse { get; set; }
    }

    
}
