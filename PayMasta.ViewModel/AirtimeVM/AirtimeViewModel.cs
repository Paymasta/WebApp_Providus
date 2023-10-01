using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.AirtimeVM
{
    public class Data
    {
        public string phone_number { get; set; }
        public int amount { get; set; }
        public string network { get; set; }
        public string flw_ref { get; set; }
        public string reference { get; set; }
    }

    public class AirtimeRecharge
    {
        public AirtimeRecharge()
        {
            data = new PayMasta.ViewModel.AirtimeVM.Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public PayMasta.ViewModel.AirtimeVM.Data data { get; set; }
    }




    public class AirtimeRechargeResponse
    {
        public AirtimeRechargeResponse()
        {

            airtimeRecharge = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge airtimeRecharge { get; set; }
    }

    public class TVRechargeResponse
    {
        public TVRechargeResponse()
        {

            tvResponse = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge tvResponse { get; set; }
    }

    public class InternetRechargeResponse
    {
        public InternetRechargeResponse()
        {

            internetResponse = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge internetResponse { get; set; }
    }

    public class DataBundleRechargeResponse
    {
        public DataBundleRechargeResponse()
        {

            databunldeRecharge = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge databunldeRecharge { get; set; }
    }

    public class ElectriCityBillRechargeResponse
    {
        public ElectriCityBillRechargeResponse()
        {

            electriCityBillRechargeResponse = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge electriCityBillRechargeResponse { get; set; }
    }
    public class BulkBillRechargeResponse
    {
        public BulkBillRechargeResponse()
        {

            bulkBillResponse = new AirtimeRecharge();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AirtimeRecharge bulkBillResponse { get; set; }
    }
}
