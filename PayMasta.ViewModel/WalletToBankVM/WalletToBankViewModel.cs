using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.WalletToBankVM
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string bankCode { get; set; }
        public string bankAccronym { get; set; }
        public string type { get; set; }
        public string bankName { get; set; }
        public string shortCode { get; set; }
    }

    public class Metadata
    {
        public int size { get; set; }
    }

    public class PouchiBankListResponse
    {
        public PouchiBankListResponse()
        {
            data = new List<Datum>();
            metadata = new Metadata();
        }
        public string code { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public Metadata metadata { get; set; }
    }

    public class BankListResponse
    {
        public BankListResponse()
        {
            pouchiBankListResponse = new PouchiBankListResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public PouchiBankListResponse pouchiBankListResponse { get; set; }
    }
    public class LatestBankListResponse
    {
        public LatestBankListResponse()
        {
            pouchiBankListResponse = new List<CashConnectBanks>();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public List<CashConnectBanks> pouchiBankListResponse { get; set; }
    }

    public class WalletToBankPaymentRequest
    {
        public WalletToBankPaymentRequest()
        {
            this.AccountType = "";
        }
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string Amount { get; set; }
        public string beneficiaryName { get; set; }
        public string destBankCode { get; set; }
    }

    public class WalletToBankRequest
    {
        public string accountNumber { get; set; }
        public string specificChannel { get; set; }
        public decimal amount { get; set; }
        public string channel { get; set; }
        public string sourceBankCode { get; set; }
        public string sourceAccountNumber { get; set; }
        public string destBankCode { get; set; }
        public string pin { get; set; }
        public string transRef { get; set; }
        public string phoneNumber { get; set; }
        public string narration { get; set; }
        public bool isToBeSaved { get; set; }
        public string beneficiaryName { get; set; }
        public string charges { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WalletToBankResponse
    {
        public string message { get; set; }
        public string code { get; set; }
        public bool error { get; set; }
        public object status { get; set; }
        public object paymentTransactionDTO { get; set; }
    }
    public class GetWalletToBankResponse
    {
        public GetWalletToBankResponse()
        {
            vTUResponse = new WalletToBankResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public WalletToBankResponse vTUResponse { get; set; }
    }

    public class VerifyAccountRequest
    {
        public Guid UserGuid { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
    }
    public class VerifyAccount
    {
        public string accountNumber { get; set; }
        public string accountType { get; set; }
        public string bankCode { get; set; }
    }
    public class VerifyAccountResponse
    {
        public string accountNumber { get; set; }
        public string accountName { get; set; }
    }
    public class GetVerifyAccountResponse
    {
        public GetVerifyAccountResponse()
        {
            verifyAccountResponse = new VerifyAccountResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public VerifyAccountResponse verifyAccountResponse { get; set; }
    }


    public class CashConnectResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class CashConnectBanks
    {
        public string name { get; set; }
        public string code { get; set; }
    }
}
