using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.PlanVM
{
    #region get plan list
    public class PlanListRequest
    {
        public string service { get; set; }
        public string channel { get; set; }
    }



    public class GetPlanListResponses
    {
        public GetPlanListResponses()
        {
            planListResponse = new PlanListResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public PlanListResponse planListResponse { get; set; }
    }
    public class Datum
    {
        public string type { get; set; }
        public string code { get; set; }
        public string duration { get; set; }
        public string amount { get; set; }
        public string value { get; set; }
        public string description { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Data()
        {
            data = new List<Datum>();
        }
        public int status { get; set; }
        public List<Datum> data { get; set; }
        public string date { get; set; }
        public string responseCode { get; set; }
        public string productCode { get; set; }
        //  public string type { get; set; }
        // public string code { get; set; }
        // public string duration { get; set; }
        // public string amount { get; set; }
        // public string value { get; set; }
        // public string description { get; set; }
    }

    public class PlanListResponse
    {
        public PlanListResponse()
        {
            this.data = new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }



    public class OperatorPlanRequest
    {
        public string service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
    }
    #endregion
}
