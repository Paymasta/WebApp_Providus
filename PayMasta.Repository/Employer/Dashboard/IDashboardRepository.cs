using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Employer.Dashboard
{
    public interface IDashboardRepository
    {
        Task<DashboardResponse> GetDashboardData(long userId, DateTime? fromDate = null, DateTime? toDate = null, int month = 0, IDbConnection exdbConnection = null);
        Task<List<PendingEWRRequestResponse>> GetDashboardEWARequestData(long userId, IDbConnection exdbConnection = null);
        Task<GetDashboardGraphResponse> GetDashboardGraphData(long userId, int week, IDbConnection exdbConnection = null);
        Task<GetDashboardPayCycleResponse> GetDashboardPayCycleData(long userId, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardMonthlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardYearlyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
        Task<List<GetDashboardMonthlyGraphResponse>> GetDashboardWeeklyGraphData(long userId, int filterDate, IDbConnection exdbConnection = null);
    }
}
