using PayMasta.ViewModel.InternetVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ProvidusExpresssWalletVM
{
    #region merchant login
    public class ProvidusMerchantLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Merchant
    {
        public string email { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public string mode { get; set; }
        public string role { get; set; }
        public string firstName { get; set; }
        public bool owner { get; set; }
        public string review { get; set; }
        public object callbackURL { get; set; }
        public string businessName { get; set; }
        public string businessType { get; set; }
        public object parentMerchant { get; set; }
        public bool canDebitCustomer { get; set; }
        public string sandboxCallbackURL { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class ProvidusMerchantLoginResponse
    {
        public ProvidusMerchantLoginResponse()
        {
            data = new Data();
            merchant = new Merchant();
        }
        public bool status { get; set; }
        public Data data { get; set; }
        public Merchant merchant { get; set; }
    }

    #endregion

    #region wallet create
    public class MetadataCreate
    {
        // [JsonProperty("even-more")]
        public string evenmore { get; set; }

        // [JsonProperty("additional-data")]
        public string additionaldata { get; set; }
    }

    public class WalletCreateRequest
    {
        public WalletCreateRequest()
        {
            metadata = new MetadataCreate();
        }
        public string bvn { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public MetadataCreate metadata { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            metadata = new WalletMetadata();
        }
        public string id { get; set; }
        public WalletMetadata metadata { get; set; }
        public string bvn { get; set; }
        public string dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string currency { get; set; }
        public string email { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string address { get; set; }
        public string BVNLastName { get; set; }
        public string BVNFirstName { get; set; }
        public bool nameMatch { get; set; }
        public string mode { get; set; }
        public string MerchantId { get; set; }
        public string tier { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class WalletMetadata
    {
        // [JsonProperty("even-more")]
        public string evenmore { get; set; }

        // [JsonProperty("additional-data")]
        public string additionaldata { get; set; }
    }

    public class WalletCreateResponse
    {
        public WalletCreateResponse()
        {
            wallet = new Wallet();
            customer = new Customer();
        }
        public bool status { get; set; }
        public Wallet wallet { get; set; }
        public Customer customer { get; set; }
    }

    public class Wallet
    {
        public string id { get; set; }
        public string mode { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public string walletId { get; set; }
        public string walletType { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public int bookedBalance { get; set; }
        public int availableBalance { get; set; }
        public string accountReference { get; set; }
    }
    #endregion

    #region customer wallet details
    public class CustomerWalletDetailResponse
    {
        public CustomerWalletDetailResponse()
        {
            wallet = new CustomerWallet();
        }
        public bool status { get; set; }
        public CustomerWallet wallet { get; set; }
        public string QrCode { get; set; }
    }

    public class CustomerWallet
    {
        public string id { get; set; }
        public string tier { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public string customerId { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string phoneNumber { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public decimal bookedBalance { get; set; }
        public decimal availableBalance { get; set; }
        public string accountReference { get; set; }
        public int minLimit { get; set; }
        public int maxLimit { get; set; }
    }
    #endregion

    #region Providus wallet to wallet

    public class WalletToWalletRequest
    {
        public int Amount { get; set; }
        public Guid UserGuid { get; set; }
        public string WalletAccountNumber { get; set; }
        public string beneficiaryName { get; set; }
        public int SubCategoryId { get; set; }
        public string Service { get; set; }
    }

    public class ProvidusWalletToWalletRequest
    {
        public int amount { get; set; }
        public string fromCustomerId { get; set; }
        public string toCustomerId { get; set; }
    }


    public class WalletToWalletData
    {
        public int amount { get; set; }
        public string reference { get; set; }
        public int total { get; set; }
        public int transaction_fee { get; set; }
        public string target_customer_id { get; set; }
        public string source_customer_id { get; set; }
        public string target_customer_wallet { get; set; }
        public string source_customer_wallet { get; set; }
        public string description { get; set; }
    }

    public class ProvidusWalletToWalletResponse
    {
        public ProvidusWalletToWalletResponse()
        {
            data = new WalletToWalletData();
        }
        public bool status { get; set; }
        public string message { get; set; }
        public WalletToWalletData data { get; set; }
    }

    public class GetExpressWalletToBankResponse
    {
        public GetExpressWalletToBankResponse()
        {
            vTUResponse = new ProvidusWalletToWalletResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ProvidusWalletToWalletResponse vTUResponse { get; set; }
    }
    #endregion

    #region express bank list
    public class Bank
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class ExpressBankList
    {
        public ExpressBankList()
        {
            banks = new List<Bank>();
        }
        public bool status { get; set; }
        public List<Bank> banks { get; set; }
    }

    public class GetExpressBankListResponse
    {
        public GetExpressBankListResponse()
        {
            expressBankList = new ExpressBankList();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        //public string productCode { get; set; }
        public ExpressBankList expressBankList { get; set; }
    }
    #endregion

    #region Bank account verify response
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class VerifyAccount
    {
        public string bankCode { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
    }

    public class VerifyExpressBankAccount
    {
        public VerifyExpressBankAccount()
        {
            account = new VerifyAccount();
        }
        public bool status { get; set; }
        public VerifyAccount account { get; set; }
    }


    #endregion

    #region wallet to bank transfer 
    public class WalletToBankMetadata
    {

        public string customerdata { get; set; }
    }

    public class ExpressWalletToBankRequest
    {
        public ExpressWalletToBankRequest()
        {
            metadata = new WalletToBankMetadata();
        }
        public decimal amount { get; set; }
        public string sortCode { get; set; }
        public string narration { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string customerId { get; set; }
        public WalletToBankMetadata metadata { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdditionalMetadata
    {
        //  [JsonProperty("customer-data")]
        public string customerdata { get; set; }
    }

    public class WalletToBankResponseMetadata
    {
        public WalletToBankResponseMetadata()
        {
            additionalMetadata = new AdditionalMetadata();
        }
        public int amount { get; set; }
        public int charges { get; set; }
        public int vat { get; set; }
        public string sortCode { get; set; }
        public string narration { get; set; }
        public string customerId { get; set; }
        public string accountName { get; set; }
        public int totalCharges { get; set; }
        public string accountNumber { get; set; }
        public AdditionalMetadata additionalMetadata { get; set; }
        public string nameEnquiryRef { get; set; }
        public string transactionReference { get; set; }
    }

    public class ExpressWalletToBankResponse
    {
        public ExpressWalletToBankResponse()
        {
            transfer = new Transfer();
        }
        public bool status { get; set; }
        public string message { get; set; }
        public Transfer transfer { get; set; }
    }

    public class Transfer
    {
        public Transfer()
        {
            metadata = new WalletToBankResponseMetadata();
        }
        public int amount { get; set; }
        public int charges { get; set; }
        public int vat { get; set; }
        public WalletToBankResponseMetadata metadata { get; set; }
        public string reference { get; set; }
        public string customerId { get; set; }
        public string status { get; set; }
        public int total { get; set; }
        public string transactionReference { get; set; }
        public string description { get; set; }
        public string destination { get; set; }
    }
    public class GetExpressWalletToBankRes
    {
        public GetExpressWalletToBankRes()
        {
            expressWalletToBankResponse = new ExpressWalletToBankResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ExpressWalletToBankResponse expressWalletToBankResponse { get; set; }
    }

    #endregion


    #region debit customer wallet
    public class DebitWalletMetadata
    {
        public string somedata { get; set; }
        public string moredata { get; set; }
    }

    public class DebitCustomerWalletRequest
    {
        public DebitCustomerWalletRequest()
        {
            metadata = new DebitWalletMetadata();
        }
        public decimal amount { get; set; }
        public string reference { get; set; }
        public string customerId { get; set; }
        public DebitWalletMetadata metadata { get; set; }
    }

    public class WalletDebitData
    {
        public WalletDebitData()
        {
            metadata = new DebitWalletMetadata();
        }
        public decimal amount { get; set; }
        public string reference { get; set; }
        public string customer_id { get; set; }
        public DebitWalletMetadata metadata { get; set; }
        public int transaction_fee { get; set; }
        public string customer_wallet_id { get; set; }
    }

    public class DebitCustomerWalletResponse
    {
        public DebitCustomerWalletResponse()
        {
            data = new WalletDebitData();
        }
        public bool status { get; set; }
        public string message { get; set; }
        public WalletDebitData data { get; set; }
    }

    #endregion

    #region verify account number
    public class Account
    {
        public string bankCode { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
    }

    public class ExpressVerifyAccount
    {
        public ExpressVerifyAccount()
        {
            account = new Account();
        }
        public bool status { get; set; }
        public Account account { get; set; }
    }
    #endregion

}
