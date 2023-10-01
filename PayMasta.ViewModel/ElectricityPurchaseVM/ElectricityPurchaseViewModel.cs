using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ElectricityPurchaseVM
{
    public class Card
    {
    }

    public class ElectricityPurchaseRequest
    {
        public string customerPhoneNumber { get; set; }
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
        public string account { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string accountNumber { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string minimumPurchase { get; set; }
        public string businessUnit { get; set; }
        public string businessUnitId { get; set; }
        public string undertaking { get; set; }
        public string customerArrears { get; set; }
        public string tariffCode { get; set; }
        public string tariff { get; set; }
        public string paidamount { get; set; }
        public string merchantId { get; set; }
        public string recieptNumber { get; set; }
        public DateTime transactionDate { get; set; }
        public string transactionReference { get; set; }
        public string transactionStatus { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string externalReference { get; set; }
        public string type { get; set; }
        public string units { get; set; }
        public string vat { get; set; }
        public double costofunit { get; set; }
        public DateTime lastTransactionDate { get; set; }
        public string responseCode { get; set; }
        public double amount { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }

    public class ElectricityPurchaseResponse
    {
        public ElectricityPurchaseResponse()
        {
            data=new BillData();
        }
        public string code { get; set; }
        public string message { get; set; }
        public BillData data { get; set; }
        public object metadata { get; set; }
    }
    public class BillData
    {
       public BillData()
        {
            tokenList = new TokenList();
        }
        public int status { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public string date { get; set; }
        public string @ref { get; set; }
        public string token { get; set; }
        public string address { get; set; }
        public string payer { get; set; }
        public double amount { get; set; }
        public string account_type { get; set; }
        public string kct { get; set; }
        public string client_id { get; set; }
        public string sgc { get; set; }
        public string msno { get; set; }
        public string unit_cost { get; set; }
        public string tran_id { get; set; }
        public string krn { get; set; }
        public string ti { get; set; }
        public string tt { get; set; }
        public string unit { get; set; }
        public string adjust_unit { get; set; }
        public string preset_unit { get; set; }
        public string total_unit { get; set; }
        public string unit_value { get; set; }
        public string manuf { get; set; }
        public string model { get; set; }
        public string feederBand { get; set; }
        public string feederName { get; set; }
        public string arrears { get; set; }
        public string balance { get; set; }
        public string refund { get; set; }
        public string walletBalance { get; set; }
        public string tariff { get; set; }
        public string tariff_class { get; set; }
        public string vatRate { get; set; }
        public string response_code { get; set; }
        public string transactionUniqueNumber { get; set; }
        public string transactionDateTime { get; set; }
        public string transactionDebitTransactionId { get; set; }
        public string dealer_name { get; set; }
        public string agent_name { get; set; }
        public string agent_code { get; set; }
        public string rate { get; set; }
        public string type { get; set; }
        public TokenList tokenList { get; set; }
        public string outstandingDebt { get; set; }
        public string costOfUnit { get; set; }
        public string vat { get; set; }
        public string fixedCharge { get; set; }
        public string reconnectionFee { get; set; }
        public string lor { get; set; }
        public string administrativeCharge { get; set; }
        public string installationFee { get; set; }
        public string penalty { get; set; }
        public string meterCost { get; set; }
        public string currentCharge { get; set; }
        public string msc { get; set; }
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string sequence { get; set; }
        public string clientReference { get; set; }
    }
    public class TokenList
    {
        public string token1 { get; set; }
        public string token1_desc { get; set; }
        public string token2_desc { get; set; }
        public string token3_desc { get; set; }
    }
    public class ElectricityBillPaymentRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string phone { get; set; }
        public double Amount { get; set; }
        public string meterNo { get; set; }
        public bool redeemBonus { get; set; }
        public double bonusAmount { get; set; }
        public string code { get; set; }
        public string productCode { get; set; }
    }
    public class GetElectricityRechargeResponse
    {
        public GetElectricityRechargeResponse()
        {
            electricityPurchaseResponse = new ElectricityPurchaseResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public ElectricityPurchaseResponse electricityPurchaseResponse { get; set; }
    }
}
