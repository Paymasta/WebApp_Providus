using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.PurchaseStarLineVM
{
    public class Card
    {
    }

    public class StarLinePurchaseReqeust
    {
        public string phone { get; set; }
        public string bouquet { get; set; }
        public string cycle { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string clientReference { get; set; }
        public string productCode { get; set; }
        public Card card { get; set; }
        public double amount { get; set; }
        public string sourceAccountNumber { get; set; }
        public string transactionPin { get; set; }
        public string narration { get; set; }
        public bool redeemBonus { get; set; }
        public double bonusAmount { get; set; }
        public string charges { get; set; }
    }

    public class Data
    {
        public bool error { get; set; }
        public string smartCardCode { get; set; }
        public string name { get; set; }
        public string bouquet { get; set; }
        public string externalReference { get; set; }
        public double amount { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }

    public class StarLinePurchaseResponse
    {
        public StarLinePurchaseResponse()
        {
            data = new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }

    public class StarLinePurchaseBillPaymentRequest
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
    public class GetStarLinePurchaseRechargeResponse
    {
        public GetStarLinePurchaseRechargeResponse()
        {
            starLinePurchaseResponse = new StarLinePurchaseResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public StarLinePurchaseResponse starLinePurchaseResponse { get; set; }
    }
}
