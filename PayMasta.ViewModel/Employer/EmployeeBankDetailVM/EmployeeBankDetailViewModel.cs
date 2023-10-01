using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Employer.EmployeeBankDetailVM
{
    public class BankDetailResponse
    {
        public long UserId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string BankAccountHolderName { get; set; }
        public string CustomerId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSalaryAccount { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
    }
    public class EmployeesBankReponse
    {
        public EmployeesBankReponse()
        {
            employeesListViewModel = new List<BankDetailResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<BankDetailResponse> employeesListViewModel { get; set; }
    }

    public class GetEmployeesBankListRequest
    {
        public GetEmployeesBankListRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            Status = -1;
        }
        public string EmployerGuid { get; set; }
        public string UserGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }
    public class SetSalaryAccountRequest
    {
        public long UserId { get; set; }
        public long BankDetailId { get; set; }
        public Guid EmployerGuid { get; set; }
    }

    public class SetSalaryAccountReponse
    {
        public SetSalaryAccountReponse()
        {
            this.RstKey = 0;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

    }
}
