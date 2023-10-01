using Newtonsoft.Json;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Repository.ItexRepository;
using PayMasta.Repository.Transactions;
using PayMasta.Service.Common;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;

namespace PayMasta.Service.ZealvendService.DataBundle
{
    public class ZealvendDataService : IZealvendDataService
    {
        private readonly IExpressWalletThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly ICommonService _commonService;
        private readonly IItexRepository _itexRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IProvidusExpresssWalletService _providusExpresssWalletService;
        public ZealvendDataService()
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

        public async Task<OperatorResponse> GetDataOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);

                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.DATABUNDLE);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetDataResponse> GetDataOperatorPlanList(OperatorPlanRequest request)
        {
            var result = new GetDataResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndService(request.SubCategoryId, request.service);
                var url = AppSetting.ZealPayDataNetworkEndpoint + walletServiceData.BillerName;
                var planReq = new DataResponse();

                if (request.service == "9 Mobile" && request.SubCategoryId == 5)
                {
                    //planReq.channel = "MOBILE";
                    //planReq.service = "9mobiledata";
                }
                else
                {
                    //planReq.channel = "MOBILE";
                    //planReq.service = walletServiceData.ServiceName.ToLower() + "data".ToLower();
                }

                var req = JsonConvert.SerializeObject(planReq);
                var res = await GetZealvendDate(url);
                var JsonResult = JsonConvert.DeserializeObject<DataResponse>(res);
                if (JsonResult.status == "success")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.dataResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public async Task<string> GetZealvendDate(string url)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    // var content = new StringContent(req, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _commonService.GetZealvendAccessToken());
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    resBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }

        public async Task<GetZealvendDataRechargeResponse> DataRechargePayment(DataBillPaymentRequest request)
        {
            var result = new GetZealvendDataRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            string serviceName = string.Empty;
            string customer = string.Empty;
            // var comissionAmt = 53.75;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForData(request.SubCategoryId, request.Service);
                var url = AppSetting.ZealPayDataPayEndpoint;// "https://zealvend.com/api/data/vend";
                var token = await _commonService.GetZealvendAccessToken();
                var userBalance = await _providusExpresssWalletService.GetVirtualAccount(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.wallet.availableBalance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    string customerNumber = request.phone.Substring(0, 1);
                    if (customerNumber != "0")
                    {
                        customer = "0" + request.phone;
                    }
                    else
                    {
                        customer = request.phone;
                    }

                    if (request.Service == "9 Mobile" && request.SubCategoryId == 5)
                    {
                        serviceName = "9mobiledata";
                    }
                    else
                    {
                        serviceName = walletServiceData.BillerName;
                    }

                    var dataPaymentRequest = new DataPaymentRequest
                    {
                        bundle = request.code,
                        network = serviceName,
                        number = request.phone,
                        referrence = invoiceNumber.InvoiceNumber,
                    };
                    var req = JsonConvert.SerializeObject(dataPaymentRequest);
                    var res = await _thirdParty.PostDataZealvend(req, url, token);
                    var JsonResult = JsonConvert.DeserializeObject<DataPaymentResponse>(res.JsonString);
                    //var JsonResult1 = JsonConvert.DeserializeObject<BillPaymentForData>(JsonResult.data);
                    if (res.StatusCode == 201 && JsonResult.status == "success")
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
                        reqTran.TransactionId = invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                    }
                    else
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
                        reqTran.TransactionId = invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                    }
                    await _transactionsRepository.InsertTransactions(reqTran);
                    if (JsonResult.status == "success")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        //result.dataRechargeResponse = JsonResult;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 7;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
