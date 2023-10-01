using PayMasta.DBEntity.AccessAmountRequest;
using PayMasta.DBEntity.CommisionMaster;
using PayMasta.DBEntity.Earning;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Earning
{
    public interface IEarningRepository
    {
        Task<EarningMasterResponse> GetEarnings(long userId, IDbConnection exdbConnection = null);
        Task<EarningMasterResponse> GetPayCycleDate(long userId, IDbConnection exdbConnection = null);
        Task<List<CommisionMaster>> GetCommisions(IDbConnection exdbConnection = null);
        Task<AccessdAmountPercentageResponse> GetAccessAmountPercentage(long userId, IDbConnection exdbConnection = null);
        Task<List<GetTransactionHistoryResponse>> GetTransactionHistory(long userId, int categoryId, int month, int pageNumber, int pageSize, IDbConnection exdbConnection = null);
        Task<int> InsertAccessAmountRequest(AccessAmountRequest accessAmountEntityt, IDbConnection exdbConnection = null);
        Task<List<GetTodayTransactionHistoryResponse>> GetTodaysTransactionHistory(long userId, IDbConnection exdbConnection = null);
        Task<List<UpComingBills>> GetUpcomingBillsHistory(long userId, IDbConnection exdbConnection = null);
        Task<WalletTransaction> GetTransactionsByTransactionId(long userId, long walletTransactionId, IDbConnection exdbConnection = null);
        Task<int> UpdateTransactionStatusForUpcoingBills(WalletTransaction walletTransaction, IDbConnection exdbConnection = null);
        Task<GetDashboardGraphResponse> GetDashboardGraphData(long userId, int week, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardMonthlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardYearlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardWeeklyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
        Task<GetTransactionResponse> GetTransactionsByTransactionIdAndUserId(long userId, long walletTransactionId, IDbConnection exdbConnection = null);
        Task<EmployerResponse> GetEmployerDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<AccessAmountRequest> IsOldEwaRequestPending(long userId, IDbConnection exdbConnection = null);
        Task<List<GetCommission>> GetCommisionsList(IDbConnection exdbConnection = null);
        Task<List<AddedBanListResponse>> GetAddedBankList(long userId, IDbConnection exdbConnection = null);
    }
}
