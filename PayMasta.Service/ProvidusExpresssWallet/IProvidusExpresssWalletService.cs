using PayMasta.ViewModel.ProvidusExpresssWalletVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ProvidusExpresssWallet
{
    public interface IProvidusExpresssWalletService
    {
        Task<bool> CreateVirtualAccount(Guid userGuid);
        Task<CustomerWalletDetailResponse> GetVirtualAccount(Guid userGuid);
        Task<GetExpressWalletToBankResponse> WalletToWalletTransfer(WalletToWalletRequest request);
        Task<GetExpressBankListResponse> GetExpressBankList(Guid userGuid);
        Task<GetExpressWalletToBankRes> WalletToBankTransfer(WalletToBankPaymentRequest request);
        Task<bool> DebitCustomerWalletForBills(Guid userGuid, string amount, string invoiveNumbe);
        Task<VerifyExpressBankAccount> VerifyAccount(string bankCode, string accountNumber);
    }
}
