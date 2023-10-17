using Newtonsoft.Json;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Repository.ItexRepository;
using PayMasta.Repository.Transactions;
using PayMasta.Service.Common;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.VirtualAccount;
using PayMasta.Utilities;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayMasta.ViewModel.Enums;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.ViewModel.ZealvendBillsVM;
using Org.BouncyCastle.Asn1.Ocsp;
using PayMasta.ViewModel.Common;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using PayMasta.ViewModel.VirtualAccountVM;
using RestClient.Net;
using PayMasta.DBEntity.WalletService;
using Org.BouncyCastle.Ocsp;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;

namespace PayMasta.Service.ZealvendService
{
    public class ZealvendBillService : IZealvendBillService
    {
        private readonly IExpressWalletThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly ICommonService _commonService;
        private readonly IItexRepository _itexRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IProvidusExpresssWalletService _providusExpresssWalletService;
        public ZealvendBillService()
        {
            _accountRepository = new AccountRepository();
            _thirdParty = new ExpressWalletThirdParty();
            _commonReporsitory = new CommonReporsitory();
            _transactionsRepository = new TransactionsRepository();
            _itexRepository = new ItexRepository();
            _commonService = new CommonService();
            _providusExpresssWalletService = new ProvidusExpresssWalletService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        //Airtime pay
        public async Task<GetAirtimeZealvendResponse> ZealvendAirtimePayment(VTUBillPaymentRequest request)
        {
            var result = new GetAirtimeZealvendResponse();
            var reqTran = new WalletTransaction();
            var res = new ResponseFromZealvend();
            string customer = string.Empty;
            var JsonResult = new ZealvendAirtimeResponse();
            //st.Append("ss");
            try
            {
                if (Convert.ToDecimal(request.Amount) < 100)
                {
                    result.IsSuccess = false;
                    result.RstKey = 4;
                    result.Message = "Amount should be greater then 100 naira";
                    //  result.vTUResponse = JsonResult;
                }

                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var userBalance = await _providusExpresssWalletService.GetVirtualAccount(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.wallet.availableBalance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount);
                var walletServiceData = await _transactionsRepository.GetWalletServicesListBySubcategoryIdAndService(request.SubCategoryId, request.Service);
                var url = AppSetting.ZealVendAirtime;
                string token = await _commonService.GetZealvendAccessToken();
                var invoiceNumber = await _commonService.GetInvoiceNumber();
                string customerNumber = request.phone.Substring(0, 1);
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    if (customerNumber != "0")
                    {
                        customer = "0" + request.phone;
                    }
                    else
                    {
                        customer = request.phone;
                    }
                    var airtimeReq = new ZealvendAirtimeRequest
                    {
                        amount = Convert.ToDecimal(request.Amount),
                        network = walletServiceData.BillerName,
                        number = customer,
                        reference = invoiceNumber.InvoiceNumber
                    };
                    var req = JsonConvert.SerializeObject(airtimeReq);
                    res = await _thirdParty.PostDataZealvend(req, url, token);
                    if (res.StatusCode == 201)
                    {
                        JsonResult = JsonConvert.DeserializeObject<ZealvendAirtimeResponse>(res.JsonString);
                        if (JsonResult != null && JsonResult.status == "success")
                        {

                            var debitRes = await _providusExpresssWalletService.DebitCustomerWalletForBills(userData.Guid, request.Amount, invoiceNumber.InvoiceNumber);
                            reqTran.IsAmountPaid = debitRes;
                            reqTran.AccountNo = request.phone;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = string.Empty;
                            reqTran.CommisionId = 0;
                            reqTran.CommisionPercent = 0;
                            reqTran.CreatedAt = DateTime.UtcNow;
                            reqTran.CreatedBy = userData.Id;
                            reqTran.DisplayContent = string.Empty;
                            reqTran.FlatCharges = 0;
                            reqTran.InvoiceNo = invoiceNumber.InvoiceNumber;
                            reqTran.IsActive = true;
                            reqTran.IsAdminTransaction = false;
                            reqTran.IsBankTransaction = false;
                            reqTran.IsDeleted = false;
                            reqTran.IsUpcomingBillShow = true;
                            reqTran.ReceiverId = userData.Id;
                            reqTran.SenderId = userData.Id; ;
                            reqTran.ServiceCategoryId = (int)walletServiceData.Id;
                            reqTran.ServiceTax = string.Empty;
                            reqTran.ServiceTaxRate = 0;
                            reqTran.SubCategoryId = walletServiceData.SubCategoryId;
                            reqTran.TotalAmount = request.Amount;
                            reqTran.TransactionId = JsonResult.data.referrence;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = "";
                            reqTran.WalletAmount = string.Empty;

                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.vTUResponse = JsonResult;
                            result.Message = ResponseMessages.TRANSACTION_DONE;
                            result.TransactionId = invoiceNumber.InvoiceNumber;
                        }

                    }
                    else if (res.StatusCode != 201 && res.StatusCode != 422)
                    {

                        reqTran.AccountNo = request.phone;
                        reqTran.TransactionStatus = (int)TransactionStatus.Failed;
                        reqTran.BankBranchCode = string.Empty;
                        reqTran.BankTransactionId = string.Empty;
                        reqTran.BenchmarkCharges = 0;
                        reqTran.Comments = string.Empty;
                        reqTran.CommisionAmount = string.Empty;
                        reqTran.CommisionId = 0;
                        reqTran.CommisionPercent = 0;
                        reqTran.CreatedAt = DateTime.UtcNow;
                        reqTran.CreatedBy = userData.Id;
                        reqTran.DisplayContent = string.Empty;
                        reqTran.FlatCharges = 0;
                        reqTran.InvoiceNo = invoiceNumber.InvoiceNumber;
                        reqTran.IsActive = true;
                        reqTran.IsAdminTransaction = false;
                        reqTran.IsBankTransaction = false;
                        reqTran.IsDeleted = false;
                        reqTran.IsUpcomingBillShow = true;
                        reqTran.ReceiverId = userData.Id;
                        reqTran.SenderId = userData.Id; ;
                        reqTran.ServiceCategoryId = (int)walletServiceData.Id;
                        reqTran.ServiceTax = string.Empty;
                        reqTran.ServiceTaxRate = 0;
                        reqTran.SubCategoryId = walletServiceData.SubCategoryId;
                        reqTran.TotalAmount = request.Amount;
                        reqTran.TransactionId = JsonResult.data.referrence;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;

                        result.Message = ResponseMessages.TRANSACTION_NOT_DONE;
                        result.TransactionId = invoiceNumber.InvoiceNumber;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.vTUResponse = JsonResult;
                    }

                    await _transactionsRepository.InsertTransactions(reqTran);
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 4;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    //  result.vTUResponse = JsonResult;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayTvVendResponseVM> PayTvVendPayment(PayTvVendRequestVM request)
        {
            var result = new PayTvVendResponseVM();
            string customer = string.Empty;
            var JsonResult = new PayTvResponse();
            var comissionAmt = 107.5;
            try
            {
                if (Convert.ToDecimal(request.Amount) < 100)
                {
                    result.IsSuccess = false;
                    result.RstKey = 4;
                    result.Message = "Amount should be greater then 100 naira";
                    //  result.vTUResponse = JsonResult;
                }
                //---------Check User Validation (User Not Found)----------
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                if (userData == null)
                {
                    result.RstKey = 101;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }

                //-------------Check User Wallet Balance---------------
                var accountDetails = await _providusExpresssWalletService.GetVirtualAccount(userData.Guid);
                decimal currentBalance = accountDetails != null ? Convert.ToDecimal(accountDetails.wallet.availableBalance) : 0;
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                if (accountDetails != null && accountDetails.wallet.availableBalance < Convert.ToInt32(requestedAmt))
                {
                    result.RstKey = 101;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (accountDetails == null)
                {
                    result.RstKey = 101;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }

                var walletServiceData = await _transactionsRepository.GetWalletServicesListBySubcategoryIdAndService(request.SubCategoryId, request.Service);

                var invoiceNumber = await _commonService.GetInvoiceNumber();
                string customerNumber = request.Phone.Substring(0, 1);

                //if (customerNumber != "0")
                //{
                //    customer = "0" + request.Phone;
                //}
                //else
                //{
                //    customer = request.Phone;
                //}
                string req = string.Empty;
                string endpoint = string.Empty;

                if (request.SubsctiptionType == (int)EnumSubsctiptionType.SubscriptionPackage)
                {
                    endpoint = AppSetting.ZealPayTvVendEndpoint;
                    var subsReq = new PayTvVendRequest
                    {
                        Plan = request.Code,
                        Product = request.Service,
                        CardNumber = request.SmartCardCode,
                        Referrence = invoiceNumber.InvoiceNumber
                    };
                    req = JsonConvert.SerializeObject(subsReq);
                }
                else
                {
                    endpoint = AppSetting.ZealPayTvTopupEndpoint;
                    var topReq = new PayTvTopupRequest
                    {
                        Amount = Convert.ToDecimal(request.Amount),
                        CardNumber = request.SmartCardCode,
                        Product = request.Service,
                        Referrence = invoiceNumber.InvoiceNumber
                    };
                    req = JsonConvert.SerializeObject(topReq);
                }
                string token = await _commonService.GetZealvendAccessToken();
                var res = await _thirdParty.PostDataZealvend(req, endpoint, token);
                if (res.StatusCode == 201)
                {
                    JsonResult = JsonConvert.DeserializeObject<PayTvResponse>(res.JsonString);
                    if (JsonResult != null && JsonResult.Status == "success")
                    {
                        var debitRes = await _providusExpresssWalletService.DebitCustomerWalletForBills(userData.Guid, requestedAmt.ToString(), invoiceNumber.InvoiceNumber);
                        await InsertWalletTransactionDetails(request, walletServiceData, userData.Id, (int)TransactionStatus.Completed, invoiceNumber.InvoiceNumber, JsonResult.Data.Referrence, debitRes);

                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.Data = JsonResult;
                        result.Message = ResponseMessages.TRANSACTION_DONE;
                        result.TransactionId = invoiceNumber.InvoiceNumber;
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.RstKey = 2;
                        result.Data = JsonResult;
                        result.Message = ResponseMessages.TRANSACTION_FAIL;
                        result.TransactionId = invoiceNumber.InvoiceNumber;
                    }
                }
                else
                {
                    result.IsSuccess = true;
                    result.RstKey = 2;
                    result.Data = JsonResult;
                    result.Message = ResponseMessages.TRANSACTION_FAIL;
                    result.TransactionId = invoiceNumber.InvoiceNumber;
                }
            }
            catch (Exception ex)
            {
                result.Message = ResponseMessages.EXCEPTION_OCCURED;
            }

            return result;
        }

        private async Task InsertWalletTransactionDetails(PayTvVendRequestVM request, WalletService walletServiceData, long userId, int status, string invoiceNumber, string referrence, bool ispaid)
        {
            var reqTran = new WalletTransaction();
            reqTran.IsAmountPaid = ispaid;
            reqTran.AccountNo = request.Phone;
            reqTran.TransactionStatus = status;
            reqTran.BankBranchCode = string.Empty;
            reqTran.BankTransactionId = string.Empty;
            reqTran.BenchmarkCharges = 0;
            reqTran.Comments = string.Empty;
            reqTran.CommisionAmount = string.Empty;
            reqTran.CommisionId = 0;
            reqTran.CommisionPercent = 0;
            reqTran.CreatedAt = DateTime.UtcNow;
            reqTran.CreatedBy = userId;
            reqTran.DisplayContent = string.Empty;
            reqTran.FlatCharges = 0;
            reqTran.InvoiceNo = invoiceNumber;
            reqTran.IsActive = true;
            reqTran.IsAdminTransaction = false;
            reqTran.IsBankTransaction = false;
            reqTran.IsDeleted = false;
            reqTran.IsUpcomingBillShow = true;
            reqTran.ReceiverId = userId;
            reqTran.SenderId = userId; ;
            reqTran.ServiceCategoryId = (int)walletServiceData.Id;
            reqTran.ServiceTax = string.Empty;
            reqTran.ServiceTaxRate = 0;
            reqTran.SubCategoryId = walletServiceData.SubCategoryId;
            reqTran.TotalAmount = request.Amount;
            reqTran.TransactionId = referrence;
            reqTran.TransactionType = status == (int)TransactionStatus.Completed ? "DEBIT" : "FAILED";
            reqTran.TransactionTypeInfo = 0;
            reqTran.UpdatedAt = DateTime.UtcNow;
            reqTran.UpdatedBy = userId;
            reqTran.VoucherCode = "";
            reqTran.WalletAmount = string.Empty;

            await _transactionsRepository.InsertTransactions(reqTran);
        }

        public async Task<PayTvProductResponseVM> GetPayTvProductList(string product)
        {
            var result = new PayTvProductResponseVM();

            try
            {
                string token = await _commonService.GetZealvendAccessToken();
                var res = await _thirdParty.GetDataZealvend(AppSetting.ZealPayTvProductEndpoint + product, token);
                if (res.StatusCode == 200)
                {
                    var JsonResult = JsonConvert.DeserializeObject<PayTvProductResponse>(res.JsonString);
                    if (JsonResult != null && JsonResult.Status == "success")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.Data = JsonResult;
                        result.Message = ResponseMessages.SUCCESS;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task<PayTvVerifyResponseVM> VerifyPayTv(PayTvVerifyRequest request)
        {
            var result = new PayTvVerifyResponseVM();

            try
            {
                string token = await _commonService.GetZealvendAccessToken();
                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.PostDataZealvend(req, AppSetting.ZealPayTvVerifyEndpoint, token);
                if (res.StatusCode == 200)
                {
                    var JsonResult = JsonConvert.DeserializeObject<PayTvVerifyResponse>(res.JsonString);
                    if (JsonResult != null && JsonResult.Status == "success")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.Data = JsonResult;
                        result.Message = ResponseMessages.SUCCESS;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
