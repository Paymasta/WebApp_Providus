using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.WalletBalanceAndNuban
{
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

    public class NubanResponse
    {
        public NubanResponse()
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
        public User user { get; set; }
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
        public string trackingRef { get; set; }
        public string nubanAccountNo { get; set; }
        public string accountFullName { get; set; }
        public double totalCustomerBalances { get; set; }
    }


}
