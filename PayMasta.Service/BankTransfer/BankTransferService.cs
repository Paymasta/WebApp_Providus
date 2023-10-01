using Newtonsoft.Json;
using PayMasta.Repository.Account;
using PayMasta.Repository.BankTransfer;
using PayMasta.Service.Common;
using PayMasta.Service.ItexService;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel.BankTransferVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.BankTransfer
{
    public class BankTransferService : IBankTransferService
    {
        private readonly IBankTransferRepository _bankTransferRepository;
        private readonly IThirdParty _thirdParty;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonService _commonService;
        public BankTransferService()
        {
            _bankTransferRepository = new BankTransferRepository();
            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _accountRepository = new AccountRepository();
            _commonService = new CommonService();
         //   _itexService = new PayMasta.Service.ItexService.ItexService();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<GetBankListResponse> GetBanks()
        {
            var result = new GetBankListResponse();
            var res = await _thirdParty.GetBankTransaction(AppSetting.GetNIPBanks);
            var banks = JsonConvert.DeserializeObject<BankTransferViewModel>(res);
            if (banks.banks.Count > 0 && banks.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.banks = banks.banks;
                result.RstKey = 1;
                result.responseMessage = banks.responseMessage;
            }
            else
            {
                result.RstKey = 2;
            }
            return result;
        }
      
        public async Task<GetAccountResponse> GetNIPAccount(GetAccountRequest request)
        {
            var result = new GetAccountResponse();
            var req = new GetNIPAccountRequest
            {
                accountNumber = request.AccountNumber,
                beneficiaryBank = request.BeneficiaryBank,
                password = AppSetting.BankPassword,
                userName = AppSetting.BankUserName
            };
            var jsonReq = JsonConvert.SerializeObject(req);
            var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.GetNIPAccount);
            var nipAccountResponse = JsonConvert.DeserializeObject<GetNIPAccountResponse>(res);
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
            }
            if (nipAccountResponse != null && nipAccountResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.getNIPAccountResponse = nipAccountResponse;
                result.Status = true;
                result.RstKey = 1;
                result.Message = nipAccountResponse.responseMessage;
            }
            else
            {
                result.getNIPAccountResponse = nipAccountResponse;
                result.Status = false;
                result.RstKey = 2;
                result.Message = nipAccountResponse.responseMessage;
            }
            return result;
        }

        public async Task<FundTransferResponse> NIPFundTransfer(FundTransferRequest request)
        {
            var result = new FundTransferResponse();
            var invoiceNumber = await _commonService.GetInvoiceNumber();
            var req = new NIPFundTransferRequest
            {
                beneficiaryAccountName = request.beneficiaryAccountName,
                beneficiaryAccountNumber = request.beneficiaryAccountNumber,
                beneficiaryBank = request.beneficiaryBank,
                currencyCode = AppSetting.currencyCode,
                narration = AppSetting.narration,
                sourceAccountName = request.sourceAccountName,
                transactionAmount = request.transactionAmount,
                transactionReference = invoiceNumber.InvoiceNumber,
                password = AppSetting.BankPassword,
                userName = AppSetting.BankUserName
            };
            var jsonReq = JsonConvert.SerializeObject(req);
            var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.NIPFundTransfer);
            var nIPFundTransferResponse = JsonConvert.DeserializeObject<NIPFundTransferResponse>(res);
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
            }
            if (nIPFundTransferResponse != null && nIPFundTransferResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.nIPFundTransferResponse = nIPFundTransferResponse;
                result.Status = true;
                result.RstKey = 1;
                result.Message = nIPFundTransferResponse.responseMessage;
            }
            else
            {

                result.Status = false;
                result.RstKey = 2;
                result.Message = nIPFundTransferResponse.responseMessage;
            }
            return result;
        }

        public async Task<ProvidusFundResponse> ProvidusFundTransfer(ProvidusFundTransferRequest request)
        {
            var result = new ProvidusFundResponse();
            var invoiceNumber = await _commonService.GetInvoiceNumber();
            var req = new ProvidusFundTransfer
            {
                creditAccount = AppSetting.PayMastaAccountNumber,
                debitAccount = request.DebitAccount,
                transactionAmount = request.TransactionAmount,
                currencyCode = AppSetting.currencyCode,
                narration = AppSetting.narration,
                transactionReference = invoiceNumber.InvoiceNumber,
                password = AppSetting.BankPassword,
                userName = AppSetting.BankUserName
            };
            var jsonReq = JsonConvert.SerializeObject(req);
            await _commonService.GetProvidusBankResponse("Request=" + jsonReq);
            var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.ProvidusFundTransfer);
            var providusFundTransferResponse = JsonConvert.DeserializeObject<ProvidusFundTransferResponse>(res);
            await _commonService.GetProvidusBankResponse("Response=" + res);

            if (providusFundTransferResponse != null && providusFundTransferResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.transferResponse = providusFundTransferResponse;
                result.Status = true;
                result.RstKey = 1;
                result.Message = providusFundTransferResponse.responseMessage;
            }
            else
            {

                result.Status = false;
                result.RstKey = 2;
                result.Message = providusFundTransferResponse.responseMessage;
            }
            return result;
        }

        public async Task<ProvidusAccountResponse> GetProvidusAccount(GetAccountRequest request)
        {
            var result = new ProvidusAccountResponse();
            var req = new GetProvidusAccount
            {
                accountNumber = request.AccountNumber,
                password = AppSetting.BankPassword,
                userName = AppSetting.BankUserName
            };
            var jsonReq = JsonConvert.SerializeObject(req);
            var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.GetProvidusAccount);
            var nipAccountResponse = JsonConvert.DeserializeObject<GetProvidusAccountResponse>(res);
            using (var dbConnection = Connection)
            {
                var user = await _accountRepository.GetUserByGuid(request.UserGuid, dbConnection);
            }
            if (nipAccountResponse != null && nipAccountResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.accountResponse = nipAccountResponse;
                result.Status = true;
                result.RstKey = 1;
                result.Message = nipAccountResponse.responseMessage;
            }
            else
            {
                result.accountResponse = nipAccountResponse;
                result.Status = false;
                result.RstKey = 2;
                result.Message = nipAccountResponse.responseMessage;
            }
            return result;
        }

        public async Task<ProvidusFundResponse> PayToPaymastaFundTransfer(string DebitAccount, string TransactionAmount)
        {
            var result = new ProvidusFundResponse();
            var invoiceNumber = await _commonService.GetInvoiceNumber();
            var req = new ProvidusFundTransfer
            {
                creditAccount = AppSetting.PayMastaAccountNumber,
                debitAccount = DebitAccount,
                transactionAmount = TransactionAmount,
                currencyCode = AppSetting.currencyCode,
                narration = AppSetting.narration,
                transactionReference = invoiceNumber.InvoiceNumber,
                password = AppSetting.BankPassword,
                userName = AppSetting.BankUserName
            };
            var jsonReq = JsonConvert.SerializeObject(req);
            await _commonService.GetProvidusBankResponse("Request PayToPaymastaFundTransfer=" + jsonReq);
            var res = await _thirdParty.PostBankTransaction(jsonReq, AppSetting.ProvidusFundTransfer);
            var providusFundTransferResponse = JsonConvert.DeserializeObject<ProvidusFundTransferResponse>(res);
            await _commonService.GetProvidusBankResponse("Response PayToPaymastaFundTransfer=" + res);

            if (providusFundTransferResponse != null && providusFundTransferResponse.responseCode == AggregatorySTATUSCODES.SUCCESSFUL)
            {
                result.transferResponse = providusFundTransferResponse;
                result.Status = true;
                result.RstKey = 1;
                result.Message = providusFundTransferResponse.responseMessage;
            }
            else
            {

                result.Status = false;
                result.RstKey = 2;
                result.Message = providusFundTransferResponse.responseMessage;
            }
            return result;
        }
    }
}
