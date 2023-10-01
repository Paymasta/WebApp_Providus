using PayMasta.DBEntity.Earning;
using PayMasta.DBEntity.ErrorLog;
using PayMasta.DBEntity.RandomInvoiceNumber;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.FlutterWaveVM;
using PayMasta.ViewModel.NotificationsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.CommonReporsitory
{
    public interface ICommonReporsitory
    {
        Task<RandomInvoiceNumber> GetInvoiceNumber(IDbConnection dbConnection = null);
        RandomInvoiceNumber GetInvoiceNumberForBulkPayment(IDbConnection dbConnection);
        Task<WalletService> GetWalletServices(long BankCode, IDbConnection exdbConnection = null);
        Task<int> InsertWalletServices(WalletService walletService, IDbConnection exdbConnection = null);
        Task<List<WalletServiceResponse>> GetWalletServicesListBySubcategoryId(int SubCategoryId, IDbConnection exdbConnection = null);
        Task<int> InsertWalletTransactions(WalletTransaction walletTransaction, IDbConnection exdbConnection = null);
        Task<List<CategoryResponse>> GetCategories(bool isactive, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServices(string BankCode, int OperatorId, IDbConnection exdbConnection = null);
        Task<List<NotificationsResponse>> GetNotifications(long userId, IDbConnection exdbConnection = null);
        Task<int> GetNotificationsCount(long userId, IDbConnection exdbConnection = null);
        Task<int> UpdateUserNotificationIsRead(long userId, IDbConnection exdbConnection = null);
        Task<int> InsertDailyEarningByScheduler(IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesOperator(string BankCode, IDbConnection exdbConnection = null);
        Task<GetCms> GetFaq(IDbConnection exdbConnection = null);
        Task<GetCms> GetTandC(IDbConnection exdbConnection = null);
        Task<GetCms> GetPrivacyPolicy(IDbConnection exdbConnection = null);
        Task<int> InsertProvidusBankResponse(ErrorLog errorLog, IDbConnection exdbConnection = null);
        Task<ErrorLog> GetProvidusBankResponseBySessionId(string id, IDbConnection exdbConnection = null);
        Task<EarningMaster> GetSchedulerCurrentDate(IDbConnection exdbConnection = null);
        Task<List<FAQResponse>> FAQ(IDbConnection exdbConnection = null);
        Task<List<FaqDetailResponse>> FAQAnswers(int FaqId, IDbConnection exdbConnection = null);
        Task<WalletService> IsWalletServicesExists(string service, string name, IDbConnection exdbConnection = null);
    }
}
