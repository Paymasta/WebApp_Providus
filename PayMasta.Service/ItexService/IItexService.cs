using PayMasta.ViewModel.BouquetsVM;
using PayMasta.ViewModel.ElectricityVM;
using PayMasta.ViewModel.InternetVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ValidateMultichoiceVM;
using PayMasta.ViewModel.ValidateStartimesVM;
using PayMasta.ViewModel.ValidatInternetVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ItexService
{
    public interface IItexService
    {
        Task<OperatorResponse> GetAirtimeOperatorList(OperatorRequest request);
        Task<OperatorResponse> GetInternetOperatorList(OperatorRequest request);
        Task<OperatorResponse> GetTvOperatorList(OperatorRequest request);
        Task<OperatorResponse> GetElectricityOperatorList(OperatorRequest request);
        Task<OperatorResponse> GetDataOperatorList(OperatorRequest request);
        Task<GetPlanListResponses> GetDataOperatorPlanList(OperatorPlanRequest request);
        Task<GetValidateMeterResponseResponses> MeterValidate(MeterValidateRequest request);
        Task<GetBouquestResponses> MultichoiceBouquets(GetBouquetDataRequest request);
        Task<GetValidateBouquestResponses> ValidateMultichoice(GetValidatBouquetDataRequest request);
        Task<GetValidateStarTimesResponses> ValidateStartimes(GetValidatStarTimesDataRequest request);
        Task<GetInternetBundlesResponse> GetInternetBundles(GetInternetBundlesRequest request);
        Task<GetValidateInternetResponse> GetInternetValidation(ValidateInternetRequest request,string token);
        Task<BankListResponse> GetBankList(OperatorRequest request);
        Task<LatestBankListResponse> GetLatestBankList(OperatorRequest request);
    }
}
