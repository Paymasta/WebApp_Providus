using PayMasta.ViewModel.OkraAPIVM;
using PayMasta.ViewModel.OkraBankVM;
using PayMasta.ViewModel.OkraVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Okra
{
    public interface IOkraService
    {
        Task<WodgetLinkGenerateResponse> GetWidgetLink(WodgetLinkGenerateRequest request);
        Task<IncomeWodgetLinkGenerateResponse> GetIncome(Guid guid, string bankId);
        Task<AuthWodgetLinkGenerateResponse> GetAuth(Guid guid, string bankId);
        Task<BalanceWodgetLinkGenerateResponse> GetBalance(Guid guid, string bankId);
        Task<IdentityWodgetLinkGenerateResponse> GetIdentity(Guid guid, string bankId);
        Task<TransactionsWodgetLinkGenerateResponse> GetTransactions(Guid guid, string bankId);
        Task<OkraBankResponse> GetOkraBankList();
        Task<LinkedOrUnlinkedBankResponse> GetLinkedOrUnLinkedBank(Guid guid);
    }
}
