using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.MultiChoicePurchaseVM;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.PurchaseInternetVM;
using PayMasta.ViewModel.PurchaseStarLineVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ItexRechargeService
{
    public interface IItexRechargeService
    {
        Task<GetVTUResponse> AirtimePayment(VTUBillPaymentRequest request);
        Task<GetDataRechargeResponse> DataRechargePayment(DataBillPaymentRequest request);
        Task<GetElectricityRechargeResponse> ElectricityRechargePayment(ElectricityBillPaymentRequest request);
        Task<GetMultiChoiceRechargeResponse> MultiChoiceRechargePayment(MultiChoicePurchaseBillPaymentRequest request);
        Task<GetStarLinePurchaseRechargeResponse> StarlineRechargePayment(StarLinePurchaseBillPaymentRequest request);
        Task<GetInternetPurchaseRechargeResponse> InternetRechargePayment(InternetPurchaseBillPaymentRequest request);
        Task<GetWalletToBankResponse> WalletToBankTransfer(WalletToBankPaymentRequest request);
        Task<GetVerifyAccountResponse> VerifyAccount(VerifyAccountRequest request);
        Task<GetWalletToBankResponse> WalletToWalletTransfer(WalletToBankPaymentRequest request);
    }
}
