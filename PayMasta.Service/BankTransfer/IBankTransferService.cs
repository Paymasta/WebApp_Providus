using PayMasta.ViewModel.BankTransferVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.BankTransfer
{
    public interface IBankTransferService
    {
        Task<GetBankListResponse> GetBanks();
        Task<GetAccountResponse> GetNIPAccount(GetAccountRequest request);
        Task<FundTransferResponse> NIPFundTransfer(FundTransferRequest request);
        Task<ProvidusFundResponse> ProvidusFundTransfer(ProvidusFundTransferRequest request);
        Task<ProvidusAccountResponse> GetProvidusAccount(GetAccountRequest request);
        Task<ProvidusFundResponse> PayToPaymastaFundTransfer(string DebitAccount, string TransactionAmount);
    }
}
