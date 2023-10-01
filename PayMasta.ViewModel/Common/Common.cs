using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Common
{
    public class InvoiceNumberResponse
    {
        public InvoiceNumberResponse()
        {
            this.InvoiceNumber = string.Empty;
            this.AutoDigit = string.Empty;


        }
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string AutoDigit { get; set; }
        public Guid Guid { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string response_code { get; set; }
        public string response_message { get; set; }
        public string flw_ref { get; set; }
        public string order_ref { get; set; }
        public string account_number { get; set; }
        public int frequency { get; set; }
        public string bank_name { get; set; }
        public string created_at { get; set; }
        public string expiry_date { get; set; }
        public string note { get; set; }
        public string amount { get; set; }
    }

    //public class VirtualAccountNumberResponse
    //{
    //    public VirtualAccountNumberResponse()
    //    {
    //        data = new PayMasta.ViewModel.Common.Data();
    //    }

    //    public string status { get; set; }
    //    public string message { get; set; }
    //    public PayMasta.ViewModel.Common.Data data { get; set; }
    //}

    public class CategoryResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
        public int MainCategoryId { get; set; }
    }
    public class GetCategoryResponse
    {
        public GetCategoryResponse()
        {
            categoryResponse = new List<CategoryResponse>();
        }
        public List<CategoryResponse> categoryResponse { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class WalletToWalletTransferRequest
    {
        public string accountNumber { get; set; }
        public decimal amount { get; set; }
        public string channel { get; set; }
        public string sourceBankCode { get; set; }
        public string sourceAccountNumber { get; set; }
        public string destBankCode { get; set; }
        public string pin { get; set; }
        public string transRef { get; set; }
        public bool isToBeSaved { get; set; }
        public string beneficiaryName { get; set; }
    }
}
