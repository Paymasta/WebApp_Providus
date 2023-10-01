using PayMasta.ViewModel.ManageFinanceVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.ManageFinance
{
    public interface IManageFinanceRepository
    {
        Task<GetManageFinance> GetPiChartData(long userId, IDbConnection exdbConnection = null);
    }
}
