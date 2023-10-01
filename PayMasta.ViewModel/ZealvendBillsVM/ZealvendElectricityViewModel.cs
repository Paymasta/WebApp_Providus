using PayMasta.ViewModel.ElectricityPurchaseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ZealvendBillsVM
{
    public class ElectricityRequest
    {
        public string meter_number { get; set; }
        public string disco { get; set; }
        public decimal amount { get; set; }
    }

    public class ElectricityData
    {
        public string referrence { get; set; }
        public string disco { get; set; }
        public int amount { get; set; }
        public string number { get; set; }
        public string status { get; set; }
        public int user_id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
        public int wallet_id { get; set; }
        public string token { get; set; }
    }

    public class ElectricityResponse
    {
        public ElectricityResponse()
        {
            data = new ElectricityData();
        }
        public string status { get; set; }
        public ElectricityData data { get; set; }
    }

    public class GetZealElectricityRechargeResponse
    {
        public GetZealElectricityRechargeResponse()
        {
            electricityPurchaseResponse = new ElectricityResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public ElectricityResponse electricityPurchaseResponse { get; set; }
    }

    public class MeterVerifyRequest
    {
        public string meter_number { get; set; }
        public string disco { get; set; }
    }
    public class MeterVerifyData
    {
        public bool error { get; set; }
        public string discoCode { get; set; }
        public string vendType { get; set; }
        public string meterNo { get; set; }
        public int minVendAmount { get; set; }
        public int maxVendAmount { get; set; }
        public int responseCode { get; set; }
        public int outstanding { get; set; }
        public int debtRepayment { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string tariff { get; set; }
        public string orderId { get; set; }
    }

    public class MeterVerifyResponse
    {
        public MeterVerifyResponse()
        {
            data = new MeterVerifyData();
        }
        public string status { get; set; }
        public MeterVerifyData data { get; set; }
    }


    public class VerifyRequest
    {
        public string meterNo { get; set; }
        public string disco { get; set; }
        public Guid UserGuid { get; set; }
    }

}
