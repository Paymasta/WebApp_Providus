using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VirtualAccountVM
{
    //public class CreateVertualAccountRequest
    //{
    //    public string account_name { get; set; }
    //    public string email { get; set; }
    //    public string mobilenumber { get; set; }
    //    public string country { get; set; }
    //}
    //public class VirtualAccountNumberResponse
    //{
    //    public VirtualAccountNumberResponse()
    //    {
    //        data = new PayMasta.ViewModel.VirtualAccountVM.Data();
    //    }

    //    public string status { get; set; }
    //    public string message { get; set; }
    //    public PayMasta.ViewModel.VirtualAccountVM.Data data { get; set; }
    //}

    //public class Data
    //{
    //    public int id { get; set; }
    //    public string account_reference { get; set; }
    //    public string account_name { get; set; }
    //    public string barter_id { get; set; }
    //    public string email { get; set; }
    //    public string mobilenumber { get; set; }
    //    public string country { get; set; }
    //    public string nuban { get; set; }
    //    public string bank_name { get; set; }
    //    public string bank_code { get; set; }
    //    public string status { get; set; }
    //    public DateTime created_at { get; set; }

    //    //-----------------balance keys

    //    public string currency { get; set; }
    //    public double available_balance { get; set; }
    //    public int ledger_balance { get; set; }
    //}

    //public class GetVirtualAccountBalanceResponse
    //{
    //    public GetVirtualAccountBalanceResponse()
    //    {
    //        this.RstKey = 0;
    //    }
    //    public string Currency { get; set; }
    //    public double AvailableBalance { get; set; }
    //    public int LedgerBalance { get; set; }
    //    public int RstKey { get; set; }
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class AccountOwner
    //{
    //    public AccountOwner()
    //    {
    //        user=new User();
    //    }
    //    public int id { get; set; }
    //    public string profileID { get; set; }
    //    public string pin { get; set; }
    //    public object deviceNotificationToken { get; set; }
    //    public string phoneNumber { get; set; }
    //    public string gender { get; set; }
    //    public string dateOfBirth { get; set; }
    //    public string address { get; set; }
    //    public object photo { get; set; }
    //    public object photoContentType { get; set; }
    //    public object bvn { get; set; }
    //    public object validID { get; set; }
    //    public object paymentTransactions { get; set; }
    //    public object billerTransactions { get; set; }
    //    public object customersubscriptions { get; set; }
    //    public User user { get; set; }
    //    public object profileType { get; set; }
    //    public object kyc { get; set; }
    //    public string fullName { get; set; }
    //}

    //public class Data
    //{
    //    public Data()
    //    {
    //        scheme=new Scheme();
    //        AccountOwner = new AccountOwner();
    //    }
    //    public int id { get; set; }
    //    public string accountName { get; set; }
    //    public string accountNumber { get; set; }
    //    public string currentBalance { get; set; }
    //    public string dateOpened { get; set; }
    //    public Scheme scheme { get; set; }
    //    public object WalletAccountType { get; set; }
    //    public AccountOwner AccountOwner { get; set; }
    //}

    //public class AccountResponseData
    //{
    //    public AccountResponseData()
    //    {
    //        this.data=new Data();
    //    }
    //    public string code { get; set; }
    //    public string message { get; set; }
    //    public Data data { get; set; }
    //}

    //public class Scheme
    //{
    //    public string id { get; set; }
    //    public int schemeID { get; set; }
    //    public string scheme { get; set; }
    //    public object schemeCategory { get; set; }
    //}

    //public class User
    //{
    //    public int id { get; set; }
    //    public string login { get; set; }
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public object email { get; set; }
    //    public bool activated { get; set; }
    //    public object langKey { get; set; }
    //    public string imageUrl { get; set; }
    //    public object resetDate { get; set; }
    //}
    #region Wallet Creation
    public class CreateVirtualAccountForUserRequest
    {
        public string phoneNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string pin { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string state { get; set; }
        public string localGovt { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
        public string scheme { get; set; }
        public string accountName { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AccountOwner
    {
        public object createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int id { get; set; }
        public object profileID { get; set; }
        public object pin { get; set; }
        public object deviceNotificationToken { get; set; }
        public object phoneNumber { get; set; }
        public object gender { get; set; }
        public object dateOfBirth { get; set; }
        public object address { get; set; }
        public object secretQuestion { get; set; }
        public object secretAnswer { get; set; }
        public object photo { get; set; }
        public object photoContentType { get; set; }
        public object bvn { get; set; }
        public object validID { get; set; }
        public string validDocType { get; set; }
        public object nin { get; set; }
        public object profilePicture { get; set; }
        public double totalBonus { get; set; }
        public List<object> myDevices { get; set; }
        public List<object> paymentTransactions { get; set; }
        public List<object> billerTransactions { get; set; }
        public List<object> customersubscriptions { get; set; }
        public object user { get; set; }
        public List<object> bonusPoints { get; set; }
        public object approvalGroup { get; set; }
        public object profileType { get; set; }
        public object kyc { get; set; }
        public List<object> beneficiaries { get; set; }
        public List<object> addresses { get; set; }
        public string fullName { get; set; }
    }

    public class Datum
    {
        public Datum()
        {
            this.scheme = new Scheme();
            this.walletAccountType = new WalletAccountType();
            this.accountOwner = new AccountOwner();
            this.subWallets = new List<object>();
        }
        public int id { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string currentBalance { get; set; }
        public object nubanAccountNo { get; set; }
        public object trackingRef { get; set; }
        public string dateOpened { get; set; }
        public string status { get; set; }
        public string actualBalance { get; set; }
        public string walletLimit { get; set; }
        public Scheme scheme { get; set; }
        public WalletAccountType walletAccountType { get; set; }
        public AccountOwner accountOwner { get; set; }
        public object parent { get; set; }
        public List<object> subWallets { get; set; }
        public string accountFullName { get; set; }
    }

    public class AccountResponseData
    {
        public AccountResponseData()
        {
            this.data = new List<Datum>();
        }
        public string code { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public object metadata { get; set; }
    }

    public class Scheme
    {
        public int id { get; set; }
        public object schemeID { get; set; }
        public object scheme { get; set; }
        public object bankCode { get; set; }
        public object accountNumber { get; set; }
        public object apiKey { get; set; }
        public object secretKey { get; set; }
        public object schemeCategory { get; set; }
        public object callbackUrl { get; set; }
    }

    public class WalletAccountType
    {
        public int id { get; set; }
        public object accountypeID { get; set; }
        public object walletAccountType { get; set; }
    }
    #endregion Wallet Creation
    #region Authenticate
    public class AuthenticateRequest
    {
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string username { get; set; }
        public string scheme { get; set; }
        public string deviceId { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Kyc
    //{
    //    public int id { get; set; }
    //    public int kycID { get; set; }
    //    public string kyc { get; set; }
    //    public string description { get; set; }
    //    public int kycLevel { get; set; }
    //    public bool phoneNumber { get; set; }
    //    public bool emailAddress { get; set; }
    //    public bool firstName { get; set; }
    //    public bool lastName { get; set; }
    //    public bool gender { get; set; }
    //    public bool dateofBirth { get; set; }
    //    public bool address { get; set; }
    //    public bool photoUpload { get; set; }
    //    public bool verifiedBVN { get; set; }
    //    public bool verifiedValidID { get; set; }
    //    public bool evidenceofAddress { get; set; }
    //    public object verificationofAddress { get; set; }
    //    public object employmentDetails { get; set; }
    //    public double dailyTransactionLimit { get; set; }
    //    public double cumulativeBalanceLimit { get; set; }
    //    public object paymentTransaction { get; set; }
    //    public object billerTransaction { get; set; }
    //}

    //public class AuthenticateResponse
    //{
    //    public AuthenticateResponse()
    //    {
    //        walletAccountList = new List<WalletAccountList>();
    //        user=new User();
    //    }
    //    public string message { get; set; }
    //    public object code { get; set; }
    //    public string token { get; set; }
    //    public User user { get; set; }
    //    public string userType { get; set; }
    //    public List<WalletAccountList> walletAccountList { get; set; }
    //}

    //public class User
    //{
    //    public User()
    //    {
    //        kyc=new Kyc();
    //        user=new User();
    //    }
    //    public DateTime createdDate { get; set; }
    //    public DateTime lastModifiedDate { get; set; }
    //    public int id { get; set; }
    //    public string profileID { get; set; }
    //    public string pin { get; set; }
    //    public object deviceNotificationToken { get; set; }
    //    public string phoneNumber { get; set; }
    //    public object gender { get; set; }
    //    public string dateOfBirth { get; set; }
    //    public string address { get; set; }
    //    public object secretQuestion { get; set; }
    //    public object secretAnswer { get; set; }
    //    public object photo { get; set; }
    //    public object photoContentType { get; set; }
    //    public object bvn { get; set; }
    //    public object validID { get; set; }
    //    public string validDocType { get; set; }
    //    public object nin { get; set; }
    //    public object profilePicture { get; set; }
    //    public double totalBonus { get; set; }
    //    public object walletAccounts { get; set; }
    //    public object myDevices { get; set; }
    //    public object paymentTransactions { get; set; }
    //    public object billerTransactions { get; set; }
    //    public object customersubscriptions { get; set; }
    //    public User user { get; set; }
    //    public object bonusPoints { get; set; }
    //    public object approvalGroup { get; set; }
    //    public object profileType { get; set; }
    //    public Kyc kyc { get; set; }
    //    public object beneficiaries { get; set; }
    //    public object addresses { get; set; }
    //    public string fullName { get; set; }
    //    public string login { get; set; }
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public string email { get; set; }
    //    public bool activated { get; set; }
    //    public object langKey { get; set; }
    //    public object imageUrl { get; set; }
    //    public object resetDate { get; set; }
    //    public string status { get; set; }
    //}

    //public class WalletAccountList
    //{
    //    public int id { get; set; }
    //    public string accountNumber { get; set; }
    //    public double currentBalance { get; set; }
    //    public string dateOpened { get; set; }
    //    public int schemeId { get; set; }
    //    public string schemeName { get; set; }
    //    public int walletAccountTypeId { get; set; }
    //    public int accountOwnerId { get; set; }
    //    public string accountOwnerName { get; set; }
    //    public string accountOwnerPhoneNumber { get; set; }
    //    public string accountName { get; set; }
    //    public string status { get; set; }
    //    public double actualBalance { get; set; }
    //    public string walletLimit { get; set; }
    //    public object trackingRef { get; set; }
    //    public string nubanAccountNo { get; set; }
    //    public string accountFullName { get; set; }
    //    public double totalCustomerBalances { get; set; }
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Kyc
    {
        public int id { get; set; }
        public int kycID { get; set; }
        public string kyc { get; set; }
        public string description { get; set; }
        public int kycLevel { get; set; }
        public bool phoneNumber { get; set; }
        public bool emailAddress { get; set; }
        public bool firstName { get; set; }
        public bool lastName { get; set; }
        public bool gender { get; set; }
        public bool dateofBirth { get; set; }
        public bool address { get; set; }
        public bool photoUpload { get; set; }
        public bool verifiedBVN { get; set; }
        public bool verifiedValidID { get; set; }
        public bool evidenceofAddress { get; set; }
        public object verificationofAddress { get; set; }
        public object employmentDetails { get; set; }
        public double dailyTransactionLimit { get; set; }
        public double cumulativeBalanceLimit { get; set; }
        public object paymentTransaction { get; set; }
        public object billerTransaction { get; set; }
    }

    public class AuthenticateResponse
    {
        public AuthenticateResponse()
        {
            user = new User();
            walletAccountList = new List<WalletAccountList>();
        }
        public string message { get; set; }
        public object code { get; set; }
        public string token { get; set; }
        public User user { get; set; }
        public string userType { get; set; }
        public List<WalletAccountList> walletAccountList { get; set; }
    }

    public class User
    {
        public User()
        {
            kyc = new Kyc();
            user = new UserData();
        }
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int id { get; set; }
        public string profileID { get; set; }
        public string pin { get; set; }
        public object deviceNotificationToken { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string address { get; set; }
        public object secretQuestion { get; set; }
        public object secretAnswer { get; set; }
        public object photo { get; set; }
        public object photoContentType { get; set; }
        public object bvn { get; set; }
        public object validID { get; set; }
        public string validDocType { get; set; }
        public object nin { get; set; }
        public object profilePicture { get; set; }
        public double totalBonus { get; set; }
        public object walletAccounts { get; set; }
        public object myDevices { get; set; }
        public object paymentTransactions { get; set; }
        public object billerTransactions { get; set; }
        public object customersubscriptions { get; set; }
        public UserData user { get; set; }
        public object bonusPoints { get; set; }
        public object approvalGroup { get; set; }
        public object profileType { get; set; }
        public Kyc kyc { get; set; }
        public object beneficiaries { get; set; }
        public object addresses { get; set; }
        public string fullName { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool activated { get; set; }
        public object langKey { get; set; }
        public object imageUrl { get; set; }
        public object resetDate { get; set; }
        public string status { get; set; }
    }

    public class WalletAccountList
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public double currentBalance { get; set; }
        public string dateOpened { get; set; }
        public int schemeId { get; set; }
        public string schemeName { get; set; }
        public int walletAccountTypeId { get; set; }
        public int accountOwnerId { get; set; }
        public string accountOwnerName { get; set; }
        public string accountOwnerPhoneNumber { get; set; }
        public string accountName { get; set; }
        public string status { get; set; }
        public double actualBalance { get; set; }
        public string walletLimit { get; set; }
        public object trackingRef { get; set; }
        public string nubanAccountNo { get; set; }
        public string accountFullName { get; set; }
        public double totalCustomerBalances { get; set; }
    }

    public class UserData
    {
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int id { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool activated { get; set; }
        public object langKey { get; set; }
        public object imageUrl { get; set; }
        public object resetDate { get; set; }
        public string status { get; set; }
    }
    #endregion Authenticate

    public class CurrentBalanceResponse
    {
        public int RstKey { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string Balance { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string NubanAccountNumber { get; set; }
        public string ImageUrl { get; set; }
    }
    public class CurrentBalance
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public double currentBalance { get; set; }
        public string dateOpened { get; set; }
        public int schemeId { get; set; }
        public string schemeName { get; set; }
        public int walletAccountTypeId { get; set; }
        public int accountOwnerId { get; set; }
        public string accountOwnerName { get; set; }
        public string accountOwnerPhoneNumber { get; set; }
        public string accountName { get; set; }
        public string status { get; set; }
        public double actualBalance { get; set; }
        public string walletLimit { get; set; }
        public object trackingRef { get; set; }
        public string nubanAccountNo { get; set; }
        public string accountFullName { get; set; }
        public double totalCustomerBalances { get; set; }
    }

    public class BalanceArray
    {
        public BalanceArray()
        {
            currentBalances = new List<CurrentBalance>();
        }
        public List<CurrentBalance> currentBalances { get; set; }
    }
    public class ChangeWalletPasswordRequest
    {
        public string phoneNumber { get; set; }
        public string pin { get; set; }
        public string newPassword { get; set; }
    }
    public class ChangeWalletPasswordResponse
    {
        public string message { get; set; }
        public string code { get; set; }
        public bool error { get; set; }
        public string status { get; set; }
        public object paymentTransactionDTO { get; set; }
    }
    public class GetNubanResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public object metadata { get; set; }
    }
    public class UpdatePinRequest
    {
        public string phoneNumber { get; set; }
        public string newPin { get; set; }
    }
    public class UpdatePinResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public object metadata { get; set; }
    }
}
