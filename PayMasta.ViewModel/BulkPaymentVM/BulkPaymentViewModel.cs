using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.BulkPaymentVM
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BulkDatum
    {
        public string country { get; set; }
        public string customer { get; set; }
        public int amount { get; set; }
        public string recurrence { get; set; }
        public string type { get; set; }
        public string reference { get; set; }
    }

    public class BulkRechargeRequest
    {
        public BulkRechargeRequest()
        {
            bulk_data = new List<BulkDatum>();
        }
        public string bulk_reference { get; set; }
        public string callback_url { get; set; }
        public List<BulkDatum> bulk_data { get; set; }
    }

    public class Data
    {
        public object batch_reference { get; set; }
    }

    public class BulkRechargeResponse
    {
        public BulkRechargeResponse()
        {
            data = new Data();
        }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class BulkPaymentRequest
    {
        public BulkPaymentRequest()
        {
            bulk_data = new List<BulkDatumForPayment>();
        }
        public Guid UserGuid { get; set; }

        public decimal TotalAmount { get; set; }
        public List<BulkDatumForPayment> bulk_data { get; set; }
    }

    public class BulkDatumForPayment
    {
        public string country { get; set; }
        public string customer { get; set; }
        public int amount { get; set; }
        public string recurrence { get; set; }
        public string type { get; set; }
        public string BillerCode { get; set; }
        public int OperatorId { get; set; }
    }
}
