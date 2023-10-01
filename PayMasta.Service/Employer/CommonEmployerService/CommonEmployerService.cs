using PayMasta.Repository.Employer.CommonEmployerRepository;
using PayMasta.Utilities;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.CommonEmployerService
{
    public class CommonEmployerService : ICommonEmployerService
    {
        private readonly ICommonEmployerRepository _commonEmployerRepository;
        public CommonEmployerService()
        {
            _commonEmployerRepository = new CommonEmployerRepository();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<GetEmployerResponse> GetEmployerList()
        {
            var result = new GetEmployerResponse();
            var employerResponses = new List<EmployerResponse>();
            try
            {
                employerResponses = await _commonEmployerRepository.GetEmployerList(true);
                if (employerResponses.Count > 0)
                {
                    result.employerResponses = employerResponses;
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<GetNonRegisterEmployerResponse> GetNonRegisteredEmployerList(string searchText="")
        {
            var result = new GetNonRegisterEmployerResponse();
            var employerResponses = new List<NonRegisterEmployerResponse>();
            try
            {
                employerResponses = await _commonEmployerRepository.GetNonRegisteredEmployerList(searchText);
                if (employerResponses.Count > 0)
                {
                    result.nonRegisterEmployerResponses = employerResponses;
                    result.RstKey = 1;
                    result.Status = true;
                }
                else
                {
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
