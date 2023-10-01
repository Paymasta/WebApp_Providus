using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.FlutterWaveVM
{
    


    //public class Data
    //{
    //    public string response_code { get; set; }
    //    public string response_message { get; set; }
    //    public string order_ref { get; set; }
    //    public string account_number { get; set; }
    //    public string bank_name { get; set; }
    //    public string amount { get; set; }
    //}

    public class CreateVertualAccountResponse
    {
        public CreateVertualAccountResponse()
        {
            data = new PayMasta.ViewModel.FlutterWaveVM.Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public PayMasta.ViewModel.FlutterWaveVM.Data data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Customer
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public object phone_number { get; set; }
        public string email { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        public string flw_ref { get; set; }
        public string device_fingerprint { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int charged_amount { get; set; }
        public double app_fee { get; set; }
        public int merchant_fee { get; set; }
        public string processor_response { get; set; }
        public string auth_model { get; set; }
        public string ip { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string payment_type { get; set; }
        public DateTime created_at { get; set; }
        public int account_id { get; set; }
        public Customer customer { get; set; }
        //for created account below
        public string response_code { get; set; }
        public string response_message { get; set; }
        public string order_ref { get; set; }
        public string account_number { get; set; }
        public string bank_name { get; set; }
    }

    public class WebhookResponse
    {
        public WebhookResponse()
        {
            data = new Data();
        }
        public string @event { get; set; }
        public Data data { get; set; }
        public string EventType { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public int id { get; set; }
        public string biller_code { get; set; }
        public string name { get; set; }
        public double default_commission { get; set; }
        public DateTime date_added { get; set; }
        public string country { get; set; }
        public bool is_airtime { get; set; }
        public string biller_name { get; set; }
        public string item_code { get; set; }
        public string short_name { get; set; }
        public int fee { get; set; }
        public bool commission_on_fee { get; set; }
        public string label_name { get; set; }
        public decimal amount { get; set; }
    }

    public class AirtimeOperatorResponse
    {
        public AirtimeOperatorResponse()
        {
            data = new List<Datum>();
        }
        public string status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }
    public class AirtimeResponse
    {
        public AirtimeResponse()
        {
            operatorResponse = new List<WalletServiceResponse>();
            // airtimeOperator = new AirtimeOperatorResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public List<WalletServiceResponse> operatorResponse { get; set; }
        // public AirtimeOperatorResponse airtimeOperator { get; set; }
    }
    public class InternetResponse
    {
        public InternetResponse()
        {
            //operatorResponse = new List<WalletServiceResponse>();
            internetOperator = new AirtimeOperatorResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        // public List<WalletServiceResponse> operatorResponse { get; set; }
        public AirtimeOperatorResponse internetOperator { get; set; }
    }



    public class WalletServiceResponse
    {
        public string ServiceName { get; set; }
        public int SubCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string BankCode { get; set; }
        public string HttpVerbs { get; set; }
        public string RawData { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CountryCode { get; set; }
        public string BillerName { get; set; }

        public decimal Amount { get; set; }
        public int OperatorId { get; set; }
    }

    public class Airtime
    {
        public string country { get; set; }
        public string customer { get; set; }
        public string amount { get; set; }
        public string recurrence { get; set; }
        public string type { get; set; }
        public string reference { get; set; }
        public string biller_name { get; set; }
    }
    public class AirtimeRechargeRequest
    {
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public string BillerName { get; set; }
        public Guid UserGuid { get; set; }
        public string BillerCode { get; set; }
        public int OperatorId { get; set; }
    }

    //public class AirtimeRecharge
    //{
    //    public string status { get; set; }
    //    public string message { get; set; }
    //    public object data { get; set; }
    //}



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WalletServiceFilterRequest
    {
        public string BillerCode { get; set; }
    }

    public class WalletServiceOperatorResponse
    {
        public string ServiceName { get; set; }
       
        public string BankCode { get; set; }
      
        public long Id { get; set; }
        public string CountryCode { get; set; }
        public string BillerName { get; set; }

        public decimal Amount { get; set; }
        public decimal fee { get; set; }
        public decimal Commision { get; set; }
        public string LabelName { get; set; }
        public string ItemCode { get; set; }
    }


    public class ProductResponse
    {
        public ProductResponse()
        {
            operatorResponse = new List<WalletServiceOperatorResponse>();
            // airtimeOperator = new AirtimeOperatorResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public List<WalletServiceOperatorResponse> operatorResponse { get; set; }
        // public AirtimeOperatorResponse airtimeOperator { get; set; }
    }
}
