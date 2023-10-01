using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ValidateStartimesVM
{
    public class ValidatStarTimesRequest
    {
        public string service { get; set; }
        public string channel { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public string amount { get; set; }
        public string smartCardCode { get; set; }
    }
    public class Bouquet
    {
        public Bouquet()
        {
            cycles = new Cycles();
        }
        public string name { get; set; }
        public Cycles cycles { get; set; }
    }

    public class Cycles
    {
        public double daily { get; set; }
        public double weekly { get; set; }
        public double monthly { get; set; }
    }

    public class Data
    {
        public Data()
        {
            bouquets = new List<Bouquet>();
        }
        public double status { get; set; }
        public bool error { get; set; }
        public string smartCardCode { get; set; }
        public string balance { get; set; }
        public string name { get; set; }
        public string bouquet { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string responseCode { get; set; }
        public List<Bouquet> bouquets { get; set; }
        public string productCode { get; set; }
    }

    public class ValidatStarTimesResponse
    {
        public ValidatStarTimesResponse()
        {
            data = new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }

        public class GetValidateStarTimesResponses
        {
            public GetValidateStarTimesResponses()
            {
                validateMeterResponse = new ValidatStarTimesResponse();
            }
            public int RstKey { get; set; }
            public bool IsSuccess { get; set; }
            public ValidatStarTimesResponse validateMeterResponse { get; set; }
        }

    public class GetValidatStarTimesDataRequest
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
