using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ZealvendBillsVM
{

    public enum EnumSubsctiptionType
    {
        SubscriptionPackage = 1,
        TopUp = 2
    }

    public class PayTvVendRequestVM
    {
        public string Service { get; set; }
        public Guid UserGuid { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountType { get; set; }
        public string Phone { get; set; }
        public string Amount { get; set; }
        public string SmartCardCode { get; set; }
        public bool RedeemBonus { get; set; }
        public double BonusAmount { get; set; }
        public string Code { get; set; }
        public string ProductCode { get; set; }
        public int SubsctiptionType { get; set; }
    }

    public class PayTvVendResponseVM
    {
        public PayTvVendResponseVM()
        {
            Data = new PayTvResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public PayTvResponse Data { get; set; }
    }

    public class PayTvVendRequest
    {
        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("referrence")]
        public string Referrence { get; set; }
    }

    public class PayTvDataModel
    {
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("referrence")]
        public string Referrence { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("wallet_id")]
        public int WalletId { get; set; }
    }

    public class PayTvResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public PayTvDataModel Data { get; set; }
    }

    public class ProductModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class PayTvProductResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("products")]
        public List<ProductModel> Products { get; set; }
    }

    public class PayTvProductResponseVM
    {
        public PayTvProductResponseVM()
        {
            Data = new PayTvProductResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public PayTvProductResponse Data { get; set; }
    }

    public class PayTvVerifyRequest
    {
        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }
    }

    public class PayTvVerifyResponseVM
    {
        public PayTvVerifyResponseVM()
        {
            Data = new PayTvVerifyResponse();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public PayTvVerifyResponse Data { get; set; }
    }

    public class PayTvVerifyData
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("customernumber")]
        public string Customernumber { get; set; }
    }

    public class PayTvVerifyResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public PayTvVerifyData Data { get; set; }
    }

    public class PayTvTopupRequest
    {
        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("referrence")]
        public string Referrence { get; set; }
    }
}
