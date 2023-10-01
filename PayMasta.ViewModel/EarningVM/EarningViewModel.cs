using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.EarningVM
{
    public class EarningRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
    }
    public class EarningResponse
    {
        public long UserId { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AccessedAmount { get; set; }
        public decimal AvailableAmount { get; set; }

        public int RstKey { get; set; }
        public string PayCycleFrom { get; set; }
        public string PayCycleTo { get; set; }
    }
    public class EarningResponseForWeb
    {
        public long UserId { get; set; }
        public string EarnedAmount { get; set; }
        public string AccessedAmount { get; set; }
        public string AvailableAmount { get; set; }

        public int RstKey { get; set; }
        public string PayCycleFrom { get; set; }
        public string PayCycleTo { get; set; }
    }
    public class GetTransactionHistoryRequest
    {
        public GetTransactionHistoryRequest()
        {
            ServiceCategoryId = 0;
            Month = 0;
            PageNumber = 1;
            PageSize = 10;
        }
        [Required]
        public Guid UserGuid { get; set; }
        public int ServiceCategoryId { get; set; }
        public int Month { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetTodayTransactionHistoryRequest
    {

        [Required]
        public Guid UserGuid { get; set; }
    }
    public class GetTransactionHistoryResponse
    {
        public Guid Guid { get; set; }
        public long WalletTransactionId { get; set; }
        public string TotalAmount { get; set; }
        public long SenderId { get; set; }
        public string AccountNo { get; set; }
        public string TransactionId { get; set; }
        public string Comments { get; set; }
        public string InvoiceNo { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        public string TransactionTypeInfo { get; set; }
        public string BankBranchCode { get; set; }
        public string BankTransactionId { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string CreatedAt { get; set; }
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
    }
    public class TransactionHistoryResponse
    {
        public TransactionHistoryResponse()
        {
            getTransactionHistoryResponse = new List<GetTransactionHistoryResponse>();
        }
        public List<GetTransactionHistoryResponse> getTransactionHistoryResponse { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class GetTodayTransactionHistoryResponse
    {
        public Guid Guid { get; set; }
        public long WalletTransactionId { get; set; }
        public string TotalAmount { get; set; }
        //public long SenderId { get; set; }
        public string AccountNo { get; set; }
        public string TransactionId { get; set; }
        //public string Comments { get; set; }
        public string InvoiceNo { get; set; }
        //public string ReferenceId { get; set; }
        //public string TransactionType { get; set; }
        //public string TransactionTypeInfo { get; set; }
        //public string BankBranchCode { get; set; }
        //public string BankTransactionId { get; set; }
        public string Description { get; set; }

        public string CategoryName { get; set; }
        public string CreatedAt { get; set; }

    }
    public class TodayTransactionHistoryResponse
    {
        public TodayTransactionHistoryResponse()
        {
            getTodayTransactionHistoryResponses = new List<GetTodayTransactionHistoryResponse>();
        }
        public List<GetTodayTransactionHistoryResponse> getTodayTransactionHistoryResponses { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class EarningMasterResponse
    {
        public long UserId { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AccessedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public string PayCycleFrom { get; set; }
        public string PayCycleTo { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
    public class AccessdAmountRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
    public class AccessdAmountPercentageResponse
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public decimal AccessedPercentage { get; set; }
    }
    public class WebAccessdAmountRequest
    {
        [Required]
        public string UserGuid { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
    public class AccessAmountResponse
    {

        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }

    public class UpComingBillsResponse
    {
        public UpComingBillsResponse()
        {
            upComingBills = new List<UpComingBills>();
        }
        public List<UpComingBills> upComingBills { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public decimal PayableAmount { get; set; }
    }
    public class TransactionResponse
    {
        public TransactionResponse()
        {
            getTransactionResponse = new GetTransactionResponse();
        }
        public GetTransactionResponse getTransactionResponse { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public decimal PayableAmount { get; set; }
        public string DebitFrom { get; set; }
    }

    public class GetTransactionResponse
    {
        public long WalletTransactionId { get; set; }
        public string TotalAmount { get; set; }
        public int CommisionId { get; set; }
        public string CommisionAmount { get; set; }
        public string WalletAmount { get; set; }
        public decimal ServiceTaxRate { get; set; }
        public string ServiceTax { get; set; }
        public int ServiceCategoryId { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string AccountNo { get; set; }
        public string TransactionId { get; set; }
        public bool IsAdminTransaction { get; set; }
        public string Comments { get; set; }
        public string InvoiceNo { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        public string TransactionTypeInfo { get; set; }
        public bool IsBankTransaction { get; set; }
        public string BankBranchCode { get; set; }
        public string BankTransactionId { get; set; }
        public string VoucherCode { get; set; }
        public decimal FlatCharges { get; set; }
        public decimal BenchmarkCharges { get; set; }
        public decimal CommisionPercent { get; set; }
        public string DisplayContent { get; set; }
        public bool IsUpcomingBillShow { get; set; }
        public int SubCategoryId { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
      
    }
    public class UpComingBills
    {
        public int OverDue { get; set; }
        public long WalletTransactionId { get; set; }
        public Guid Guid { get; set; }
        public string TotalAmount { get; set; }
        public string AccountNo { get; set; }
        public string LastRecharge { get; set; }
        public long SenderId { get; set; }
        public string ServiceName { get; set; }
        public int SubCategoryId { get; set; }
        public string BankCode { get; set; }
        public string BillerName { get; set; }
        public string RechargeOn { get; set; }
        public string TransactionId { get; set; }
        public bool IsUpcomingBillShow { get; set; }
        public string ReferenceId { get; set; }
    }

    public class UpComingBillsRequest
    {
        [Required]
        public Guid UserGuid { get; set; }

    }

    public class RemoveUpComingBillsRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        public long WalletTransactionId { get; set; }

        public bool IsRemove { get; set; }
    }

    public class RemoveUpComingBillsResponse
    {

        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class GetUserDashboardGraphRequest
    {
        public GetUserDashboardGraphRequest()
        {
            LastWeak = -1;
        }
        public int LastWeak { get; set; }
        public Guid EmployeeGuid { get; set; }
    }

    public class EmployerResponse
    {
        [Required]
        public long UserId { get; set; }
        public long EmployerId { get; set; }
        public int Status { get; set; }
        [Required]
        public string FirstName { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
    }

    public class GetCommissionResponse
    {
        public GetCommissionResponse()
        {
            getCommissions = new List<GetCommission>();
        }
        public List<GetCommission> getCommissions { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public decimal PayableAmount { get; set; }
    }
    public class GetCommission
    {
        public int AmountFrom { get; set; }
        public int AmountTo { get; set; }
        public decimal CommisionPercent { get; set; }
        public decimal FlatCharges { get; set; }
        public decimal BenchmarkCharges { get; set; }

    }
}
