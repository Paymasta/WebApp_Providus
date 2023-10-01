using PayMasta.ViewModel.VirtualAccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.VirtualAccount
{
    public interface IVirtualAccountService
    {
        // Task<bool> CreateVirtualAccount(string FirstName, string LastName, string Email, string PhoneNumber, long userId);
        //  Task<GetVirtualAccountBalanceResponse> GetVirtualAccountBalance(Guid userId);
        Task<bool> CreateVirtualAccount(string FirstName, string LastName, string Bvn, long userId, string addess, DateTime dateOfBirth, string email, string gender, string phoneNumber, string latitude
             , string localGovt, string longitude, string password, string pin, string state);
        Task<CurrentBalanceResponse> GetVirtualAccountBalance(Guid guid);
        Task<bool> AuthenticateVirtualAccount(string username, string password, bool rememberMe, string schemeid, string deviceId, long userId);
        Task<CurrentBalanceResponse> ChangesWalletPassword(Guid guid, string password);
        Task<bool> UpdatePin(long userId,int pin);
    }
}
