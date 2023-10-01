using PayMasta.ViewModel.LidyaVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ThirdParty
{
    public interface ILidyaThirdParty
    {
        Task<LidyaAuthResponse> LidyaAuth(object req, string url);
        Task<string> LidyaCreateMandate(object req, string url, string token);
    }
}
