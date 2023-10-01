using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.OkraAPIVM
{
    #region Income
    public class Stream
    {
        public string _id { get; set; }
        public string account { get; set; }
        public int avg_monthly_income { get; set; }
        public string bank { get; set; }
        public int days { get; set; }
        public int monthly_income { get; set; }
        public string income_type { get; set; }
        public string employer { get; set; }
        public DateTime? income_start_date { get; set; }
    }

    public class Income
    {
        public Income()
        {
            streams = new List<Stream>();
        }
        public string _id { get; set; }
        public string customer { get; set; }
        public int __v { get; set; }
        public string confidence { get; set; }
        public DateTime created_at { get; set; }
        public string env { get; set; }
        public double last_two_years_income { get; set; }
        public double last_two_years_to_this_year_projected_income { get; set; }
        public DateTime last_updated { get; set; }
        public double last_year_income { get; set; }
        public double last_year_to_this_year_projected_income { get; set; }
        public int max_number_of_overlapping_income_streams { get; set; }
        public int number_of_income_streams { get; set; }
        public List<string> owner { get; set; }
        public double projected_yearly_income { get; set; }
        public List<string> projects { get; set; }
        public List<string> record { get; set; }
        public List<Stream> streams { get; set; }
        public string version { get; set; }
    }

    public class PeriodicTransactions
    {
        public bool status { get; set; }
        public int number { get; set; }
    }

    public class Guarantors
    {
        public bool status { get; set; }
        public int number { get; set; }
    }

    public class Directors
    {
        public bool status { get; set; }
        public int number { get; set; }
    }

    public class Products
    {
        public Products()
        {
            PeriodicTransactions = new PeriodicTransactions();
            guarantors = new Guarantors();
            directors = new Directors();
        }
        //  [JsonProperty("periodic-transactions")]
        public PeriodicTransactions PeriodicTransactions { get; set; }
        public Guarantors guarantors { get; set; }
        public Directors directors { get; set; }
        public bool auth { get; set; }
        public bool balance { get; set; }
        public bool accounts { get; set; }
        public bool transactions { get; set; }
        public bool income { get; set; }
        public bool identity { get; set; }

        //  [JsonProperty("complete-view")]
        public bool CompleteView { get; set; }

        //[JsonProperty("insurance-claims")]
        public bool InsuranceClaims { get; set; }

        // [JsonProperty("abuse-recognition")]
        public bool AbuseRecognition { get; set; }
        public bool liabilities { get; set; }

        //[JsonProperty("spending-pattern")]
        public bool SpendingPattern { get; set; }
        public bool investments { get; set; }
        public bool revenue { get; set; }
        public bool payment { get; set; }
        public bool refund { get; set; }

        // [JsonProperty("direct-debit")]
        public bool DirectDebit { get; set; }
    }

    public class Process
    {
        public bool running { get; set; }
        public bool completed { get; set; }
    }

    public class Status
    {
        public Status()
        {
            process = new Process();
            transactions = new Transactions();
        }
        public Process process { get; set; }
        public Transactions transactions { get; set; }
    }



    //public class Data
    //{
    //    public Income income { get; set; }
    //    public object last_transaction_date { get; set; }
    //    public string product { get; set; }
    //    public string record { get; set; }
    //    public Products products { get; set; }
    //    public Status status { get; set; }
    //}

    public class OkraIncomeResponseModel
    {
        public OkraIncomeResponseModel()
        {
            data = new PayMasta.ViewModel.OkraAPIVM.@Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public PayMasta.ViewModel.OkraAPIVM.@Data data { get; set; }
    }

    public class @Data
    {
        public @Data()
        {

            income = new Income();
            products = new Products();
            status = new Status();
            auth = new PayMasta.ViewModel.OkraAPIVM.Auth();
            balance = new Balance();
            balances = new List<Balance>();
            transactions = new List<Transaction>();
        }
        public Income income { get; set; }
        public object last_transaction_date { get; set; }
        public string product { get; set; }
        public string record { get; set; }
        public Products products { get; set; }
        public Status status { get; set; }
        public PayMasta.ViewModel.OkraAPIVM.Auth auth { get; set; }
        public Balance balance { get; set; }
        public List<Balance> balances { get; set; }
        public object msg { get; set; }

        public List<Transaction> transactions { get; set; }

    }

    #endregion income

    public class IncomeWodgetLinkGenerateResponse
    {
        public IncomeWodgetLinkGenerateResponse()
        {
            okraIncomeResponseModel = new OkraIncomeResponseModel();
        }
        public OkraIncomeResponseModel okraIncomeResponseModel { get; set; }
        public int RstKey { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    #region Auth
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Auth
    {
        public string _id { get; set; }
        public string record { get; set; }
        public int __v { get; set; }
        public string bank { get; set; }
        public DateTime created_at { get; set; }
        public string current_project { get; set; }
        public string customer { get; set; }
        public string env { get; set; }
        public DateTime last_updated { get; set; }
        public string owner { get; set; }
        public List<string> projects { get; set; }
        public bool validated { get; set; }
    }


    public class Transactions
    {
        public object last_success_at { get; set; }
        public object last_fail_at { get; set; }
    }



    //public class Data
    //{
    //    public Auth auth { get; set; }
    //    public object last_transaction_date { get; set; }
    //    public string product { get; set; }
    //    public string record { get; set; }
    //    public Products products { get; set; }
    //    public Status status { get; set; }
    //}

    public class AuthCallBackResponse
    {
        public AuthCallBackResponse()
        {
            data = new Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class AuthWodgetLinkGenerateResponse
    {
        public AuthWodgetLinkGenerateResponse()
        {
            authCallBackResponse = new AuthCallBackResponse();
        }
        public AuthCallBackResponse authCallBackResponse { get; set; }
        public int RstKey { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    #endregion

    #region Balance

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Account
    {
        public string id { get; set; }
        public string nuban { get; set; }
        public string name { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public List<string> email { get; set; }
        public List<string> phone { get; set; }
    }

    public class Balance
    {
        public Balance()
        {
            account = new Account();
            customer = new Customer();
        }
        public Account account { get; set; }
        public int available_balance { get; set; }
        public string currency { get; set; }
        public Customer customer { get; set; }
        public int ledger_balance { get; set; }
        public string id { get; set; }
    }

    public class Balance2
    {
        public Balance2()
        {
            account = new Account();
            customer = new Customer();
        }
        public Account account { get; set; }
        public int available_balance { get; set; }
        public string currency { get; set; }
        public Customer customer { get; set; }
        public int ledger_balance { get; set; }
        public string id { get; set; }
        public bool? connected { get; set; }
    }

    public class BalanceCallBackResponse
    {
        public BalanceCallBackResponse()
        {
            data = new Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class BalanceWodgetLinkGenerateResponse
    {
        public BalanceWodgetLinkGenerateResponse()
        {
           // balanceCallBackResponse = new BalanceCallBackResponse();
        }
       // public BalanceCallBackResponse balanceCallBackResponse { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string Balance { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        public string ImageUrl { get; set; }

    }
    #endregion

    #region identity




    public class OkraIdentityResponseModel
    {
        public OkraIdentityResponseModel()
        {
            data = new Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class IdentityWodgetLinkGenerateResponse
    {
        public IdentityWodgetLinkGenerateResponse()
        {
            okraIdentityResponseModel = new OkraIdentityResponseModel();
        }
        public OkraIdentityResponseModel okraIdentityResponseModel { get; set; }
        public int RstKey { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    #endregion

    #region Transactions

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Notes
    {
        public string desc { get; set; }
        public List<string> topics { get; set; }
        public List<string> places { get; set; }
        public List<string> people { get; set; }
        public List<string> actions { get; set; }
        public List<string> subjects { get; set; }
        public List<string> prepositions { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double @long { get; set; }
        public string raw { get; set; }
    }

    public class Ner
    {
        public string beneficiaries { get; set; }
        public List<string> account { get; set; }
    }

    public class Transaction
    {
        public Transaction()
        {
            notes = new Notes();
            ner = new Ner();
            location = new Location();
        }
        public string _id { get; set; }
        public string bank { get; set; }
        public DateTime cleared_date { get; set; }
        public int? credit { get; set; }
        public string customer { get; set; }
        public int? debit { get; set; }
        public Notes notes { get; set; }
        public object @ref { get; set; }
        public DateTime trans_date { get; set; }
        public string account { get; set; }
        public object balance { get; set; }
        public string branch { get; set; }
        public string code { get; set; }
        public DateTime created_at { get; set; }
        public object currency { get; set; }
        public string env { get; set; }
        public DateTime last_updated { get; set; }
        public Location location { get; set; }
        public Ner ner { get; set; }
        public List<string> owner { get; set; }
        public List<string> record { get; set; }
        public string unformatted_cleared_date { get; set; }
        public string unformatted_trans_date { get; set; }
        public List<string> projects { get; set; }
        public List<string> fetched { get; set; }
    }




    public class OkraTransactionsResponseModel
    {
        public OkraTransactionsResponseModel()
        {
            data = new Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class TransactionsWodgetLinkGenerateResponse
    {
        public TransactionsWodgetLinkGenerateResponse()
        {
            okraTransactionsResponseModel = new OkraTransactionsResponseModel();
        }
        public OkraTransactionsResponseModel okraTransactionsResponseModel { get; set; }
        public int RstKey { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    #endregion
}
