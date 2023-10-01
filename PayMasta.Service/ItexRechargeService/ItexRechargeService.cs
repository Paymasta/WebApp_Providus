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
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.ElectricityVM;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.MultiChoicePurchaseVM;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.PurchaseInternetVM;
using PayMasta.ViewModel.PurchaseStarLineVM;
using PayMasta.ViewModel.ValidateMultichoiceVM;
using PayMasta.ViewModel.ValidateStartimesVM;
using PayMasta.ViewModel.ValidatInternetVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ItexRechargeService
{
    public class ItexRechargeService : IItexRechargeService
    {
        private readonly IThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly ICommonService _commonService;
        private readonly IItexRepository _itexRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IVirtualAccountService _virtualAccountService;

        public ItexRechargeService()
        {
            _accountRepository = new AccountRepository();
            _thirdParty = new ThirdPartyService();
            _commonReporsitory = new CommonReporsitory();
            _transactionsRepository = new TransactionsRepository();
            _itexRepository = new ItexRepository();
            _commonService = new CommonService();
            _virtualAccountService = new VirtualAccountService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<GetVTUResponse> AirtimePayment(VTUBillPaymentRequest request)
        {
            var result = new GetVTUResponse();
            var reqTran = new WalletTransaction();

            string customer = string.Empty;
            //st.Append("ss");
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.PurchaseVtu;
                string token = tokenDetail.AuthToken;
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
                var vtuReq = new VTURequest
                {
                    amount = request.Amount,
                    bonusAmount = 0, //request.bonusAmount,
                    channel = "MOBILE",
                    narration = "airtime",
                    clientReference = invoiceNumber.InvoiceNumber,
                    paymentMethod = "CASE",
                    phone = customer,
                    redeemBonus = request.redeemBonus,
                    service = walletServiceData.BillerName.ToLower(),
                    sourceAccountNumber = tokenDetail.AccountNumber,
                    transactionPin = userData.WalletPin.ToString(),
                };
                var req = JsonConvert.SerializeObject(vtuReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<VTUResponse>(res);
                if (JsonResult.code == "51")
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                    result.vTUResponse = JsonResult;
                    return result;
                }
                else
                {
                    var JsonResult1 = JsonConvert.DeserializeObject<ResponseForAirtimeRe>(JsonResult.data);
                    if (JsonResult != null && JsonResult.code == "00")
                    {

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
                        reqTran.TransactionId = JsonResult1.transactionID;
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
                        reqTran.TransactionId = JsonResult1.transactionID;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                    }
                    await _transactionsRepository.InsertTransactions(reqTran);
                    if (JsonResult.code == "00")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.vTUResponse = JsonResult;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.vTUResponse = JsonResult;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetDataRechargeResponse> DataRechargePayment(DataBillPaymentRequest request)
        {
            var result = new GetDataRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            string serviceName = string.Empty;
            string customer = string.Empty;
            // var comissionAmt = 53.75;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForData(request.SubCategoryId, request.Service);
                var url = AppSetting.DataPurchase;
                string token = tokenDetail.AuthToken;
                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
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
                        serviceName = walletServiceData.ServiceName.ToLower() + "data".ToLower();
                    }
                    var vtuReq = new DataRechargeRequest
                    {
                        bonusAmount = request.bonusAmount,
                        card = { },
                        code = request.code,
                        productCode = request.productCode,
                        redeemBonus = request.redeemBonus,
                        narration = walletServiceData.BankCode.ToLower(),
                        clientReference = invoiceNumber.InvoiceNumber,
                        paymentMethod = "cash",
                        phone = customer,
                        service = serviceName,
                        sourceAccountNumber = tokenDetail.AccountNumber,
                        transactionPin = userData.WalletPin.ToString(),
                        charges = "0.0"
                    };
                    var req = JsonConvert.SerializeObject(vtuReq);
                    var res = await _thirdParty.postOperatorList(req, url, token);
                    var JsonResult = JsonConvert.DeserializeObject<DataRechargeResponse>(res);
                    var JsonResult1 = JsonConvert.DeserializeObject<BillPaymentForData>(JsonResult.data);
                    if (JsonResult != null && JsonResult.code == "00")
                    {
                        //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                        //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
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
                        reqTran.TransactionId = JsonResult1.transactionID;
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
                        reqTran.TransactionId = JsonResult1.transactionID != null ? JsonResult1.transactionID : invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                    }
                    await _transactionsRepository.InsertTransactions(reqTran);
                    if (JsonResult.code == "00")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.dataRechargeResponse = JsonResult;
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

        public async Task<GetElectricityRechargeResponse> ElectricityRechargePayment(ElectricityBillPaymentRequest request)
        {
            var result = new GetElectricityRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            var comissionAmt = 50;
            string customer = string.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity1(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.ElectricityPurchase;
                string token = tokenDetail.AuthToken;

                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    if (request.phone != null)
                    {
                        var planReq = new ValidateMeter
                        {
                            accountType = walletServiceData.AccountType,
                            amount = request.Amount.ToString(),
                            meterNo = request.meterNo,
                            channel = "MOBILE",
                            service = walletServiceData.BillerName.ToLower(),
                        };
                        var meterValidRes = await MeterValidate(planReq, token);
                        if (meterValidRes.validateMeterResponse.code == "00")
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

                            var vtuReq = new ElectricityPurchaseRequest
                            {
                                bonusAmount = request.bonusAmount,
                                card = { },
                                productCode = meterValidRes.validateMeterResponse.data.productCode,
                                redeemBonus = request.redeemBonus,
                                narration = "electricity",
                                clientReference = invoiceNumber.InvoiceNumber,
                                paymentMethod = "cash",
                                service = walletServiceData.BillerName.ToLower(),
                                sourceAccountNumber = tokenDetail.AccountNumber,
                                transactionPin = userData.WalletPin.ToString(),
                                amount = request.Amount,
                                customerPhoneNumber = customer,
                                charges = comissionAmt.ToString(),
                            };
                            var req = JsonConvert.SerializeObject(vtuReq);
                            var res = await _thirdParty.postOperatorList(req, url, token);
                            var JsonResult = JsonConvert.DeserializeObject<ElectricityPurchaseResponse>(res);
                            if (JsonResult != null && JsonResult.code == "00")
                            {
                                //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                                //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                                reqTran.AccountNo = request.phone;
                                reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                                reqTran.BankBranchCode = string.Empty;
                                reqTran.BankTransactionId = string.Empty;
                                reqTran.BenchmarkCharges = 0;
                                reqTran.Comments = string.Empty;
                                reqTran.CommisionAmount = "107.5";
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
                                reqTran.TransactionId = JsonResult.data.transactionUniqueNumber;
                                reqTran.TransactionType = "DEBIT";
                                reqTran.TransactionTypeInfo = 0;
                                reqTran.UpdatedAt = DateTime.UtcNow;
                                reqTran.UpdatedBy = userData.Id;
                                reqTran.VoucherCode = request.meterNo;
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
                                reqTran.TotalAmount = request.Amount.ToString();
                                reqTran.TransactionId = JsonResult.data.transactionUniqueNumber;
                                reqTran.TransactionType = "DEBIT";
                                reqTran.TransactionTypeInfo = 0;
                                reqTran.UpdatedAt = DateTime.UtcNow;
                                reqTran.UpdatedBy = userData.Id;
                                reqTran.VoucherCode = request.meterNo;
                                reqTran.WalletAmount = string.Empty;
                            }
                            await _transactionsRepository.InsertTransactions(reqTran);
                            if (JsonResult.code == "00")
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
                            result.RstKey = 992;
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

        public async Task<GetMultiChoiceRechargeResponse> MultiChoiceRechargePayment(MultiChoicePurchaseBillPaymentRequest request)
        {
            var result = new GetMultiChoiceRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            var comissionAmt = 50;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.MultichoicePurchase;
                string token = tokenDetail.AuthToken;
                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    var reqValidation = new ValidatRequest
                    {
                        account = request.SmartCardCode,
                        amount = request.Amount,
                        channel = "MOBILE",
                        service = walletServiceData.BillerName,
                        smartCardCode = request.SmartCardCode,
                        type = walletServiceData.AccountType
                    };
                    var validateData = await ValidateMultichoice(reqValidation, token);
                    if (validateData.validateMeterResponse.code == "00")
                    {
                        var vtuReq = new MultiChoicePurchaseRequest
                        {
                            bonusAmount = request.bonusAmount,
                            card = { },
                            productCode = validateData.validateMeterResponse.data.productCode,
                            redeemBonus = request.redeemBonus,
                            narration = "multichoice",
                            clientReference = invoiceNumber.InvoiceNumber,
                            paymentMethod = "CASE",
                            service = walletServiceData.BillerName.ToLower(),
                            sourceAccountNumber = tokenDetail.AccountNumber,
                            transactionPin = userData.WalletPin.ToString(),
                            totalAmount = request.Amount,
                            phone = request.phone,
                            channel = "B2B",
                            code = request.code,
                            productMonths = 1,
                            renew = true,
                            charges = comissionAmt.ToString()
                        };
                        var req = JsonConvert.SerializeObject(vtuReq);
                        var res = await _thirdParty.postOperatorList(req, url, token);
                        var JsonResult = JsonConvert.DeserializeObject<MultiChoicePurchaseResponse>(res);
                        var JsonResult1 = JsonConvert.DeserializeObject<DataPurchaseResponse>(JsonResult.data);
                        if (JsonResult != null && JsonResult.code == "00")
                        {
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            reqTran.AccountNo = request.phone;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = "107.5";
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
                            reqTran.TransactionId = JsonResult1.externalReference != null ? JsonResult1.externalReference : invoiceNumber.InvoiceNumber;
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
                            reqTran.TotalAmount = request.Amount.ToString();
                            reqTran.TransactionId = JsonResult1.externalReference != null ? JsonResult1.externalReference : invoiceNumber.InvoiceNumber;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = "";
                            reqTran.WalletAmount = string.Empty;
                        }
                        await _transactionsRepository.InsertTransactions(reqTran);
                        if (JsonResult.code == "00")
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.multiChoicePurchaseResponse = JsonResult;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.RstKey = 2;
                            result.multiChoicePurchaseResponse = JsonResult;
                        }
                    }
                    else
                    {
                        //write code for else
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetStarLinePurchaseRechargeResponse> StarlineRechargePayment(StarLinePurchaseBillPaymentRequest request)
        {
            var result = new GetStarLinePurchaseRechargeResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            var comissionAmt = 50;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.SubscribeStartimes;
                string token = tokenDetail.AuthToken;

                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();

                    var validatReq = new ValidatStarTimesRequest
                    {
                        type = "default",
                        service = walletServiceData.BillerName.ToLower(),
                        account = request.SmartCardCode,
                        amount = request.Amount,
                        channel = "MOBILE",
                        smartCardCode = request.SmartCardCode,
                    };
                    var valRes = await ValidateStartimes(validatReq, token);
                    if (valRes.validateMeterResponse.code == "00")
                    {
                        var vtuReq = new StarLinePurchaseReqeust
                        {
                            bonusAmount = request.bonusAmount,
                            card = { },
                            productCode = valRes.validateMeterResponse.data.productCode,
                            redeemBonus = request.redeemBonus,
                            narration = "startimes",
                            clientReference = invoiceNumber.InvoiceNumber,
                            paymentMethod = "CASE",
                            service = walletServiceData.BillerName.ToLower(),
                            sourceAccountNumber = tokenDetail.AccountNumber,
                            transactionPin = userData.WalletPin.ToString(),
                            amount = Convert.ToDouble(request.Amount),
                            phone = request.phone,
                            bouquet = "BASIC",
                            cycle = "daily",
                            charges = comissionAmt.ToString(),
                        };
                        var req = JsonConvert.SerializeObject(vtuReq);
                        var res = await _thirdParty.postOperatorList(req, url, token);
                        var JsonResult = JsonConvert.DeserializeObject<StarLinePurchaseResponse>(res);
                        if (JsonResult != null && JsonResult.code == "00")
                        {
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            reqTran.AccountNo = request.phone;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = "107.5";
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
                            reqTran.TransactionId = JsonResult.data.externalReference;
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
                            reqTran.TotalAmount = request.Amount.ToString();
                            reqTran.TransactionId = JsonResult.data.externalReference;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = "";
                            reqTran.WalletAmount = string.Empty;
                        }
                        await _transactionsRepository.InsertTransactions(reqTran);
                        if (JsonResult.code == "00")
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.starLinePurchaseResponse = JsonResult;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.RstKey = 2;
                        }
                    }
                    else
                    {
                        //condition code;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetInternetPurchaseRechargeResponse> InternetRechargePayment(InternetPurchaseBillPaymentRequest request)
        {
            var result = new GetInternetPurchaseRechargeResponse();
            var reqTran = new WalletTransaction();
            var JsonResult = new InternetPurchaseResponse();
            var responseTran = new WalletToWalletTransferResponse();
            //st.Append("ss");
            var comissionAmt = 50;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.SubscribeInternet;
                string token = tokenDetail.AuthToken;

                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                //belwo if condition for checking current balance
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();

                    var validate = new ValidateInternetRequest
                    {
                        account = request.phone,
                        channel = "MOBILE",
                        service = walletServiceData.BillerName.ToLower(),
                        type = walletServiceData.AccountType
                    };
                    var validateRes = await GetInternetValidation(validate, token);
                    if (validateRes.validateInternetResponse.code == "00")
                    {

                        var vtuReq = new InternetPurchaseRequest
                        {
                            bonusAmount = 0,// request.bonusAmount,
                            card = { },
                            productCode = validateRes.validateInternetResponse.data.productCode,
                            redeemBonus = request.redeemBonus,
                            narration = "Internet",
                            clientReference = invoiceNumber.InvoiceNumber,
                            paymentMethod = "CASE",
                            service = walletServiceData.BillerName.ToLower(),
                            sourceAccountNumber = "3129637109",//tokenDetail.AccountNumber,
                            transactionPin = userData.WalletPin.ToString(),
                            amount = "50",// request.Amount,
                            phone = request.phone,
                            code = request.code,
                            type = "subscription",
                            charges = comissionAmt.ToString(),
                        };
                        var req = JsonConvert.SerializeObject(vtuReq);
                        var res = await _thirdParty.postOperatorList(req, url, token);
                        JsonResult = JsonConvert.DeserializeObject<InternetPurchaseResponse>(res);
                        if (JsonResult != null && JsonResult.code == "00")
                        {
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            reqTran.AccountNo = request.phone;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = "107.5";
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
                            reqTran.TransactionId = JsonResult.data.transactionID;
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
                            reqTran.TotalAmount = request.Amount.ToString();
                            reqTran.TransactionId = JsonResult.data.transactionID;
                            reqTran.TransactionType = "DEBIT";
                            reqTran.TransactionTypeInfo = 0;
                            reqTran.UpdatedAt = DateTime.UtcNow;
                            reqTran.UpdatedBy = userData.Id;
                            reqTran.VoucherCode = "";
                            reqTran.WalletAmount = string.Empty;
                        }
                       ;
                        if (JsonResult.code == "00" && await _transactionsRepository.InsertTransactions(reqTran) > 0)
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.internetPurchaseResponse = JsonResult;
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
                        result.RstKey = 3;
                        result.internetPurchaseResponse = JsonResult;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private async Task<GetValidateInternetResponse> GetInternetValidation(ValidateInternetRequest request, string token)
        {
            var result = new GetValidateInternetResponse();
            try
            {
                //var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.InternetValidation;
                // string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";


                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidateInternetResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateInternetResponse = JsonResult;
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

        private async Task<GetValidateBouquestResponses> ValidateMultichoice(ValidatRequest request, string token)
        {
            var result = new GetValidateBouquestResponses();
            try
            {
                //var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.Validatemultichoice;
                //string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";


                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidatResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
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

        private async Task<GetValidateStarTimesResponses> ValidateStartimes(ValidatStarTimesRequest request, string token)
        {
            var result = new GetValidateStarTimesResponses();
            try
            {
                //    var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //    var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //    var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.Validatestartimes;
                //  string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";
                //var planReq = new ValidatStarTimesRequest
                //{
                //    type = request.AccountType,
                //    service = walletServiceData.BillerName.ToLower(),
                //    account = request.Account,
                //    amount = request.Amount,
                //    channel = "MOBILE",
                //    smartCardCode = request.SmartCardCode,
                //};
                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidatStarTimesResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
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


        private async Task<GetValidateMeterResponseResponses> MeterValidate(ValidateMeter request, string token)
        {
            var result = new GetValidateMeterResponseResponses();
            try
            {
                //var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.meterValidate;
                // string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";


                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidateMeterResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
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

        public async Task<GetWalletToBankResponse> WalletToBankTransfer(WalletToBankPaymentRequest request)
        {
            var result = new GetWalletToBankResponse();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            string customer = string.Empty;
            double comissionAmt = 0;
            double comissionAmtWithOutVat = 0;
            //st.Append("ss");
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service);
                var url = AppSetting.SendMoneyToBank;
                string token = tokenDetail.AuthToken;
                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                if (Convert.ToDecimal(request.Amount) > 1 && Convert.ToDecimal(request.Amount) <= 5000)
                {
                    comissionAmt = Convert.ToDouble(21.5);
                    comissionAmtWithOutVat = Convert.ToDouble(20);
                }
                else if (Convert.ToDecimal(request.Amount) > 5000 && Convert.ToDecimal(request.Amount) <= 50000)
                {
                    comissionAmt = Convert.ToDouble(26.87);
                    comissionAmtWithOutVat = Convert.ToDouble(25);
                }
                else if (Convert.ToDecimal(request.Amount) > 50000 && Convert.ToDecimal(request.Amount) <= 50000)
                {
                    comissionAmt = Convert.ToDouble(53.75);
                    comissionAmtWithOutVat = Convert.ToDouble(50);
                }
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    var vtuReq = new WalletToBankRequest
                    {
                        accountNumber = request.AccountNumber,
                        amount = Convert.ToDecimal(request.Amount),
                        channel = "wallettoBank",
                        beneficiaryName = request.beneficiaryName,
                        destBankCode = request.destBankCode,
                        isToBeSaved = false,
                        narration = "Transfer",
                        phoneNumber = tokenDetail.PhoneNumber,
                        pin = userData.WalletPin.ToString(),
                        sourceAccountNumber = tokenDetail.AccountNumber,
                        sourceBankCode = "DMB",
                        transRef = invoiceNumber.InvoiceNumber,
                        specificChannel = "sendMoney",
                        charges = comissionAmtWithOutVat.ToString(),
                    };
                    var req = JsonConvert.SerializeObject(vtuReq);
                    var res = await _thirdParty.postOperatorList(req, url, token);
                    var JsonResult = JsonConvert.DeserializeObject<WalletToBankResponse>(res);
                    if (JsonResult.code == "51")
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.vTUResponse = JsonResult;
                        return result;
                    }
                    else
                    {
                        var JsonResult1 = JsonConvert.DeserializeObject<WalletToBankResponse>(res);
                        if (JsonResult != null && JsonResult.code == "00")
                        {
                            // if()
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            reqTran.AccountNo = request.AccountNumber;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = string.Empty;
                            reqTran.CommisionAmount = comissionAmt.ToString();
                            //if (responseTran != null && responseTran.code == "success")
                            //{
                            //    reqTran.CommisionAmount = comissionAmt.ToString();
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
                            reqTran.AccountNo = request.AccountNumber;
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
                        if (JsonResult.code == "00")
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.vTUResponse = JsonResult;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.RstKey = 2;
                            result.vTUResponse = JsonResult;
                        }
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

        public async Task<GetVerifyAccountResponse> VerifyAccount(VerifyAccountRequest request)
        {
            var result = new GetVerifyAccountResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.VerifyAccount;
                // string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";

                var reqVerifyAccount = new VerifyAccount
                {
                    accountNumber = request.AccountNumber,
                    accountType = "bank",
                    bankCode = request.BankCode,
                };

                var req = JsonConvert.SerializeObject(reqVerifyAccount);
                var res = await _thirdParty.postOperatorList(req, url, tokenDetail.AuthToken);
                var JsonResult = JsonConvert.DeserializeObject<VerifyAccountResponse>(res);
                if (JsonResult.accountNumber != null)
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.verifyAccountResponse = JsonResult;
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

        public async Task<GetWalletToBankResponse> WalletToWalletTransfer(WalletToBankPaymentRequest request)
        {
            var result = new GetWalletToBankResponse();
            var reqTran = new WalletTransaction();
            var reqTranCredit = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            string customer = string.Empty;
            double comissionAmt = 0;
            double comissionAmtWithOutVat = 0;
            //st.Append("ss");
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var receiverDetail = await _itexRepository.GetVirtualAccountDetailByVirtualAccountNumber(request.AccountNumber.Trim());
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service);
                var url = AppSetting.SendMoneyToBank;
                if (tokenDetail == null)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.SENDER_NOT_EXIST;
                    return result;
                }
                if (receiverDetail != null && receiverDetail.UserId == userData.Id)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.SELF_WALLET;
                    return result;
                }
                if (receiverDetail == null)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.RECEIVER_NOT_EXIST;
                    return result;
                }
                string token = tokenDetail.AuthToken;
                var userBalance = await _virtualAccountService.GetVirtualAccountBalance(request.UserGuid);
                decimal currentBalance = Convert.ToDecimal(userBalance.Balance);
                if (Convert.ToDecimal(request.Amount) > 1 && Convert.ToDecimal(request.Amount) <= 5000)
                {
                    comissionAmt = Convert.ToDouble(21.5);
                    comissionAmtWithOutVat = Convert.ToDouble(20);
                }
                else if (Convert.ToDecimal(request.Amount) > 5000 && Convert.ToDecimal(request.Amount) <= 50000)
                {
                    comissionAmt = Convert.ToDouble(26.87);
                    comissionAmtWithOutVat = Convert.ToDouble(25);
                }
                else if (Convert.ToDecimal(request.Amount) > 50000 && Convert.ToDecimal(request.Amount) <= 50000)
                {
                    comissionAmt = Convert.ToDouble(53.75);
                    comissionAmtWithOutVat = Convert.ToDouble(50);
                }
                decimal requestedAmt = Convert.ToDecimal(request.Amount) + Convert.ToDecimal(comissionAmt);
                if (currentBalance != 0 && currentBalance > 0 && currentBalance >= requestedAmt)
                {
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    var vtuReq = new WalletToBankRequest
                    {
                        accountNumber = request.AccountNumber,
                        amount = Convert.ToDecimal(request.Amount),
                        channel = "wallettowallet",
                        beneficiaryName = request.beneficiaryName,
                        destBankCode = string.Empty,
                        isToBeSaved = false,
                        narration = "Transfer",
                        phoneNumber = tokenDetail.PhoneNumber,
                        pin = userData.WalletPin.ToString(),
                        sourceAccountNumber = tokenDetail.AccountNumber,
                        sourceBankCode = "DMB",
                        transRef = invoiceNumber.InvoiceNumber,
                        specificChannel = "sendMoney",
                        charges = "0",
                    };
                    var req = JsonConvert.SerializeObject(vtuReq);
                    var res = await _thirdParty.postOperatorList(req, url, token);
                    var JsonResult = JsonConvert.DeserializeObject<WalletToBankResponse>(res);
                    if (JsonResult.code == "51")
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.vTUResponse = JsonResult;
                        return result;
                    }
                    else
                    {
                        var JsonResult1 = JsonConvert.DeserializeObject<WalletToBankResponse>(res);
                        if (JsonResult != null && JsonResult.code == "00")
                        {
                            // if()
                            //var debitDetail = await _commonService.WalletToWalletTransfer(userData.Id, comissionAmt);
                            //responseTran = JsonConvert.DeserializeObject<WalletToWalletTransferResponse>(debitDetail);
                            #region sender 
                            reqTran.AccountNo = request.AccountNumber;
                            reqTran.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTran.BankBranchCode = string.Empty;
                            reqTran.BankTransactionId = string.Empty;
                            reqTran.BenchmarkCharges = 0;
                            reqTran.Comments = request.beneficiaryName;
                            reqTran.CommisionAmount = "0";
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
                            #endregion sender
                            #region receiver 
                            reqTranCredit.AccountNo = request.AccountNumber;
                            reqTranCredit.TransactionStatus = (int)TransactionStatus.Completed;
                            reqTranCredit.BankBranchCode = string.Empty;
                            reqTranCredit.BankTransactionId = string.Empty;
                            reqTranCredit.BenchmarkCharges = 0;
                            reqTranCredit.Comments = userData.FirstName + " " + userData.LastName;
                            reqTranCredit.CommisionAmount = "0";
                            reqTranCredit.CommisionId = 0;
                            reqTranCredit.CommisionPercent = 0;
                            reqTranCredit.CreatedAt = DateTime.UtcNow;
                            reqTranCredit.CreatedBy = userData.Id;
                            reqTranCredit.DisplayContent = string.Empty;
                            reqTranCredit.FlatCharges = 0;
                            reqTranCredit.InvoiceNo = invoiceNumber.InvoiceNumber;
                            reqTranCredit.IsActive = true;
                            reqTranCredit.IsAdminTransaction = false;
                            reqTranCredit.IsBankTransaction = false;
                            reqTranCredit.IsDeleted = false;
                            reqTranCredit.IsUpcomingBillShow = true;
                            reqTranCredit.ReceiverId = receiverDetail.UserId;
                            reqTranCredit.SenderId = receiverDetail.UserId; ;
                            reqTranCredit.ServiceCategoryId = (int)walletServiceData.Id;
                            reqTranCredit.ServiceTax = string.Empty;
                            reqTranCredit.ServiceTaxRate = 0;
                            reqTranCredit.SubCategoryId = walletServiceData.SubCategoryId;
                            reqTranCredit.TotalAmount = request.Amount;
                            reqTranCredit.TransactionId = invoiceNumber.InvoiceNumber;
                            reqTranCredit.TransactionType = "CREDIT";
                            reqTranCredit.TransactionTypeInfo = 0;
                            reqTranCredit.UpdatedAt = DateTime.UtcNow;
                            reqTranCredit.UpdatedBy = userData.Id;
                            reqTranCredit.VoucherCode = "";
                            reqTranCredit.WalletAmount = string.Empty;
                            await _transactionsRepository.InsertTransactions(reqTranCredit);
                            #endregion receiver
                        }
                        else
                        {
                            reqTran.AccountNo = request.AccountNumber;
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
                        if (JsonResult.code == "00")
                        {
                            result.IsSuccess = true;
                            result.RstKey = 1;
                            result.vTUResponse = JsonResult;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.RstKey = 2;
                            result.vTUResponse = JsonResult;
                        }
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
                result.IsSuccess = false;
                result.RstKey = 2;
            }
            return result;
        }
    }
}
