using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.NotificationsVM;
using PayMasta.ViewModel.ProvidusBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Common
{
    public interface ICommonService
    {
        Task<InvoiceNumberResponse> GetInvoiceNumber(int digit = 6);
        Task<GetCategoryResponse> GetCategories();
        Task<GetNotificationsResponse> GetNotificationByUserGuid(NotificationsRequest notificationsRequest);
        Task<UpdateNotificationsResponse> UpdateNotificationByUserGuid(NotificationsRequest notificationsRequest);
        Task<UpdateNotificationsResponse> InsertDailyEarningByScheduler();
        InvoiceNumberResponse GetInvoiceNumberForBulkPayment(int digit = 6);
        Task<List<FAQResponse>> GetFaq();
        Task<CmsResponse> GetPrivacyPolicy();
        Task<CmsResponse> GetTermAndCondition();
        Task<GetProvidusResponse> GetProvidusBankResponse(ProvidusBankRequest request);
        Task<List<string>> MovieList();
        Task<int> GetProvidusBankResponse(string request);
        Task<UpdateNotificationsResponse> RequestDemo(RequestDemoRequest request);
        Task<string> WalletToWalletTransfer(long userId, double amount);
        Task<string> YourMethod();
        Task<string> GetZealvendAccessToken();
    }
}
