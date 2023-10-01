using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Employer.EWAVM
{
    public class EmployeesReponse
    {
        public EmployeesReponse()
        {
            employeesListViewModel = new List<EmployeesListViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public bool IsEwaApprovalAccess { get; set; }
        public List<EmployeesListViewModel> employeesListViewModel { get; set; }
    }
    public class EmployeesListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public long EmployerId { get; set; }
        public string CreatedAt { get; set; }
        public decimal AccessAmount { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public int StatusId { get; set; }
        public bool IsEwaApprovalAccess { get; set; }
        public string AdminStatus { get; set; }
        public int AdminStatusId { get; set; }
    }
    public class GetEmployerListRequest
    {
        public GetEmployerListRequest()
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
        //public Guid EmployerGuid { get; set; }
    }
    public class UpdateEWAStatusRequest
    {
        public Guid UserGuid { get; set; }
        public string AccessAmountIds { get; set; }
        public int Status { get; set; }

    }
    public class UpdateEWAStatusResponse
    {
        public UpdateEWAStatusResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }


    public class EmployeeEwaDetailReponse
    {
        public EmployeeEwaDetailReponse()
        {
            employeeEwaDetail = new EmployeeEwaDetail();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public EmployeeEwaDetail employeeEwaDetail { get; set; }
    }
    public class EmployeeEwaDetail
    {
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffId { get; set; }
        public int WorkiingDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal EarnedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public string EmployerName { get; set; }
    }
    public class EmployeesEWAWithdrawlsRequest
    {
        public Guid UserGuid { get; set; }
    }
    public class EmployeesWithdrawlsRequest
    {
        public int Month { get; set; }
        public Guid UserGuid { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
    public class EmployeesWithdrawls
    {

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
        public string AdminStatus { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public string CreatedAt { get; set; }
        public decimal AccessAmount { get; set; }
        public int StatusId { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public int AdminStatusId { get; set; }
    }
    public class EmployeesWithdrawlsForApp
    {

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalCount { get; set; }
        public long RowNumber { get; set; }
        public string Status { get; set; }
        public string AdminStatus { get; set; }
        public Guid UserGuid { get; set; }
        public long UserId { get; set; }
        public string CreatedAt { get; set; }
        public decimal AccessAmount { get; set; }
        public int StatusId { get; set; }
        public long AccessAmountId { get; set; }
        public Guid AccessAmountGuid { get; set; }
        public int AdminStatusId { get; set; }
        public string EmployerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeesWithdrawlsResponse
    {
        public EmployeesWithdrawlsResponse()
        {
            employeesWithdrawls = new List<EmployeesWithdrawls>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesWithdrawls> employeesWithdrawls { get; set; }
    }
    public class EmployeesWithdrawlsResponseForApp
    {
        public EmployeesWithdrawlsResponseForApp()
        {
            employeesWithdrawls = new List<EmployeesWithdrawlsForApp>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesWithdrawlsForApp> employeesWithdrawls { get; set; }
    }
    public class DownloadLogReportRequest
    {
        public DownloadLogReportRequest()
        {
            this.DateFrom = null;
            this.DateTo = null;
            this.Month = 0;
        }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Month { get; set; }
        public Guid userGuid { get; set; }
    }

}
