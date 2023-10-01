using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.DBEntity.WalletService;
using PayMasta.ViewModel.ItexVM;
namespace PayMasta.Repository.ItexRepository
{
    public interface IItexRepository
    {
        Task<List<WalletServiceResponse>> GetWalletServicesListBySubcategoryId(int SubCategoryId, IDbConnection exdbConnection = null);
        Task<VirtualAccountDetail> GetVirtualAccountDetailByUserId(long userId, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndService(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity(int SubCategoryId, string serviceName, string accountType, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity1(int SubCategoryId, string serviceName, string accountType, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForData(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null);
        Task<WalletService> GetWalletServicesListBySubcategoryIdAndServiceForElectricity(int SubCategoryId, string serviceName, IDbConnection exdbConnection = null);
        Task<VirtualAccountDetail> GetVirtualAccountDetailByVirtualAccountNumber(string accountNumber, IDbConnection exdbConnection = null);
    }
}
