using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.OkraBankVM
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Colors
    //{
    //    public string primary { get; set; }
    //    public string accent { get; set; }
    //    public string bg { get; set; }
    //    public string button { get; set; }
    //    public string icon { get; set; }
    //}

    //public class Ind
    //{
    //    public bool exists { get; set; }
    //    public bool recurring { get; set; }
    //    public bool save_bene { get; set; }
    //}

    //public class Corp
    //{
    //    public bool exists { get; set; }
    //    public bool recurring { get; set; }
    //    public bool save_bene { get; set; }
    //}

    //public class Mobile
    //{
    //    public bool exists { get; set; }
    //    public bool sso { get; set; }
    //    public bool recurring { get; set; }
    //    public bool save_bene { get; set; }
    //}

    //public class Ussd
    //{
    //    public bool exists { get; set; }
    //    public bool recurring { get; set; }
    //    public bool save_bene { get; set; }
    //}

    //public class Web
    //{
    //    public bool recurring { get; set; }
    //}

    //public class Payments
    //{
    //    public Payments()
    //    {
    //        ind = new Ind();
    //        corp = new Corp();
    //        mobile = new Mobile();
    //        ussd = new Ussd();
    //        web = new Web();
    //    }
    //    public Ind ind { get; set; }
    //    public Corp corp { get; set; }
    //    public Mobile mobile { get; set; }
    //    public Ussd ussd { get; set; }
    //    public bool secret { get; set; }
    //    public Web web { get; set; }
    //}

    //public class Bank
    //{
    //    public Bank()
    //    {
    //        colors = new Colors();
    //        payments = new Payments();
    //    }
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string slug { get; set; }
    //    public string icon { get; set; }
    //    public string v2_icon { get; set; }
    //    public string v2_logo { get; set; }
    //    public List<string> products { get; set; }
    //    public Colors colors { get; set; }
    //    public bool secret_question_or_otp { get; set; }
    //    public bool secret_question_or_otp_mobile { get; set; }
    //    public bool secret_question_or_otp_corp { get; set; }
    //    public Payments payments { get; set; }
    //    public bool ussd { get; set; }
    //    public List<string> countries { get; set; }
    //    public bool corporate { get; set; }
    //    public string status { get; set; }
    //    public bool individual { get; set; }
    //    public string sortcode { get; set; }
    //    public string alt_sortcode { get; set; }
    //    public DateTime created_at { get; set; }
    //    public DateTime last_updated { get; set; }
    //}

    //public class BankListData
    //{
    //    public BankListData()
    //    {
    //        banks = new List<Bank>();
    //    }
    //    public List<Bank> banks { get; set; }
    //    public object credentials { get; set; }
    //}

    //public class OkraBankListResponse
    //{
    //    public OkraBankListResponse()
    //    {
    //        data = new BankListData();
    //    }
    //    public string status { get; set; }
    //    public string message { get; set; }
    //    public BankListData data { get; set; }
    //}
    //------------------------------------------------------------------------------
    public class OkraBankResponse
    {
        public OkraBankResponse()
        {
            okraBankListResponse = new OkraBankListResponse();
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
        public OkraBankListResponse okraBankListResponse { get; set; }
    }

    // -------------------------------------------------------------------------new code

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class OkraBank
    {
        public OkraBank()
        {
            colors=new Colors();
            payments=new Payments();
        }
        public string id { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Colors colors { get; set; }
        public List<string> countries { get; set; }
        public string icon { get; set; }
        public bool? ussd { get; set; }
        public string status { get; set; }
        public Payments payments { get; set; }
        public bool? corporate { get; set; }
        public bool? individual { get; set; }
        public string v2_logo { get; set; }
        public string v2_icon { get; set; }
        public List<string> products { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class Colors
    {
        public string primary { get; set; }
        public string accent { get; set; }
        public string bg { get; set; }
        public string button { get; set; }
        public string icon { get; set; }
    }

    public class Corp
    {
        public bool exists { get; set; }
        public bool recurring { get; set; }
        public bool save_bene { get; set; }
    }

    public class OkraBankListData
    {
        public OkraBankListData()
        {
            banks=new List<OkraBank>();
        }
        public List<OkraBank> banks { get; set; }
        public string credentials { get; set; }
    }

    public class Ind
    {
        public bool exists { get; set; }
        public bool recurring { get; set; }
        public bool save_bene { get; set; }
    }

    public class Mobile
    {
        public bool exists { get; set; }
        public bool sso { get; set; }
        public bool recurring { get; set; }
        public bool save_bene { get; set; }
    }

    public class Payments
    {
        public Payments()
        {
            ind=new Ind();
            corp=new Corp();
            mobile=new Mobile();
            ussd=new Ussd();
            web=new Web();
        }
        public Ind ind { get; set; }
        public Corp corp { get; set; }
        public Mobile mobile { get; set; }
        public Ussd ussd { get; set; }
        public bool secret { get; set; }
        public Web web { get; set; }
    }

    public class OkraBankListResponse
    {
        public OkraBankListResponse()
        {
            data = new OkraBankListData();
        }
        public string status { get; set; }
        public string message { get; set; }
        public OkraBankListData data { get; set; }
    }

    public class Ussd
    {
        public bool exists { get; set; }
        public bool recurring { get; set; }
        public bool save_bene { get; set; }
    }

    public class Web
    {
        public bool recurring { get; set; }
    }


}
