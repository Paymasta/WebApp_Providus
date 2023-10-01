using PayMasta.Repository.Account;
using PayMasta.Repository.ManageFinance;
using PayMasta.Service.Earning;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.VirtualAccount;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.ManageFinanceVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageFinance
{
    public class ManageFinanceService : IManageFinanceService
    {
        private readonly IManageFinanceRepository _manageFinanceRepository;
        private readonly IAccountRepository _accountRepository;
        private IVirtualAccountService _virtualAccountService;
        private IEarningService _earningService;
        private IProvidusExpresssWalletService _providusExpresssWalletService;
        public ManageFinanceService()
        {
            _manageFinanceRepository = new ManageFinanceRepository();
            _accountRepository = new AccountRepository();
            _virtualAccountService = new VirtualAccountService();
            _earningService = new EarningService();
            _providusExpresssWalletService = new ProvidusExpresssWalletService();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<GetManageFinanceResponse> GetPiChartData(ManageFinanceRequest request)
        {
            var result = new GetManageFinanceResponse();
            // var billsData = new GetManageFinance();
            try
            {

                var user = await _accountRepository.GetUserByGuid(request.UserGuid);
                var balance = await _providusExpresssWalletService.GetVirtualAccount(request.UserGuid);
                var piChartData = await _manageFinanceRepository.GetPiChartData(user.Id);
                var req = new UpComingBillsRequest
                {
                    UserGuid = request.UserGuid
                };
                var okTospend = await _earningService.GetUpcomingBillsHistory(req);
                if (user != null && piChartData != null && piChartData.Bills != null)
                {

                    result.manageFinance = piChartData;
                    if (balance.status == true)
                    {
                        var finalAmt = Convert.ToDecimal(balance.wallet.availableBalance) - Convert.ToDecimal(okTospend.PayableAmount);
                        piChartData.OkToSpend = Convert.ToInt32(finalAmt);
                        result.manageFinance = piChartData;
                    }
                    else
                    {
                        result.manageFinance = piChartData;
                    }

                    result.RstKey = 1;
                    result.Message = ResponseMessages.DATA_RECEIVED; ;
                    result.IsSuccess = true;
                }
                else if (piChartData.Bills == null)
                {
                    var amt = balance != null ? Convert.ToDecimal(balance.wallet.availableBalance) : 0;
                    result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    result.IsSuccess = true;
                    result.RstKey = 2;
                    piChartData.OkToSpend = Convert.ToInt32(amt);
                    result.manageFinance = piChartData;
                    // result.manageFinance.OkToSpend = Convert.ToInt32(balance.Balance) - 0;
                }
                else
                {
                    result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                    result.IsSuccess = true;
                    result.RstKey = 3;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
