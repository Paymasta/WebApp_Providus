using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ValidatInternetVM
{
    public class ValidateInternetRequest
    {
        public string service { get; set; }
        public string channel { get; set; }
        public string type { get; set; }
        public string account { get; set; }
    }
    public class Data
    {
        public bool error { get; set; }
        public string message { get; set; }
        public string customerName { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
        public string productCode { get; set; }
    }

    public class ValidateInternetResponse
    {
        public ValidateInternetResponse()
        {
            data=new Data();
        }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object metadata { get; set; }
    }
    public class GetValidateInternetResponse
    {
        public GetValidateInternetResponse()
        {
            validateInternetResponse = new ValidateInternetResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public ValidateInternetResponse validateInternetResponse { get; set; }
    }

    public class GetInternetValidateRequest
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
    }
}
