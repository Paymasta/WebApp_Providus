using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.LidyaVM
{
    #region Auth 

    public class LidyaAuthResponse
    {
        public string Data { get; set; }
        public string Token { get; set; }
    }
    public class LidyaAuth
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class Access
    {
        public object key { get; set; }
        public int country_id { get; set; }
        public string range_id { get; set; }
    }

    public class Onboard
    {
        public object current_step { get; set; }
        public object steps { get; set; }
    }

    public class Role
    {
        public Role()
        {
            access = new List<Access>();
        }
        public int id { get; set; }
        public string role_name { get; set; }
        public int country_id { get; set; }
        public List<Access> access { get; set; }
        public object other_countries { get; set; }
        public bool is_administrator { get; set; }
    }

    public class LidyaMandateCreateResponse
    {
        public LidyaMandateCreateResponse()
        {
            roles = new List<Role>();
            onboard = new Onboard();
        }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public object photo { get; set; }
        public int country_id { get; set; }
        public List<Role> roles { get; set; }
        public bool pass_expired { get; set; }
        public object country_date_format { get; set; }
        public object onboard_step { get; set; }
        public Onboard onboard { get; set; }
        public bool onboard_completed { get; set; }
        public string phone_number { get; set; }
        public string identification_number { get; set; }
        public string gender { get; set; }
        public DateTime birthdate { get; set; }
        public int default_enterprise_id { get; set; }
        public int phone_prefix_id { get; set; }
        public int identification_type_id { get; set; }
        public bool phone_number_locked { get; set; }
        public bool auto_sign { get; set; }
        public List<object> list_sort_preferences { get; set; }
        public object two_factor_authorization_type { get; set; }
        public bool show_compliance_notification { get; set; }
        public bool compliance_finished { get; set; }
        public object category { get; set; }
        public bool can_change_registration_type { get; set; }
    }

    #endregion

    #region Create mandate 
    public class PayerData
    {
        public string bvn { get; set; }
        public string name { get; set; }
        public int phone_prefix_id { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
    }
    public class LidyaMandateCreate
    {
        public LidyaMandateCreate()
        {
            payer_data = new PayerData();
        }
        public int enterprise_id { get; set; }
        public int amount { get; set; }
        public string type { get; set; }
        public string frequency { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public string duration { get; set; }
        public PayerData payer_data { get; set; }
    }




    #endregion

    #region API request and response
    public class MandateResponse
    {

        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        //public List<WalletServiceResponse> operatorResponse { get; set; }
        public int collect_id { get; set; }
    }

    public class MandateRequest
    {

        public string Amount { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public Guid UserGuid { get; set; }

    }
    #endregion
}
