using PayMasta.ViewModel.BankTransferVM;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.PayToPayMasta
{
    public interface IPayToPayMastaService
    {
        Task<AccessAmountViewModelResponse> GetEmployeesEwaRequestList(PayToPayMastaRequest request);
        Task<PayableAmountResponse> GetPayAbleAmount(Guid userGuid);
        Task<MemoryStream> ExportUserListReport(PayToPayMastaRequest request);
        Task<ProvidusFundResponse> PayToPayMastaAccount(ProvidusFundTransferRequest request);
        Task<string> Invoice(Guid userGuid);
    }
}
