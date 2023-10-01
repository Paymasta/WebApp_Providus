using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Employer.EWAVM
{
    public class AccessAmountViewModel
    {
        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployerName { get; set; }
        public long EmployerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }

        public decimal AccessAmount { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public string CreatedAt { get; set; }
        public string AdminStatus { get; set; }
        public int AdminStatusId { get; set; }
        public string StaffId { get; set; }
        public string IsPaidToPayMasta { get; set; }
        public bool IsPaidToPayMastaId { get; set; }

        public string CommissionCharge { get; set; }
        public string TotalAmountWithCommission { get; set; }
    }
    public class AccessAmountViewModelResponse
    {
        public AccessAmountViewModelResponse()
        {
            accessAmountViewModels = new List<AccessAmountViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<AccessAmountViewModel> accessAmountViewModels { get; set; }

        public bool isInvoiceDownload { get; set; }
    }
    public class PayToPayMastaRequest
    {
        public PayToPayMastaRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = -1;
        }
        public string userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }

    public class PayableAmount
    {
        public decimal TotalAmount { get; set; }
        public int TotalUser { get; set; }
        public decimal TotalFee { get; set; }
    }
    public class PayableAmountResponse
    {
        public PayableAmountResponse()
        {
            payableAmount = new PayableAmount();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public PayableAmount payableAmount { get; set; }
    }


}
