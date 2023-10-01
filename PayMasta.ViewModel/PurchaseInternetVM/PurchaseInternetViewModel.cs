using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.PurchaseInternetVM
{
    public class Card
    {
    }

    public class InternetPurchaseRequest
    {
        public string phone { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string amount { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string clientReference { get; set; }
        public string productCode { get; set; }
        public Card card { get; set; }
        public string sourceAccountNumber { get; set; }
        public string transactionPin { get; set; }
        public string narration { get; set; }
        public bool redeemBonus { get; set; }
        public int bonusAmount { get; set; }
        public string charges { get; set; }
    }
    public class Data
    {
        public string responseCode { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public bool error { get; set; }
        public string transactionID { get; set; }
        public string reference { get; set; }
        public string bundle { get; set; }
        public double amount { get; set; }
    }

    public class InternetPurchaseResponse
    {
        public InternetPurchaseResponse()
        {
            data=new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }
    public class InternetPurchaseBillPaymentRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string phone { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
        public bool redeemBonus { get; set; }
        public double bonusAmount { get; set; }
        public string code { get; set; }
        public string productCode { get; set; }
    }
    public class GetInternetPurchaseRechargeResponse
    {
        public GetInternetPurchaseRechargeResponse()
        {
            internetPurchaseResponse = new InternetPurchaseResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public InternetPurchaseResponse internetPurchaseResponse { get; set; }
    }
}
