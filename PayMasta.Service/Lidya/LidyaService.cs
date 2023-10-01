using Newtonsoft.Json;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.ViewModel.LidyaVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace PayMasta.Service.Lidya
{
    public class LidyaService : ILidyaService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILidyaThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        public LidyaService()
        {
            _accountRepository = new AccountRepository();
            _thirdParty = new LidyaThirdParty();
            _commonReporsitory = new CommonReporsitory();
        }

        /// <summary>
        /// CreateMandat
        /// </summary>
        /// <param name="mandateRequest"></param>
        /// <returns></returns>
        public async Task<MandateResponse> CreateMandat(MandateRequest mandateRequest)
        {
            var result = new MandateResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(mandateRequest.UserGuid);

                if (userData != null)
                {
                   

                    var authReq = new LidyaAuth
                    {
                        username = AppSetting.LidyaAuthUsername,
                        password = AppSetting.LidyaAuthPassword
                    };

                    var authRes = await _thirdParty.LidyaAuth(authReq, AppSetting.LidyaAuthUrl);
                    if (authRes != null && !string.IsNullOrWhiteSpace(authRes.Token))
                    {
                        var bankData = await _accountRepository.GetBankDetailByUserId(userData.Id);
                        var authData = JsonConvert.DeserializeObject<LidyaMandateCreateResponse>(authRes.Data);
                        var createReq = new LidyaMandateCreate
                        {
                            amount = Convert.ToInt32(mandateRequest.Amount),
                            description = mandateRequest.Description,
                            duration = mandateRequest.Duration.ToString(),
                            enterprise_id = authData.default_enterprise_id,
                            frequency = "",
                            start_date = mandateRequest.StartDate.ToString("yyyy-MM-dd"),
                            type = AppSetting.TypeVariable,
                            payer_data = new PayerData
                            {
                                bvn = bankData.BVN,
                                email = userData.Email,
                                name = userData.FirstName + " " + userData.LastName,
                                phone_number = userData.PhoneNumber,
                                phone_prefix_id = 2
                            }
                        };
                        var createRes = await _thirdParty.LidyaCreateMandate(createReq, AppSetting.LidyaMandateCreateUrl, authRes.Token);
                        if (createRes != null)
                        {
                            result.RstKey = 1;
                            result.IsSuccess = true;
                            result.collect_id = Convert.ToInt32(createRes);
                        }
                    }
                    else
                    {
                        result.RstKey = 2;
                        result.IsSuccess = false;
                    }
                }
                else
                {
                    result.RstKey = 3;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.RstKey = 4;
                result.IsSuccess = false;
            }

            return result;
        }
    }
}
