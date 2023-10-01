using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Employer.EmployeesVM
{
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
        public string CreatedAt { get; set; }
        public bool IsverifiedByEmployer { get; set; }
    }
    public class GetEmployeesListForTransactionRequest
    {
        public GetEmployeesListForTransactionRequest()
        {
            ToDate = null;
            FromDate = null;
            SearchTest = "";
            pageNumber = 1;
            PageSize = 10;
            month = 0;
        }
        public string userGuid { get; set; }
        public string SearchTest { get; set; }
        public int pageNumber { get; set; }

        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int month { get; set; }
    }
    public class EmployeesListForTransactionsReponse
    {
        public EmployeesListForTransactionsReponse()
        {
            employeesListForTransactions = new List<EmployeesListForTransactions>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesListForTransactions> employeesListForTransactions { get; set; }
    }
    public class EmployeesListForTransactions
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
        public string EmployerName { get; set; }
        public string CreatedAt { get; set; }
        public string StatusId { get; set; }
    }
    public class GetEmployerDetailResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int UserType { get; set; }
        public string OrganisationName { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public string Email { get; set; }
    }
    public class EmployeesReponse
    {
        public EmployeesReponse()
        {
            employeesListViewModel = new List<EmployeesListViewModel>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<EmployeesListViewModel> employeesListViewModel { get; set; }
    }
    public class GetEmployeesListRequest
    {
        public GetEmployeesListRequest()
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
    public class BlockUnBlockEmployeeRequest
    {
        public string EmployeeUserGuid { get; set; }
        public string EmployeerUserGuid { get; set; }
        [Range(1, 2)]
        public int DeleteOrBlock { get; set; }
    }
    public class DeleteEmployeeRequest
    {
        public string EmployeeUserGuid { get; set; }
        [Range(1, 2)]
        public int DeleteOrBlock { get; set; }
    }


    public class BlockUnBlockEmployeeResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class ViewEmployeesProfileRequest
    {
        public string EmployeeUserGuid { get; set; }

    }
    public class ViewEmployeesProfileResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public int Status { get; set; }
        public string CountryName { get; set; }
        public string Gender { get; set; }
        public string StaffId { get; set; }
        public decimal NetPay { get; set; }
        public decimal GrossPay { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsUserVerified { get; set; }
    }

    public class UpdateEmployeeNetAndGrossPayRequest
    {
        public string EmployeeUserGuid { get; set; }
        public string EmployeerUserGuid { get; set; }
        public decimal NetPay { get; set; }
        public decimal GrossPay { get; set; }
    }

    public class ApproveUserProfileRequest
    {
        public string UserGuid { get; set; }
        public string EmployerGuid { get; set; }
    }
    public class UpdateEmployeeNetAndGrossPayResponse
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
    }
    public class BulkUploadRecords
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public int Sucess { get; set; }
        public int Error { get; set; }
    }

}
