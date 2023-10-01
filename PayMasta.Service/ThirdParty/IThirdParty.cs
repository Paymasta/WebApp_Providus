using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ThirdParty
{
    public interface IThirdParty
    {
        Task<string> NinVerification(string req, string url);
        Task<string> VNinVerification(string req, string url);
        Task<string> PostBankTransaction(string req, string url);
        Task<string> GetBankTransaction(string url);
        Task<string> PostGenerateWidgetLink(string req, string url);
        Task<string> GetGenerateWidgetLink(string url);
        Task<string> CreateVertualAccount(string req, string url);
        Task<string> getOperatorList(string url);
        Task<string> GetAggregatorFunctionForOkra(string url);
        Task<string> UploadFiles(string image, string fileName);
        Task<string> PostAirtime(string req, string url);
        Task<string> GetVertualAccountBalance(string url);
        Task<string> CreateProvidusVirtualAccount(string req, string url);
        Task<string> GetVirtualAccount(string token, string url);
        Task<string> getOperatorList(string url, string token);
        Task<string> postOperatorList(string request, string url, string token);
        Task<string> ChangePassword(string url, string requestBody);
        Task<string> GetNubanAccountNumber(string url, string requestBody, string token);
        Task<string> UpdatePin(string req, string url, string token);
        Task<string> NubanVerification(string req, string url, string token);
        Task<string> QoreIdAuthToken(string req, string url);
    }
}
