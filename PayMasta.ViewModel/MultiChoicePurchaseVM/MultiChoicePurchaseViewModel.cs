using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.MultiChoicePurchaseVM
{
    public class Card
    {
    }

    public class MultiChoicePurchaseRequest
    {
        public string phone { get; set; }
        public string code { get; set; }
        public bool renew { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string clientReference { get; set; }
        public int productMonths { get; set; }
        public string totalAmount { get; set; }
        public string productCode { get; set; }
        public Card card { get; set; }
        public string channel { get; set; }
        public string sourceAccountNumber { get; set; }
        public string transactionPin { get; set; }
        public string narration { get; set; }
        public string redeemBonus { get; set; }
        public string bonusAmount { get; set; }
        public string charges { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Data()
        {
            response = new Response();
        }
        public bool error { get; set; }
        public string message { get; set; }
        public string @ref { get; set; }
        public string amount { get; set; }
        public string bouquetCode { get; set; }
        public string bouquetName { get; set; }
        public string account { get; set; }
        public string externalReference { get; set; }
        public string auditReferenceNumber { get; set; }
        public string date { get; set; }
        public Response response { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }

    public class Response
    {
    }

    public class MultiChoicePurchaseResponse
    {
       
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public object metadata { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataResponse
    {
        public string statusCode { get; set; }
        public string transactionStatus { get; set; }
        public string transactionReference { get; set; }
        public string transactionMessage { get; set; }
        public string baxiReference { get; set; }
        public string provider_message { get; set; }
    }

    public class DataPurchaseResponse
    {
        public DataPurchaseResponse()
        {
            response=new DataResponse();
        }
        public bool error { get; set; }
        public string message { get; set; }
        public string @ref { get; set; }
        public string amount { get; set; }
        public string bouquetCode { get; set; }
        public string bouquetName { get; set; }
        public string account { get; set; }
        public string externalReference { get; set; }
        public string auditReferenceNumber { get; set; }
        public string date { get; set; }
        public DataResponse response { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }


    public class MultiChoicePurchaseBillPaymentRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string phone { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
        public string redeemBonus { get; set; }
        public string bonusAmount { get; set; }
        public string code { get; set; }
        public string productCode { get; set; }
    }
    public class GetMultiChoiceRechargeResponse
    {
        public GetMultiChoiceRechargeResponse()
        {
            multiChoicePurchaseResponse = new MultiChoicePurchaseResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public MultiChoicePurchaseResponse multiChoicePurchaseResponse { get; set; }
    }
}
