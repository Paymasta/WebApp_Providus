using Newtonsoft.Json;
using PayMasta.DBEntity.OkraCallBack;
using PayMasta.Repository.Account;
using PayMasta.Repository.CallBack;
using PayMasta.Service.Common;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel.CallBack;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.CallBack
{
    public class CallBackService : ICallBackService
    {
        private readonly IThirdParty _thirdParty;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonService _commonService;
        private readonly ICallBackRepository _callBackRepository;

        public CallBackService()
        {

            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _accountRepository = new AccountRepository();
            _callBackRepository = new CallBackRepository();
            _commonService = new CommonService();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<WidgetResponse> WidgetCallBack(AuthWidgetResponse request)
        {
            var result = new WidgetResponse();

            try
            {
                //var bankData = await _callBackRepository.GetBankDetailByCustomerId(request.customerId);
                //if (bankData != null)
                //{
                var userData = await _accountRepository.GetUserById(request.options.UserId);
                if (userData != null && userData.IslinkToOkra == false)
                {
                    userData.IslinkToOkra = true;
                    await _callBackRepository.UpdateOkraLinkStatus(userData);
                }
                using (var dbConnection = Connection)
                {
                    var jsonData = JsonConvert.SerializeObject(request);
                    var req = new OkraCallBackResponse
                    {
                        CallBackType = request.callback_type,
                        CallBackUrl = request.callback_url,
                        CustomerId = request.customerId,
                        RawContent = jsonData,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = 0,
                        UserId = request.options.UserId,
                        CreatedAt = DateTime.UtcNow,
                        BankCodeOrBankId=request.bankId
                    };
                    var res = await _callBackRepository.InsertOkraCallBackDetail(req, dbConnection);
                    if (res > 0)
                    {
                        result.Message = ResponseMessages.SUCCESS;
                        result.IsSuccess = true;
                        result.RstKey = 1;
                    }
                    else
                    {
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                        result.RstKey = 2;
                    }
                }

                // }
            }
            catch (Exception ex)
            {

            }
            return result;
        }



    }
}
