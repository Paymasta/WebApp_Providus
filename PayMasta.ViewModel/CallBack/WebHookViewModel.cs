using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.CallBack
{

    public class WidgetResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class Options
    {
        public string UserEmail { get; set; }
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
    }

    public class Meta
    {
    }

    public class Extras
    {
    }

    public class IncomeWidgetResponse
    {
        public IncomeWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public string accountId { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public string current_project { get; set; }
        public string token { get; set; }
        public string owner { get; set; }
    }


    public class TransactionWidgetResponse
    {
        public TransactionWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public string accountId { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public int newTransactions { get; set; }
        public string nuban { get; set; }
        public string token { get; set; }
        public string current_project { get; set; }
        public string owner { get; set; }
    }

    public class Account
    {
        public string _id { get; set; }
        public string bank { get; set; }
        public string env { get; set; }
        public string nuban { get; set; }
        public DateTime created_at { get; set; }
        public string customer { get; set; }
        public DateTime last_updated { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string currency { get; set; }
    }

    public class AccountWidgetResponse
    {
        public AccountWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
            account = new Account();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public string accountId { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public string nuban { get; set; }
        public Account account { get; set; }
        public string current_project { get; set; }
        public string token { get; set; }
        public string owner { get; set; }
    }


    public class Periodic
    {
        public List<object> available_balance { get; set; }
        public List<object> ledger_balance { get; set; }
    }

    public class Balance
    {
        public Periodic periodic { get; set; }
        public List<object> connected { get; set; }
        public List<string> owner { get; set; }
        public List<string> record { get; set; }
        public List<string> projects { get; set; }
        public string _id { get; set; }
        public string account { get; set; }
        public int __v { get; set; }
        public int available_balance { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string customer { get; set; }
        public string env { get; set; }
        public DateTime last_updated { get; set; }
        public int ledger_balance { get; set; }
    }

    public class BalanceWidgetResponse
    {
        public BalanceWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
            balance = new Balance();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public string accountId { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public string nuban { get; set; }
        public Balance balance { get; set; }
        public string current_project { get; set; }
        public string token { get; set; }
        public string owner { get; set; }
    }



    public class PhotoId
    {
        public string url { get; set; }
        public string image_type { get; set; }
    }

    public class Identity
    {
        public Identity()
        {
            photo_id = new List<PhotoId>();
        }
        public string fullname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string id { get; set; }
        public string bvn { get; set; }
        public string nin { get; set; }
        public int score { get; set; }
        public string env { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_updated { get; set; }
        public List<string> aliases { get; set; }
        public string customer { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public List<string> phone { get; set; }
        public List<string> email { get; set; }
        public List<string> address { get; set; }
        public bool verified { get; set; }
        public List<PhotoId> photo_id { get; set; }
        public string status { get; set; }
        public string verification_country { get; set; }
        public string lga_of_origin { get; set; }
        public string lga_of_residenc { get; set; }
        public string marital_status { get; set; }
        public string nationality { get; set; }
        public string state_of_origin { get; set; }
        public string state_of_residence { get; set; }
        public object credentials { get; set; }
        public string bank { get; set; }
        public List<string> projects { get; set; }
    }

    public class IdentityWidgetResponse
    {
        public IdentityWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
            identity = new Identity();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public Identity identity { get; set; }
        public string token { get; set; }
        public string current_project { get; set; }
        public string owner { get; set; }
    }


    //public class Data
    //{
    //}

    public class Auth
    {
        public string clientId { get; set; }
        public string type { get; set; }
        public bool status { get; set; }
        public bool reauth { get; set; }
        public string record { get; set; }
        //public PayMasta.ViewModel.CallBack.Data data { get; set; }
    }

    public class AuthWidgetResponse
    {
        public AuthWidgetResponse()
        {
            meta = new Meta();
            extras = new Extras();
            options = new Options();
            auth = new PayMasta.ViewModel.CallBack.Auth();
        }
        public string method { get; set; }
        public string callback_type { get; set; }
        public string callback_code { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string callbackURL { get; set; }
        public string env { get; set; }
        public string status { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public string message { get; set; }
        public object meta_responses { get; set; }
        public Options options { get; set; }
        public Meta meta { get; set; }
        public string bankName { get; set; }
        public string bankType { get; set; }
        public string bankId { get; set; }
        public string bankSlug { get; set; }
        public string record { get; set; }
        public string recordId { get; set; }
        public string callback_url { get; set; }
        public string login_type { get; set; }
        public string customerId { get; set; }
        public List<string> customerEmail { get; set; }
        public string country { get; set; }
        public Extras extras { get; set; }
        public PayMasta.ViewModel.CallBack.Auth auth { get; set; }
        public string current_project { get; set; }
        public string token { get; set; }
        public string owner { get; set; }
    }
}
