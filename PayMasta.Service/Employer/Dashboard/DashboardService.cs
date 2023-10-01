using PayMasta.Repository.Employer.Dashboard;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IEmployeesRepository _employeesRepository;
        public DashboardService()
        {
            _dashboardRepository = new DashboardRepository();
            _employeesRepository = new EmployeesRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<DashboardTabResponse> GetDashboardData(DashboardRequest request)
        {
            var res = new DashboardTabResponse();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardData(result.Id, request.FromDate, request.ToDate, request.Month);
                if (dashboardData != null)
                {
                    res.dashboard = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<PendingEWRResponse> GetDashboardEWARequestData(PendingEWRRequest request)
        {
            var res = new PendingEWRResponse();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardEWARequestData(result.Id);
                if (dashboardData.Count>0)
                {
                    res.pendingEWRRequestResponse = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    res.getDashboardGraphResponse = dashboardData;
                    //res.getDashboardMonthlyGraphResponses=
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }


        public async Task<GetDashboardGraph> GetDashboardMonthlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardMonthlyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardWeeklyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardYearlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardYearlyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<GetDashboardPayCycle> GetDashboardPayCycleData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardPayCycle();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var dashboardData = await _dashboardRepository.GetDashboardPayCycleData(result.Id);
                if (dashboardData != null)
                {
                    res.getDashboardPayCycleResponse = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
