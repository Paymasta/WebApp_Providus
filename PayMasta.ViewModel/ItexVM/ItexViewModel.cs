using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ItexVM
{

    #region get operator list
    public class AccountType
    {
        public string name { get; set; }
    }

    public class Biller
    {
        public Biller()
        {
            this.accountType = new List<AccountType>();
        }
        public string name { get; set; }
        public string service { get; set; }
        public List<AccountType> accountType { get; set; }
    }

    public class Datum
    {
        public Datum()
        {
            billers = new List<Biller>();
        }
        public string category { get; set; }
        public List<Biller> billers { get; set; }
    }

    public class ITextGetOperatorListResponse
    {
        public ITextGetOperatorListResponse()
        {
            this.data = new List<Datum>();
        }
        public string code { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public object metadata { get; set; }
    }

    public class OperatorResponse
    {
        public OperatorResponse()
        {
            operatorResponse = new List<WalletServiceResponse>();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public List<WalletServiceResponse> operatorResponse { get; set; }
    }
    public class WalletServiceResponse
    {
        public string ServiceName { get; set; }
        public int SubCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string BankCode { get; set; }
        public string HttpVerbs { get; set; }
        public string RawData { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CountryCode { get; set; }
        public string BillerName { get; set; }

        public decimal Amount { get; set; }
        public int OperatorId { get; set; }
        public string AccountType { get; set; }
    }

    public class OperatorRequest
    {
        public Guid UserGuid { get; set; }
    }
    public class GetTvProductRequest
    {
        public string Product { get; set; }
    }
    #endregion get operator list


}
