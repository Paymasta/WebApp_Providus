using PayMasta.ViewModel.DataRechargeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ZealvendBillsVM
{
    public class Datum
    {
        public string network { get; set; }
        public string validity { get; set; }
        public string bundle { get; set; }
        public string price { get; set; }
    }

    public class DataResponse
    {
        public DataResponse()
        {
            data = new List<Datum>();
        }
        public string status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class GetDataResponse
    {
        public GetDataResponse()
        {
            dataResponse = new DataResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public DataResponse dataResponse { get; set; }
    }

    public class DataPaymentRequest
    {
        public string network { get; set; }
        public string bundle { get; set; }
        public string number { get; set; }
        public string referrence { get; set; }
    }
    public class Data
    {
        public string number { get; set; }
        public string referrence { get; set; }
        public string network { get; set; }
        public int price { get; set; }
        public string bundle { get; set; }
        public int megabytes { get; set; }
        public string status { get; set; }
        public int user_id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
        public int wallet_id { get; set; }
    }

    public class DataPaymentResponse
    {
        public DataPaymentResponse()
        {
            data = new Data();
        }
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class GetZealvendDataRechargeResponse
    {
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
