using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ValidateMultichoiceVM
{
    public class ValidatRequest
    {
        public string service { get; set; }
        public string channel { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public string amount { get; set; }
        public string smartCardCode { get; set; }
    }

    public class Bouquet
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string code { get; set; }
    }

    public class CurrentPlan
    {
        public CurrentPlan()
        {
            items = new List<Item>();
        }
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }

    //public class Data
    //{
    //    public bool error { get; set; }
    //    public string @ref { get; set; }
    //    public string name { get; set; }
    //    public string unit { get; set; }
    //    public string date { get; set; }
    //    public Renew renew { get; set; }
    //    public string bouquetCode { get; set; }
    //    public string serviceId { get; set; }
    //    public string customerNumber { get; set; }
    //    public string responseCode { get; set; }
    //    public string message { get; set; }
    //    public string description { get; set; }
    //    public string account { get; set; }
    //    public string type { get; set; }
    //    public string basketId { get; set; }
    //    public List<Bouquet> bouquets { get; set; }
    //    public string productCode { get; set; }
    //}
    public class DataMultiChoice
    {
        public DataMultiChoice()
        {
            renew = new Renew();
            bouquets = new List<Bouquet>();
        }
        public bool error { get; set; }
        public string @ref { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string date { get; set; }
        public Renew renew { get; set; }
        public string bouquetCode { get; set; }
        public string serviceId { get; set; }
        public string customerNumber { get; set; }
        public string responseCode { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string account { get; set; }
        public string type { get; set; }
        public string basketId { get; set; }
        public List<Bouquet> bouquets { get; set; }
        public string productCode { get; set; }
    }
    public class Item
    {
        public string code { get; set; }
        public int price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Renew
    {
        public Renew()
        {
            currentPlan = new CurrentPlan();
        }
        public string dueDate { get; set; }
        public CurrentPlan currentPlan { get; set; }
    }

    public class ValidatResponse
    {
        public ValidatResponse()
        {
            data = new DataMultiChoice();
        }
        public string code { get; set; }
        public string message { get; set; }
        public DataMultiChoice data { get; set; }
        public object metadata { get; set; }
    }


    public class GetValidatBouquetDataRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
    }

    public class GetValidateBouquestResponses
    {
        public GetValidateBouquestResponses()
        {
            validateMeterResponse = new ValidatResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public ValidatResponse validateMeterResponse { get; set; }
    }
}
