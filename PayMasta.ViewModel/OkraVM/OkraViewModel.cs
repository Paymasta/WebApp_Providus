using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.OkraVM
{

    public class OkraWodgetLinkGenerateRequest
    {
        public string name { get; set; }
        public string logo { get; set; }
        public List<string> countries { get; set; }
        public List<string> billable_products { get; set; }
        public string color { get; set; }
        public string support_email { get; set; }
        public string success_url { get; set; }
        public string callback_url { get; set; }
        public string continue_cta { get; set; }

        public options options { get; set; }
    }
    public class options
    {
        public Guid UserGuid { get; set; }
        public string UserEmail { get; set; }
        public long UserId { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Filter
    {
        public List<object> banks { get; set; }
        public List<object> corp_banks { get; set; }
        public List<object> ind_banks { get; set; }
    }

    public class Owner
    {
        public string _id { get; set; }
        public string name { get; set; }
    }

    public class App
    {
        public string _id { get; set; }
        public string tracking_id { get; set; }
    }

    public class Link
    {
        public Link()
        {
            filter = new Filter();
            owner = new Owner();
            app = new App();
        }
        public Filter filter { get; set; }
        public string currency { get; set; }
        public List<string> countries { get; set; }
        public int count { get; set; }
        public List<object> unique_views { get; set; }
        public List<string> billable_products { get; set; }
        public bool corporate { get; set; }
        public int limit { get; set; }
        public bool archived { get; set; }
        public bool debitLater { get; set; }
        public bool vanity { get; set; }
        public string link_type { get; set; }
        public bool ignore { get; set; }
        public bool multi_account { get; set; }
        public List<object> institutions { get; set; }
        public List<object> projects { get; set; }
        public string _id { get; set; }
        public string short_url { get; set; }
        public Owner owner { get; set; }
        public object lead { get; set; }
        public object success_title { get; set; }
        public object success_message { get; set; }
        public object widget_success { get; set; }
        public object widget_failed { get; set; }
        public string callback_url { get; set; }
        public string name { get; set; }
        public string env { get; set; }
        public string logo { get; set; }
        public string color { get; set; }
        public object connectMessage { get; set; }
        public object fullMessage { get; set; }
        public string mode { get; set; }
        public string continue_cta { get; set; }
        public App app { get; set; }
        public bool reauth { get; set; }
        public string current_project { get; set; }
        public List<object> past_refs { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_updated { get; set; }
        public int __v { get; set; }
        public string url { get; set; }
    }

    public class Data
    {//-------
        public Data()
        {
            link = new Link();
            //income = new Income();
            //products = new Products();
            //status = new Status();

        }
        //public Income income { get; set; }
        //public object last_transaction_date { get; set; }
        //public string product { get; set; }
        //public string record { get; set; }
        //public Products products { get; set; }
        //public Status status { get; set; }
        public Link link { get; set; }
    }

    public class OkraWodgetLinkGenerateResponse
    {
        public OkraWodgetLinkGenerateResponse()
        {
            data = new PayMasta.ViewModel.OkraVM.Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public PayMasta.ViewModel.OkraVM.Data data { get; set; }
    }

    public class WodgetLinkGenerateResponse
    {
        public WodgetLinkGenerateResponse()
        {

        }
        public int RstKey { get; set; }
        public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class WodgetLinkGenerateRequest
    {
        public Guid UserGuid { get; set; }
        public string BankCodeOrBankId { get; set; }
    }
    //-----------------------------------------------------

    public class LinkedOrUnlinkedBankRequest
    {
        public Guid UserGuid { get; set; }

    }

    public class LinkedOrUnlinkedBank
    {
        public long UserId { get; set; }

        public string BankName { get; set; }
        public string CallBackType { get; set; }
        public string CallBackUrl { get; set; }
        public string BankCodeOrBankId { get; set; }
        public bool IsLinked { get; set; }

        public string ImageUrl { get; set; }
    }

    public class LinkedOrUnlinkedBankResponse
    {
        public LinkedOrUnlinkedBankResponse()
        {
            linkedOrUnlinkedBanks = new List<LinkedOrUnlinkedBank>();
        }
        public int RstKey { get; set; }
        // public string UrlLink { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public List<LinkedOrUnlinkedBank> linkedOrUnlinkedBanks { get; set; }
    }
}
