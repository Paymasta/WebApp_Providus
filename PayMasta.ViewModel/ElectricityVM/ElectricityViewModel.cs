using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ElectricityVM
{
    public class ValidateMeter
    {
        public string meterNo { get; set; }
        public string accountType { get; set; }
        public string service { get; set; }
        public string amount { get; set; }
        public string channel { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Data()
        {
            data = new Datum();
        }
        public string responseCode { get; set; }
        public string message { get; set; }

        public Datum data { get; set; }
    }

    public class ValidateMeterResponse
    {
        public ValidateMeterResponse()
        {
            data=new Data1();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data1 data { get; set; }
        public object metadata { get; set; }
    }

    public class Data1
    {
        public bool error { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string responseCode { get; set; }
        public string productCode { get; set; }
    }

    public class Datum
    {
        public bool error { get; set; }
        public string message { get; set; }
        public string customerId { get; set; }
        public string name { get; set; }
        public string meterNumber { get; set; }
        public string accountNumber { get; set; }
        public string businessUnit { get; set; }
        public string businessUnitId { get; set; }
        public string undertaking { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime lastTransactionDate { get; set; }
        public string minimumPurchase { get; set; }
        public string customerArrears { get; set; }
        public string tariffCode { get; set; }
        public string tariff { get; set; }
        public string description { get; set; }
        public string customerType { get; set; }
        public string responseCode { get; set; }
        public string productCode { get; set; }
    }

    public class MeterValidateRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string Amount { get; set; }
        public string MeterNo { get; set; }
    }

    public class GetValidateMeterResponseResponses
    {
        public GetValidateMeterResponseResponses()
        {
            validateMeterResponse = new ValidateMeterResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public ValidateMeterResponse validateMeterResponse { get; set; }
    }
}
