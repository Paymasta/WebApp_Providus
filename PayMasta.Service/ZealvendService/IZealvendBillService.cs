using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ZealvendService
{
    public interface IZealvendBillService
    {
        Task<PayTvProductResponseVM> GetPayTvProductList(string product);
        Task<PayTvVendResponseVM> PayTvVendPayment(PayTvVendRequestVM request);
        Task<PayTvVerifyResponseVM> VerifyPayTv(PayTvVerifyRequest request);
        Task<GetAirtimeZealvendResponse> ZealvendAirtimePayment(VTUBillPaymentRequest request);
    }
}
