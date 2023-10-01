using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Support
{
    public interface ISupportService
    {
        Task<SupportResponse> InsertSupportTicket(SupportRequest request);
        Task<SupportMasterResponse> GetSupportDetailList(GetSupportMasterRequest request);
        Task<SupportMasterResponse> GetSupportDetailList(GetSupportListRequest request);
    }
}
