using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.DataRechargeVM
{
    public class Card
    {
    }

    public class DataRechargeRequest
    {
        public string phone { get; set; }
        public string code { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string clientReference { get; set; }
        public string productCode { get; set; }
        public Card card { get; set; }
        public string sourceAccountNumber { get; set; }
        public string transactionPin { get; set; }
        public string narration { get; set; }
        public string redeemBonus { get; set; }
        public string bonusAmount { get; set; }
        public string charges { get; set; }
    }

    public class Data
    {
        public bool error { get; set; }
        public string message { get; set; }
        public int amount { get; set; }
        public string @ref { get; set; }
        public string date { get; set; }
        public string transactionID { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
    }

    public class DataRechargeResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public object metadata { get; set; }
    }
    public class BillPaymentForData
    {
        public bool error { get; set; }
        public string message { get; set; }
        public int amount { get; set; }
        public string @ref { get; set; }
        public string date { get; set; }
        public string transactionID { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }
    public class GetDataRechargeResponse
    {
        public GetDataRechargeResponse()
        {
            dataRechargeResponse = new DataRechargeResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public DataRechargeResponse dataRechargeResponse { get; set; }
    }
        public class DataBillPaymentRequest
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
}
