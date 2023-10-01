using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Employer.Dashboard
{
    public class DashboardResponse
    {
        public int TotalEmployees { get; set; }
        public int TotalActiveEmployees { get; set; }
        public int TotalWithdrawRequest { get; set; }
        public int TotalPendingWithdrawRequest { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalWithdrawlValue { get; set; }

    }
    public class DashboardRequest
    {
        public Guid EmployerGuid { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int Month { get; set; }
    }
    public class DashboardTabResponse
    {
        public DashboardTabResponse()
        {
            dashboard = new DashboardResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public DashboardResponse dashboard { get; set; }
    }

    public class PendingEWRRequestResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccessAmount { get; set; }
        public string CreatedAt { get; set; }
        public long UserId { get; set; }
        public long Id { get; set; }

    }
    public class PendingEWRRequest
    {
        public Guid EmployerGuid { get; set; }
    }
    public class PendingEWRResponse
    {
        public PendingEWRResponse()
        {
            pendingEWRRequestResponse = new List<PendingEWRRequestResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<PendingEWRRequestResponse> pendingEWRRequestResponse { get; set; }
    }

    public class GetDashboardGraphResponse
    {
        public int SundayData { get; set; }
        public int MondayData { get; set; }
        public int TuesdayData { get; set; }
        public int WednesdayData { get; set; }
        public int ThursdayData { get; set; }
        public int FridayData { get; set; }
        public int SaturdayData { get; set; }

    }
    public class GetDashboardMonthlyGraphResponse
    {
        public string DataName { get; set; }
        public int Total { get; set; }

    }
    public class GetDashboardGraphRequest
    {
        public GetDashboardGraphRequest()
        {
            LastWeak = -1;
        }
        public int LastWeak { get; set; }
        public Guid EmployerGuid { get; set; }
    }

    public class GetDashboardGraph
    {
        public GetDashboardGraph()
        {
            getDashboardGraphResponse = new GetDashboardGraphResponse();
            getDashboardMonthlyGraphResponses = new List<GetDashboardMonthlyGraphResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public GetDashboardGraphResponse getDashboardGraphResponse { get; set; }
        public List<GetDashboardMonthlyGraphResponse> getDashboardMonthlyGraphResponses { get; set; }
    }
    public class GetDashboardPayCycle
    {
        public GetDashboardPayCycle()
        {
            getDashboardPayCycleResponse = new GetDashboardPayCycleResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public GetDashboardPayCycleResponse getDashboardPayCycleResponse { get; set; }
    }

    public class GetDashboardPayCycleResponse
    {
        public string PayCycleFrom { get; set; }
        public string PayCycleTo { get; set; }


    }
    public class GetAddedBanListResponse
    {
        public GetAddedBanListResponse()
        {
            addedBanListResponses = new List<AddedBanListResponse>();

        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public List<AddedBanListResponse> addedBanListResponses { get; set; }
    }
    public class AddedBanListResponse
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

    }
}
