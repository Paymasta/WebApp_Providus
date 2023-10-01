using PayMasta.ViewModel;
using PayMasta.ViewModel.VerifyNinVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.VerifyNin
{
    public interface IVerifyNinService
    {
        //Task<VerifyResponse> VerifyMe(VerifyRequest request);
        Task<QoreIdBvnNubanResult> VerifyNuban(RegisterByNubanRequest request);
        Task<BankListResult> BankListForRegister();
        Task<IsOtpVerifiedResponse> VerifyOTPWeb(VerifyOTPRequest request);
        Task<AddOtherDetailResponse> AddUserOtherDetail(AddOtherDetailPRequest request);
        Task<QoreIdBvnNubanResult> VerifyVnin(RegisterByVNinRequest request);
    }
}
