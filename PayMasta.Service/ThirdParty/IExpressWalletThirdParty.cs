using PayMasta.ViewModel.ZealvendBillsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ThirdParty
{
    public interface IExpressWalletThirdParty
    {
        Task<string> PostData(string req, string url);
        Task<string> GetDate(string url);
        Task<ResponseFromZealvend> PostDataZealvend(string req, string url, string token = null);
        Task<ResponseFromZealvend> GetDataZealvend(string url, string token);
    }
}
