using PayMasta.ViewModel.ManageFinanceVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ManageFinance
{
    public interface IManageFinanceService
    {
        Task<GetManageFinanceResponse> GetPiChartData(ManageFinanceRequest request);
    }
}
