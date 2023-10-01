using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.PayAirtimeAndOtherBillsVM
{
    public class VTURequest
    {
        public string phone { get; set; }
        public string paymentMethod { get; set; }
        public string service { get; set; }
        public string amount { get; set; }
        public string clientReference { get; set; }
        public string channel { get; set; }
        public string sourceAccountNumber { get; set; }
        public string transactionPin { get; set; }
        public string narration { get; set; }
        public bool redeemBonus { get; set; }
        public double bonusAmount { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public bool error { get; set; }
        public string message { get; set; }
        public string amount { get; set; }
        public string @ref { get; set; }
        public string date { get; set; }
        public string transactionID { get; set; }
        public string value { get; set; }
        public string token { get; set; }
        public string response { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }

    public class VTUResponse
    {
        public VTUResponse()
        {
         //   data= new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public object metadata { get; set; }
    }
    public class ResponseForAirtimeRe
    {
        public bool error { get; set; }
        public string message { get; set; }
        public string amount { get; set; }
        public string @ref { get; set; }
        public string date { get; set; }
        public string transactionID { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }
    public class VTUBillPaymentRequest
    {
        public VTUBillPaymentRequest()
        {
            this.AccountType = "";
        }
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string phone { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
        public bool redeemBonus { get; set; }
        public double bonusAmount { get; set; }
    }

    public class GetVTUResponse
    {
        public GetVTUResponse()
        {
            vTUResponse = new VTUResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public VTUResponse vTUResponse { get; set; }
    }
    public class WalletToWalletTransferResponse
    {
        public string message { get; set; }
        public string code { get; set; }
        public bool error { get; set; }
        public string paymentTransactionDTO { get; set; }
    }
}
