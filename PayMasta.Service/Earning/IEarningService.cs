using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Earning
{
    public interface IEarningService
    {
        Task<EarningResponseForWeb> GetEarnings(EarningRequest request);
        Task<TransactionHistoryResponse> GetTransactionHistory(GetTransactionHistoryRequest request);
        Task<AccessAmountResponse> AccessAmountRequest(AccessdAmountRequest request);
        Task<TodayTransactionHistoryResponse> GetTodaysTransactionHistory(GetTodayTransactionHistoryRequest request);
        Task<UpComingBillsResponse> GetUpcomingBillsHistory(UpComingBillsRequest request);
        Task<RemoveUpComingBillsResponse> RemoveBillfromUpcomingBilsList(RemoveUpComingBillsRequest request);
        Task<GetDashboardGraph> GetDashboardGraphData(GetUserDashboardGraphRequest request);
        Task<GetDashboardGraph> Invite(string email, Guid EmployeeGuid);
        Task<GetDashboardGraph> GetDashboardMonthlyGraphData(GetDashboardGraphRequest request);
        Task<GetDashboardGraph> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request);
        Task<GetDashboardGraph> GetDashboardYearlyGraphData(GetDashboardGraphRequest request);
        Task<TransactionResponse> GetTransactionByWalletTransactionId(RemoveUpComingBillsRequest request);
        Task<GetCommissionResponse> GetCommisions();
        Task<GetAddedBanListResponse> GetAddedBankList(Guid userGuid);
        //Task<EarningResponseForWeb> GetEarningsForWeb(EarningRequest request);
    }
}
