using Newtonsoft.Json;
using PayMasta.DBEntity.WidgetLinkMaster;
using PayMasta.Repository.Account;
using PayMasta.Repository.Okra;
using PayMasta.Service.Common;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.OkraAPIVM;
using PayMasta.ViewModel.OkraBankVM;
using PayMasta.ViewModel.OkraVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Okra
{
    public class OkraService : IOkraService
    {

        private readonly IThirdParty _thirdParty;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonService _commonService;
        private readonly IOkraRepository _okraRepository;
        public OkraService()
        {

            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _accountRepository = new AccountRepository();
            _commonService = new CommonService();
            _okraRepository = new OkraRepository();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<WodgetLinkGenerateResponse> GetWidgetLink(WodgetLinkGenerateRequest request)
        {
            var result = new WodgetLinkGenerateResponse();
            var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
            var widgetData = await _okraRepository.GetWidgetLinkByUserId(userData.Id);
            if (widgetData == null)
            {

                string[] products = { "auth", "identity", "balance", "transactions", "income" };
                List<string> billable_products = new List<string>(products);
                string[] country = { "NG" };
                List<string> countries = new List<string>(country);
                var req = new OkraWodgetLinkGenerateRequest();
                var option = new options();
                req.color = "#2D2D3D";
                req.callback_url = AppSetting.okracallback_url;
                req.logo = AppSetting.Okralogo;
                req.name = AppSetting.OkraUserName;
                req.support_email = AppSetting.Okrasupport_email;
                req.billable_products = billable_products;
                req.countries = countries;
                req.success_url = "http://104.45.194.41:8083/";
                req.continue_cta = AppSetting.continue_cta;
                option.UserEmail = userData.Email;
                option.UserGuid = userData.Guid;
                option.UserId = userData.Id;
                req.options = option;

                var jsonreq = JsonConvert.SerializeObject(req);
                var res = await _thirdParty.PostGenerateWidgetLink(jsonreq, AppSetting.OkraLinkUrl);
                var widgetResponse = JsonConvert.DeserializeObject<OkraWodgetLinkGenerateResponse>(res);
                if (widgetResponse.data.link.url != null)
                {
                    var widgetLinkMasterReq = new WidgetLinkMaster
                    {
                        RawContent = res,
                        UserId = userData.Id,
                        WidgetLink = widgetResponse.data.link.url,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userData.Id,
                    };
                    result.Status = true;
                    result.UrlLink = widgetResponse.data.link.url;
                    result.RstKey = 1;
                    result.Message = ResponseMessages.SUCCESS;
                    try
                    {
                        var data = await _okraRepository.InsertWidgetLinkDetail(widgetLinkMasterReq);
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            else if (widgetData.WidgetLink != null)
            {
                result.Status = true;
                result.UrlLink = widgetData.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<IncomeWodgetLinkGenerateResponse> GetIncome(Guid guid, string bankId)
        {
            var result = new IncomeWodgetLinkGenerateResponse();

            var user = await _accountRepository.GetUserByGuid(guid);
            var okraRes = await _okraRepository.GetIncomeUrlByUserId(user.Id, OkraCallBackType.INCOME, bankId);

            var res = await _thirdParty.GetGenerateWidgetLink(okraRes.CallBackUrl);
            var widgetResponse = JsonConvert.DeserializeObject<OkraIncomeResponseModel>(res);
            if (widgetResponse != null)
            {
                result.Status = true;
                result.okraIncomeResponseModel = widgetResponse;
                //result.UrlLink = AppSetting.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<AuthWodgetLinkGenerateResponse> GetAuth(Guid guid, string bankId)
        {
            var result = new AuthWodgetLinkGenerateResponse();

            var user = await _accountRepository.GetUserByGuid(guid);
            var okraRes = await _okraRepository.GetIncomeUrlByUserId(user.Id, OkraCallBackType.AUTH, bankId);

            var res = await _thirdParty.GetGenerateWidgetLink(okraRes.CallBackUrl);
            var widgetResponse = JsonConvert.DeserializeObject<AuthCallBackResponse>(res);
            if (widgetResponse != null)
            {
                result.Status = true;
                result.authCallBackResponse = widgetResponse;
                //result.UrlLink = AppSetting.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<BalanceWodgetLinkGenerateResponse> GetBalance(Guid guid, string bankId)
        {
            var result = new BalanceWodgetLinkGenerateResponse();

            var user = await _accountRepository.GetUserByGuid(guid);
            var okraRes = await _okraRepository.GetIncomeUrlByUserId(user.Id, OkraCallBackType.BALANCE, bankId);
            if (okraRes != null)
            {
                //var bankData = await _accountRepository.GetBankByBankCode(bankId, user.Id);
                var res = await _thirdParty.GetGenerateWidgetLink(okraRes.CallBackUrl);
                var balanceResponse = JsonConvert.DeserializeObject<BalanceCallBackResponse>(res);
                if (balanceResponse != null)
                {
                    result.Status = true;
                    // result.balanceCallBackResponse = balanceResponse;
                   // result.BankName = bankData.BankName;
                    //result.AccountNumber = bankData.AccountNumber;
                    result.Balance = balanceResponse.data.balance.available_balance.ToString();
                    result.RstKey = 1;
                    //result.ImageUrl = bankData.ImageUrl;
                    result.Message = ResponseMessages.SUCCESS;
                }
                else
                {
                    result.RstKey = 2;
                    result.Status = false;
                    result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                }
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<IdentityWodgetLinkGenerateResponse> GetIdentity(Guid guid, string bankId)
        {
            var result = new IdentityWodgetLinkGenerateResponse();

            var user = await _accountRepository.GetUserByGuid(guid);
            var okraRes = await _okraRepository.GetIncomeUrlByUserId(user.Id, OkraCallBackType.IDENTITY, bankId);

            var res = await _thirdParty.GetGenerateWidgetLink(okraRes.CallBackUrl);
            var identityResponse = JsonConvert.DeserializeObject<OkraIdentityResponseModel>(res);
            if (identityResponse != null)
            {
                result.Status = true;
                result.okraIdentityResponseModel = identityResponse;
                //result.UrlLink = AppSetting.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<TransactionsWodgetLinkGenerateResponse> GetTransactions(Guid guid, string bankId)
        {
            var result = new TransactionsWodgetLinkGenerateResponse();

            var user = await _accountRepository.GetUserByGuid(guid);
            var okraRes = await _okraRepository.GetIncomeUrlByUserId(user.Id, OkraCallBackType.TRANSACTIONS, bankId);

            var res = await _thirdParty.GetGenerateWidgetLink(okraRes.CallBackUrl);
            var transactionResponse = JsonConvert.DeserializeObject<OkraTransactionsResponseModel>(res);
            if (transactionResponse != null)
            {
                result.Status = true;
                result.okraTransactionsResponseModel = transactionResponse;
                //result.UrlLink = AppSetting.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }

        public async Task<OkraBankResponse> GetOkraBankList()
        {
            var result = new OkraBankResponse();

            var response = new OkraBankListResponse();
            try
            {
                var res = await _thirdParty.GetAggregatorFunctionForOkra(AppSetting.OkraGetBankList);
                response = JsonConvert.DeserializeObject<OkraBankListResponse>(res);
                if (response.status == "success")
                {
                    result.okraBankListResponse = response;
                    result.Status = true;
                    result.RstKey = 1;
                    result.Message = ResponseMessages.SUCCESS;
                }
                else
                {
                    result.RstKey = 2;
                    result.Status = false;
                    result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                }
            }
            catch (Exception ex)
            {

            }
          
            return result;
        }

        public async Task<LinkedOrUnlinkedBankResponse> GetLinkedOrUnLinkedBank(Guid guid)
        {
            var result = new LinkedOrUnlinkedBankResponse();
            var user = await _accountRepository.GetUserByGuid(guid);
            var bankListRes = await _okraRepository.GetLinkedOrUnLinkedBank(user.Id);
            if (bankListRes.Count > 0)
            {
                result.Status = true;
                result.linkedOrUnlinkedBanks = bankListRes;
                //result.UrlLink = AppSetting.WidgetLink;
                result.RstKey = 1;
                result.Message = ResponseMessages.SUCCESS;
            }
            else
            {
                result.RstKey = 2;
                result.Status = false;
                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
            }
            return result;
        }
    }
}
