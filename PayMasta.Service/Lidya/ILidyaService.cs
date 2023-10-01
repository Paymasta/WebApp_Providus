using PayMasta.ViewModel.LidyaVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Lidya
{
    public interface ILidyaService
    {
        Task<MandateResponse> CreateMandat(MandateRequest mandateRequest);
    }
}
