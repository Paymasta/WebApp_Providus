using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardTabResponse> GetDashboardData(DashboardRequest request);
        Task<PendingEWRResponse> GetDashboardEWARequestData(PendingEWRRequest request);
        Task<GetDashboardGraph> GetDashboardGraphData(GetDashboardGraphRequest request);
        Task<GetDashboardPayCycle> GetDashboardPayCycleData(GetDashboardGraphRequest request);
        Task<GetDashboardGraph> GetDashboardMonthlyGraphData(GetDashboardGraphRequest request);
        Task<GetDashboardGraph> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request);
        Task<GetDashboardGraph> GetDashboardYearlyGraphData(GetDashboardGraphRequest request);
    }
}
