using PayMasta.ViewModel.BulkPaymentVM;
using PayMasta.ViewModel.FlutterWaveVM;
using PayMasta.ViewModel.VirtualAccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.FlutterWave
{
    public interface IFlutterWaveService
    {
        //Task<CreateVertualAccountResponse> CreateVertualAccount(CreateVertualAccountRequest request);
        //Task<string> CreateVertualAccount(CreateVertualAccountRequest request);
        Task<AirtimeResponse> GetAirtimeOperatorList();
        Task<AirtimeResponse> GetDataBundleOperatorList();
        Task<AirtimeResponse> GetWifiInternetOperatorList();
        Task<AirtimeResponse> GetTVOperatorList();
        Task<AirtimeResponse> GetElectricityOperatorList();
        Task<ProductResponse> FilterOperatorByBillerCode(string BillerCode);
        Task<PayMasta.ViewModel.AirtimeVM.AirtimeRechargeResponse> AirtimeRecharge(AirtimeRechargeRequest request);
        Task<PayMasta.ViewModel.AirtimeVM.TVRechargeResponse> TVRecharge(AirtimeRechargeRequest request);
        Task<PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse> InternetRecharge(AirtimeRechargeRequest request);
        Task<PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse> DataBundleRecharge(AirtimeRechargeRequest request);
        Task<PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse> ElectriCityBillRecharge(AirtimeRechargeRequest request);
        Task<PayMasta.ViewModel.AirtimeVM.BulkBillRechargeResponse> BulkBillPayment(BulkPaymentRequest request);
    }
}
