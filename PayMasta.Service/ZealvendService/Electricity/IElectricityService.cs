using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ZealvendService.Electricity
{
    public interface IElectricityService
    {
        Task<OperatorResponse> GetElectricityOperatorList(OperatorRequest request);
        Task<GetZealElectricityRechargeResponse> ElectricityRechargePayment(ElectricityBillPaymentRequest request);
        Task<MeterVerifyResponse> MeterVerify(VerifyRequest request);
    }
}
