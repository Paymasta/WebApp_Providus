using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ZealvendBillsVM
{
    #region Airtime

    public class GetAirtimeZealvendResponse
    {
        public GetAirtimeZealvendResponse()
        {
            vTUResponse = new ZealvendAirtimeResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public ZealvendAirtimeResponse vTUResponse { get; set; }
    }
    public class ZealvendAirtimeRequest
    {
        public string network { get; set; }
        public string number { get; set; }
        public decimal amount { get; set; }
        public string reference { get; set; }
    }

    public class AirData
    {
        public string number { get; set; }
        public string referrence { get; set; }
        public string network { get; set; }
        public int amount { get; set; }
        public int price { get; set; }
        public string status { get; set; }
        public int user_id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
        public int wallet_id { get; set; }
    }

    public class ZealvendAirtimeResponse
    {
        public ZealvendAirtimeResponse() { data = new AirData(); }
        public string status { get; set; }
        public AirData data { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Error
    {
        public Error()
        {
            errors = new Errors();
        }
        public string message { get; set; }
        public Errors errors { get; set; }
        public int status_code { get; set; }
    }

    public class Errors
    {
        public List<string> number { get; set; }
    }

    public class AirtimeError
    {
        public AirtimeError()
        {
            error = new Error();
        }
        public Error error { get; set; }
    }


    #endregion

    public class ResponseFromZealvend
    {
        public int StatusCode { get; set; }
        public string JsonString { get; set; }
    }
}
