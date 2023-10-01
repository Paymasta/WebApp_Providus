using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ZealvendService.DataBundle
{
    public interface IZealvendDataService
    {
        Task<OperatorResponse> GetDataOperatorList(OperatorRequest request);
        Task<GetDataResponse> GetDataOperatorPlanList(OperatorPlanRequest request);
        Task<GetZealvendDataRechargeResponse> DataRechargePayment(DataBillPaymentRequest request);
    }
}
