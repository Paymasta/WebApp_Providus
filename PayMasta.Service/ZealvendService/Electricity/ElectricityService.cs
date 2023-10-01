using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Repository.ItexRepository;
using PayMasta.Repository.Transactions;
using PayMasta.Service.Common;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.ItexVM;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.ElectricityVM;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.ZealvendBillsVM;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace PayMasta.Service.ZealvendService.Electricity
{
    public class ElectricityService : IElectricityService
    {
        private readonly IExpressWalletThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly ICommonService _commonService;
        private readonly IItexRepository _itexRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IProvidusExpresssWalletService _providusExpresssWalletService;
        public ElectricityService()
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

        public async Task<OperatorResponse> GetElectricityOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetZealElectricityRechargeResponse> ElectricityRechargePayment(ElectricityBillPaymentRequest request)
        {
            var result = new GetZealElectricityRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            var comissionAmt = 107.5;
            string customer = string.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _providusExpresssWalletService.GetVirtualAccount(userData.Guid);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity1(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.ZealPayElectricityEndpoint;// + "https://zealvend.com/api/power/vend";
                var token = await _commonService.GetZealvendAccessToken();
                var userBalance = await _providusExpresssWalletService.GetVirtualAccount(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.wallet.availableBalance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    if (request.phone != null)
                    {


                        string customerNumber = request.phone.Substring(0, 1);
                        if (customerNumber != "0")
                        {
                            customer = "0" + request.phone;
                        }
                        else
                        {
                            customer = request.phone;
                        }

                        var electicityReq = new ElectricityRequest
                        {
                            amount = Convert.ToDecimal(request.Amount),
                            disco = walletServiceData.BillerName,
                            meter_number = request.meterNo
                        };
                        var req = JsonConvert.SerializeObject(electicityReq);
                        var res = await _thirdParty.PostDataZealvend(req, url, token);
                        var JsonResult = JsonConvert.DeserializeObject<ElectricityResponse>(res.JsonString);
                        if (res.StatusCode == 201 && JsonResult.status == "success")
                        {
                            var debitRes = await _providusExpresssWalletService.DebitCustomerWalletForBills(userData.Guid, requestedAmt.ToString(), invoiceNumber.InvoiceNumber);
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            reqTran.AccountNo = request.phone + "," + request.meterNo;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = "107.5";
                            reqTran.IsAmountPaid = debitRes;
                            //if (responseTran != null && responseTran.code == "success")
                            //{
                            //    reqTran.CommisionAmount = "107.5";
                            //}
                            //else
                            //{
                            //    reqTran.CommisionAmount = string.Empty;
                            //}
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
                            reqTran.TotalAmount = request.Amount.ToString();
                            reqTran.TransactionId = JsonResult.data.referrence;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = request.meterNo;
                            reqTran.WalletAmount = string.Empty;
                        }
                        else
                        {
                            reqTran.AccountNo = request.phone + "," + request.meterNo;
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
                            reqTran.TotalAmount = request.Amount.ToString();
                            reqTran.TransactionId = invoiceNumber.InvoiceNumber;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = request.meterNo;
                            reqTran.WalletAmount = string.Empty;
                        }
                        await _transactionsRepository.InsertTransactions(reqTran);
                        if (JsonResult.status == "success")
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.electricityPurchaseResponse = JsonResult;
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
                        result.RstKey = 99;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 991;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<MeterVerifyResponse> MeterVerify(VerifyRequest request)
        {
            var result = new MeterVerifyResponse();
            try
            {
                var url = AppSetting.ZealPayElectricityMeterEndpoint;// "https://zealvend.com/api/power/verify";
                var token = await _commonService.GetZealvendAccessToken();
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                var meterReq = new MeterVerifyRequest
                {
                    disco = request.disco,
                    meter_number = request.meterNo
                };
                var req = JsonConvert.SerializeObject(meterReq);
                var res = await _thirdParty.PostDataZealvend(req, url, token);
                if (res.StatusCode == 200)
                {
                    result = JsonConvert.DeserializeObject<MeterVerifyResponse>(res.JsonString);
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
