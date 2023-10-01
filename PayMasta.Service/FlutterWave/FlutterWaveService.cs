using Newtonsoft.Json;
using PayMasta.DBEntity.WalletService;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Service.Common;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.ViewModel.AirtimeVM;
using PayMasta.ViewModel.BulkPaymentVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.FlutterWaveVM;
using PayMasta.ViewModel.VirtualAccountVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.FlutterWave
{
    public class FlutterWaveService : IFlutterWaveService
    {
        private readonly IThirdParty _thirdParty;

        private readonly ICommonReporsitory _commonReporsitory;
        private readonly ICommonService _commonService;
        private readonly IAccountRepository _accountRepository;


        public FlutterWaveService()
        {
            //_accountRepository = new AccountRepository();
            //_iSMSUtils = new SMSUtils();
            //_emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _commonReporsitory = new CommonReporsitory();
            _accountRepository = new AccountRepository();
            _commonService = new CommonService();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        //public async Task<CreateVertualAccountResponse> CreateVertualAccount(CreateVertualAccountRequest request)
        //public async Task<string> CreateVertualAccount(CreateVertualAccountRequest request)
        //{
        //   // var result = new CreateVertualAccountResponse();
        //    var req = JsonConvert.SerializeObject(request);
        //    var url = AppSetting.FlutterWaveVertualAccountUrl;
        //    var res = await _thirdParty.CreateVertualAccount(req, url);
        //    //  var JsonResult = JsonConvert.DeserializeObject<CreateVertualAccountResponse>(res);
        //    return res;
        //}
        public async Task<AirtimeResponse> GetAirtimeOperatorList()
        {
            var result = new AirtimeResponse();
            try
            {
                var url = AppSetting.FlutterWaveAirtimeOperator;
                var res = await _thirdParty.getOperatorList(url);
                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        var walletService = await _commonReporsitory.GetWalletServices(item.id);
                        if (walletService == null)
                        {
                            var reqWalletserive = new WalletService
                            {
                                ServiceName = item.name,
                                BankCode = item.biller_code,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 0,
                                SubCategoryId = 1,
                                IsActive = true,
                                IsDeleted = false,
                                ImageUrl = "",
                                HttpVerbs = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 0,
                                RawData = res,
                                CountryCode = item.country,
                                BillerName = item.biller_name,
                                OperatorId = item.id
                            };

                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                        }
                    }
                    var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.AIRTIME);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }

                //var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.AIRTIME);
                //result.RstKey = 1;
                //result.IsSuccess = true;
                //result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<AirtimeResponse> GetDataBundleOperatorList()
        {
            var result = new AirtimeResponse();
            try
            {

                var url = AppSetting.FlutterWaveInternetOperator;
                var res = await _thirdParty.getOperatorList(url);

                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        var walletService = await _commonReporsitory.GetWalletServicesOperator(item.biller_code);
                        if (walletService == null)
                        {
                            var reqWalletserive = new WalletService
                            {
                                ServiceName = item.name,
                                BankCode = item.biller_code,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 0,
                                SubCategoryId = (int)WalletTransactionSubTypes.DATABUNDLE,
                                IsActive = true,
                                IsDeleted = false,
                                ImageUrl = "",
                                HttpVerbs = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 0,
                                RawData = res,
                                CountryCode = item.country,
                                BillerName = item.biller_name,
                                OperatorId = item.id
                            };

                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                        }
                    }
                    var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.DATABUNDLE);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
                //var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.DATABUNDLE);
                //result.RstKey = 1;
                //result.IsSuccess = true;
                //result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<AirtimeResponse> GetWifiInternetOperatorList()
        {
            var result = new AirtimeResponse();
            try
            {

                var url = AppSetting.FlutterWaveInternetWifiOperator;
                var res = await _thirdParty.getOperatorList(url);

                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        var walletService = await _commonReporsitory.GetWalletServicesOperator(item.biller_code);
                        if (walletService == null)
                        {
                            var reqWalletserive = new WalletService
                            {
                                ServiceName = item.name,
                                BankCode = item.biller_code,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 0,
                                SubCategoryId = (int)WalletTransactionSubTypes.INTERNET,
                                IsActive = true,
                                IsDeleted = false,
                                ImageUrl = "",
                                HttpVerbs = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 0,
                                RawData = res,
                                CountryCode = item.country,
                                BillerName = item.biller_name,
                                OperatorId = item.id
                            };

                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                        }
                    }
                    var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.INTERNET);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
                //var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.INTERNET);
                //result.RstKey = 1;
                //result.IsSuccess = true;
                //result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        //WIP
        public async Task<AirtimeResponse> GetTVOperatorList()
        {
            var result = new AirtimeResponse();
            try
            {
                var url = AppSetting.FlutterWaveTvOperator;
                var res = await _thirdParty.getOperatorList(url);
                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        var walletService = await _commonReporsitory.GetWalletServicesOperator(item.biller_code);
                        if (walletService == null)
                        {
                            var reqWalletserive = new WalletService
                            {
                                ServiceName = item.name,
                                BankCode = item.biller_code,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 0,
                                SubCategoryId = (int)WalletTransactionSubTypes.TV,
                                IsActive = true,
                                IsDeleted = false,
                                ImageUrl = "",
                                HttpVerbs = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 0,
                                RawData = res,
                                CountryCode = item.country,
                                BillerName = item.biller_name,
                                OperatorId = item.id
                            };

                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                        }
                    }
                    var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.TV);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<AirtimeResponse> GetElectricityOperatorList()
        {
            var result = new AirtimeResponse();
            try
            {
                var url = AppSetting.FlutterWaveElectricityOperator;
                var res = await _thirdParty.getOperatorList(url);
                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        var walletService = await _commonReporsitory.GetWalletServicesOperator(item.biller_code);
                        if (walletService == null)
                        {
                            var reqWalletserive = new WalletService
                            {
                                ServiceName = item.name,
                                BankCode = item.biller_code,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 0,
                                SubCategoryId = (int)WalletTransactionSubTypes.Power,
                                IsActive = true,
                                IsDeleted = false,
                                ImageUrl = "",
                                HttpVerbs = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 0,
                                RawData = res,
                                CountryCode = item.country,
                                BillerName = item.biller_name,
                                OperatorId = item.id
                            };

                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                        }
                    }
                    var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
                //var data = await _commonReporsitory.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                //result.RstKey = 1;
                //result.IsSuccess = true;
                //result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<ProductResponse> FilterOperatorByBillerCode(string BillerCode)
        {
            var result = new ProductResponse();
            var itemEntity = new List<WalletServiceOperatorResponse>();
            try
            {
                var url = AppSetting.FlutterWaveBillerCodefilter + BillerCode;
                var res = await _thirdParty.getOperatorList(url);
                var JsonResult = JsonConvert.DeserializeObject<AirtimeOperatorResponse>(res);
                if (JsonResult.data.Count > 0)
                {

                    JsonResult.data.ForEach(x =>
                    {
                        if (x.id > 0)
                        {
                            itemEntity.Add(
                                new WalletServiceOperatorResponse
                                {
                                    BankCode = x.biller_code,
                                    BillerName = x.biller_name,
                                    ServiceName = x.name,
                                    CountryCode = x.country,
                                    Id = x.id,
                                    Amount = Convert.ToDecimal(x.amount),
                                    fee = Convert.ToDecimal(x.fee),
                                    Commision = Convert.ToDecimal(x.default_commission),
                                    ItemCode = x.item_code,
                                    LabelName = x.label_name,
                                });
                        }
                        else
                        {

                        }
                    });


                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = itemEntity;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.AirtimeRechargeResponse> AirtimeRecharge(AirtimeRechargeRequest request)
        {
            var result = new AirtimeRechargeResponse();
            var walletTransaction = new WalletTransaction();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var invoice = await _commonService.GetInvoiceNumber();
                var walletService = await _commonReporsitory.GetWalletServices(request.BillerCode, request.OperatorId);
                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (walletService == null)
                {
                    result.RstKey = 6;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND;
                    return result;
                }
                if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.Amount))
                {
                    var airtimeRequest = new Airtime
                    {
                        amount = request.Amount,
                        biller_name = walletService.BillerName,
                        country = walletService.CountryCode,
                        customer = request.MobileNumber,
                        recurrence = "ONCE",
                        reference = invoice.InvoiceNumber,
                        type = walletService.BillerName,
                    };
                    var airtimeResult = await Airtime(airtimeRequest);
                    if (airtimeResult != null)
                    {
                        if (airtimeResult.status == "error")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                            result.RstKey = 4;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "success")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                            result.RstKey = 3;
                            result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "pending")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                            result.RstKey = 5;
                            result.Message = ResponseMessages.PENDING_TRANSACTION;
                        }
                        walletTransaction.TotalAmount = request.Amount;
                        walletTransaction.CommisionId = 0;
                        walletTransaction.CommisionAmount = "0";
                        walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                        walletTransaction.ServiceTaxRate = 0;
                        walletTransaction.ServiceTax = "0";
                        walletTransaction.ServiceCategoryId = (int)walletService.Id;
                        walletTransaction.SenderId = userData.Id;
                        walletTransaction.ReceiverId = userData.Id;
                        walletTransaction.AccountNo = request.MobileNumber;
                        walletTransaction.TransactionId = airtimeResult.data.flw_ref + "," + airtimeResult.data.reference;
                        walletTransaction.IsAdminTransaction = false;
                        walletTransaction.IsActive = true;
                        walletTransaction.IsDeleted = false;
                        walletTransaction.CreatedAt = DateTime.UtcNow;
                        walletTransaction.UpdatedAt = DateTime.UtcNow;
                        walletTransaction.Comments = string.Empty;
                        walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                        walletTransaction.TransactionType = "DEBIT";
                        walletTransaction.TransactionTypeInfo = 0;
                        walletTransaction.IsBankTransaction = false;
                        walletTransaction.BankBranchCode = "0";
                        walletTransaction.BankTransactionId = "0";
                        walletTransaction.VoucherCode = "0";
                        walletTransaction.FlatCharges = 0;
                        walletTransaction.BenchmarkCharges = 0;
                        walletTransaction.CommisionPercent = 0;
                        walletTransaction.DisplayContent = string.Empty;
                        walletTransaction.IsUpcomingBillShow = true;
                        walletTransaction.SubCategoryId = walletService.SubCategoryId;
                        var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                        //result.IsSuccess = true;

                    }
                    else
                    {
                        result.RstKey = 8;
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                    }
                    // result.airtimeOperatorResponse = data;
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.AirtimeRecharge> Airtime(Airtime airtime)
        {
            var result = new PayMasta.ViewModel.AirtimeVM.AirtimeRecharge();
            try
            {
                var url = AppSetting.FlutterWaveBillPayMentUrl;
                var jsonReq = JsonConvert.SerializeObject(airtime);
                // var res = await _thirdParty.PostAirtime(jsonReq, url);
                var res = "{\"status\":\"success\",\"message\":\"BillpaymentSuccessful\",\"data\":{\"phone_number\":\"+2349060009334\",\"amount\":100,\"network\":\"MTN\",\"flw_ref\":\"CF-FLYAPI-20220421122252446603\",\"reference\":\"BPUSSD16505437735091845197\"}}";
                result = JsonConvert.DeserializeObject<PayMasta.ViewModel.AirtimeVM.AirtimeRecharge>(res);

            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<BulkRechargeResponse> BulkPayment(BulkRechargeRequest request)
        {
            var result = new BulkRechargeResponse();
            try
            {
                var url = AppSetting.FlutterWaveBulkBillPayMentUrl;
                var jsonReq = JsonConvert.SerializeObject(request);
                // var res = await _thirdParty.PostAirtime(jsonReq, url);
                var res = "{\"status\":\"success\",\"message\":\"BulkbillPaymentwasqueuedforprocessing\",\"data\":{\"batch_reference\":null}}";
                result = JsonConvert.DeserializeObject<BulkRechargeResponse>(res);

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.TVRechargeResponse> TVRecharge(AirtimeRechargeRequest request)
        {
            var result = new TVRechargeResponse();
            var walletTransaction = new WalletTransaction();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var invoice = await _commonService.GetInvoiceNumber();
                var walletService = await _commonReporsitory.GetWalletServices(request.BillerCode, request.OperatorId);
                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (walletService == null)
                {
                    result.RstKey = 6;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND;
                    return result;
                }
                if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.Amount))
                {
                    var airtimeRequest = new Airtime
                    {
                        amount = request.Amount,
                        biller_name = walletService.BillerName,
                        country = walletService.CountryCode,
                        customer = request.MobileNumber,
                        recurrence = "ONCE",
                        reference = invoice.InvoiceNumber,
                        type = walletService.BillerName,
                    };
                    var airtimeResult = await Airtime(airtimeRequest);
                    if (airtimeResult != null)
                    {
                        if (airtimeResult.status == "error")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                            result.RstKey = 4;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "success")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                            result.RstKey = 3;
                            result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "pending")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                            result.RstKey = 5;
                            result.Message = ResponseMessages.PENDING_TRANSACTION;
                        }
                        walletTransaction.TotalAmount = request.Amount;
                        walletTransaction.CommisionId = 0;
                        walletTransaction.CommisionAmount = "0";
                        walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                        walletTransaction.ServiceTaxRate = 0;
                        walletTransaction.ServiceTax = "0";
                        walletTransaction.ServiceCategoryId = (int)walletService.Id;
                        walletTransaction.SenderId = userData.Id;
                        walletTransaction.ReceiverId = userData.Id;
                        walletTransaction.AccountNo = request.MobileNumber;
                        walletTransaction.TransactionId = airtimeResult.data.flw_ref + "," + airtimeResult.data.reference;
                        walletTransaction.IsAdminTransaction = false;
                        walletTransaction.IsActive = true;
                        walletTransaction.IsDeleted = false;
                        walletTransaction.CreatedAt = DateTime.UtcNow;
                        walletTransaction.UpdatedAt = DateTime.UtcNow;
                        walletTransaction.Comments = string.Empty;
                        walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                        walletTransaction.TransactionType = "DEBIT";
                        walletTransaction.TransactionTypeInfo = 0;
                        walletTransaction.IsBankTransaction = false;
                        walletTransaction.BankBranchCode = "0";
                        walletTransaction.BankTransactionId = "0";
                        walletTransaction.VoucherCode = "0";
                        walletTransaction.FlatCharges = 0;
                        walletTransaction.BenchmarkCharges = 0;
                        walletTransaction.CommisionPercent = 0;
                        walletTransaction.DisplayContent = string.Empty;
                        walletTransaction.IsUpcomingBillShow = true;
                        walletTransaction.SubCategoryId = walletService.SubCategoryId;
                        var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                        //result.IsSuccess = true;

                    }
                    else
                    {
                        result.RstKey = 8;
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                    }
                    // result.airtimeOperatorResponse = data;
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse> InternetRecharge(AirtimeRechargeRequest request)
        {
            var result = new InternetRechargeResponse();
            var walletTransaction = new WalletTransaction();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var invoice = await _commonService.GetInvoiceNumber();
                var walletService = await _commonReporsitory.GetWalletServices(request.BillerCode, request.OperatorId);
                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (walletService == null)
                {
                    result.RstKey = 6;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND;
                    return result;
                }
                if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.Amount))
                {
                    var airtimeRequest = new Airtime
                    {
                        amount = request.Amount,
                        biller_name = walletService.BillerName,
                        country = walletService.CountryCode,
                        customer = request.MobileNumber,
                        recurrence = "ONCE",
                        reference = invoice.InvoiceNumber,
                        type = walletService.BillerName,
                    };
                    var airtimeResult = await Airtime(airtimeRequest);
                    if (airtimeResult != null)
                    {
                        if (airtimeResult.status == "error")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                            result.RstKey = 4;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "success")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                            result.RstKey = 3;
                            result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "pending")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                            result.RstKey = 5;
                            result.Message = ResponseMessages.PENDING_TRANSACTION;
                        }
                        walletTransaction.TotalAmount = request.Amount;
                        walletTransaction.CommisionId = 0;
                        walletTransaction.CommisionAmount = "0";
                        walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                        walletTransaction.ServiceTaxRate = 0;
                        walletTransaction.ServiceTax = "0";
                        walletTransaction.ServiceCategoryId = (int)walletService.Id;
                        walletTransaction.SenderId = userData.Id;
                        walletTransaction.ReceiverId = userData.Id;
                        walletTransaction.AccountNo = request.MobileNumber;
                        walletTransaction.TransactionId = airtimeResult.data.flw_ref + "," + airtimeResult.data.reference;
                        walletTransaction.IsAdminTransaction = false;
                        walletTransaction.IsActive = true;
                        walletTransaction.IsDeleted = false;
                        walletTransaction.CreatedAt = DateTime.UtcNow;
                        walletTransaction.UpdatedAt = DateTime.UtcNow;
                        walletTransaction.Comments = string.Empty;
                        walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                        walletTransaction.TransactionType = "DEBIT";
                        walletTransaction.TransactionTypeInfo = 0;
                        walletTransaction.IsBankTransaction = false;
                        walletTransaction.BankBranchCode = "0";
                        walletTransaction.BankTransactionId = "0";
                        walletTransaction.VoucherCode = "0";
                        walletTransaction.FlatCharges = 0;
                        walletTransaction.BenchmarkCharges = 0;
                        walletTransaction.CommisionPercent = 0;
                        walletTransaction.DisplayContent = string.Empty;
                        walletTransaction.IsUpcomingBillShow = true;
                        walletTransaction.SubCategoryId = walletService.SubCategoryId;
                        var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                        //result.IsSuccess = true;

                    }
                    else
                    {
                        result.RstKey = 8;
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                    }
                    // result.airtimeOperatorResponse = data;
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public async Task<PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse> DataBundleRecharge(AirtimeRechargeRequest request)
        {
            var result = new DataBundleRechargeResponse();
            var walletTransaction = new WalletTransaction();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var invoice = await _commonService.GetInvoiceNumber();
                var walletService = await _commonReporsitory.GetWalletServices(request.BillerCode, request.OperatorId);
                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (walletService == null)
                {
                    result.RstKey = 6;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND;
                    return result;
                }
                if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.Amount))
                {
                    var airtimeRequest = new Airtime
                    {
                        amount = request.Amount,
                        biller_name = walletService.BillerName,
                        country = walletService.CountryCode,
                        customer = request.MobileNumber,
                        recurrence = "ONCE",
                        reference = invoice.InvoiceNumber,
                        type = walletService.BillerName,
                    };
                    var airtimeResult = await Airtime(airtimeRequest);
                    if (airtimeResult != null)
                    {
                        if (airtimeResult.status == "error")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                            result.RstKey = 4;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "success")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                            result.RstKey = 3;
                            result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "pending")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                            result.RstKey = 5;
                            result.Message = ResponseMessages.PENDING_TRANSACTION;
                        }
                        walletTransaction.TotalAmount = request.Amount;
                        walletTransaction.CommisionId = 0;
                        walletTransaction.CommisionAmount = "0";
                        walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                        walletTransaction.ServiceTaxRate = 0;
                        walletTransaction.ServiceTax = "0";
                        walletTransaction.ServiceCategoryId = (int)walletService.Id;
                        walletTransaction.SenderId = userData.Id;
                        walletTransaction.ReceiverId = userData.Id;
                        walletTransaction.AccountNo = request.MobileNumber;
                        walletTransaction.TransactionId = airtimeResult.data.flw_ref + "," + airtimeResult.data.reference;
                        walletTransaction.IsAdminTransaction = false;
                        walletTransaction.IsActive = true;
                        walletTransaction.IsDeleted = false;
                        walletTransaction.CreatedAt = DateTime.UtcNow;
                        walletTransaction.UpdatedAt = DateTime.UtcNow;
                        walletTransaction.Comments = string.Empty;
                        walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                        walletTransaction.TransactionType = "DEBIT";
                        walletTransaction.TransactionTypeInfo = 0;
                        walletTransaction.IsBankTransaction = false;
                        walletTransaction.BankBranchCode = "0";
                        walletTransaction.BankTransactionId = "0";
                        walletTransaction.VoucherCode = "0";
                        walletTransaction.FlatCharges = 0;
                        walletTransaction.BenchmarkCharges = 0;
                        walletTransaction.CommisionPercent = 0;
                        walletTransaction.DisplayContent = string.Empty;
                        walletTransaction.IsUpcomingBillShow = true;
                        walletTransaction.SubCategoryId = walletService.SubCategoryId;
                        var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                        //result.IsSuccess = true;

                    }
                    else
                    {
                        result.RstKey = 8;
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                    }
                    // result.airtimeOperatorResponse = data;
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse> ElectriCityBillRecharge(AirtimeRechargeRequest request)
        {
            var result = new ElectriCityBillRechargeResponse();
            var walletTransaction = new WalletTransaction();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var invoice = await _commonService.GetInvoiceNumber();
                var walletService = await _commonReporsitory.GetWalletServices(request.BillerCode, request.OperatorId);
                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.Amount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (walletService == null)
                {
                    result.RstKey = 6;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.TRANSACTION_SERVICE_CATEGORY_NOT_FOUND;
                    return result;
                }
                if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.Amount))
                {
                    var airtimeRequest = new Airtime
                    {
                        amount = request.Amount,
                        biller_name = walletService.BillerName,
                        country = walletService.CountryCode,
                        customer = request.MobileNumber,
                        recurrence = "ONCE",
                        reference = invoice.InvoiceNumber,
                        type = walletService.BillerName,
                    };
                    var airtimeResult = await Airtime(airtimeRequest);
                    if (airtimeResult != null)
                    {
                        if (airtimeResult.status == "error")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                            result.RstKey = 4;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "success")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                            result.RstKey = 3;
                            result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                            result.IsSuccess = true;
                        }
                        else if (airtimeResult.status == "pending")
                        {
                            walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                            result.RstKey = 5;
                            result.Message = ResponseMessages.PENDING_TRANSACTION;
                        }
                        walletTransaction.TotalAmount = request.Amount;
                        walletTransaction.CommisionId = 0;
                        walletTransaction.CommisionAmount = "0";
                        walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                        walletTransaction.ServiceTaxRate = 0;
                        walletTransaction.ServiceTax = "0";
                        walletTransaction.ServiceCategoryId = (int)walletService.Id;
                        walletTransaction.SenderId = userData.Id;
                        walletTransaction.ReceiverId = userData.Id;
                        walletTransaction.AccountNo = request.MobileNumber;
                        walletTransaction.TransactionId = airtimeResult.data.flw_ref + "," + airtimeResult.data.reference;
                        walletTransaction.IsAdminTransaction = false;
                        walletTransaction.IsActive = true;
                        walletTransaction.IsDeleted = false;
                        walletTransaction.CreatedAt = DateTime.UtcNow;
                        walletTransaction.UpdatedAt = DateTime.UtcNow;
                        walletTransaction.Comments = string.Empty;
                        walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                        walletTransaction.TransactionType = "DEBIT";
                        walletTransaction.TransactionTypeInfo = 0;
                        walletTransaction.IsBankTransaction = false;
                        walletTransaction.BankBranchCode = "0";
                        walletTransaction.BankTransactionId = "0";
                        walletTransaction.VoucherCode = "0";
                        walletTransaction.FlatCharges = 0;
                        walletTransaction.BenchmarkCharges = 0;
                        walletTransaction.CommisionPercent = 0;
                        walletTransaction.DisplayContent = string.Empty;
                        walletTransaction.IsUpcomingBillShow = true;
                        walletTransaction.SubCategoryId = walletService.SubCategoryId;
                        var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                        //result.IsSuccess = true;

                    }
                    else
                    {
                        result.RstKey = 8;
                        result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                        result.IsSuccess = false;
                    }
                    // result.airtimeOperatorResponse = data;
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<PayMasta.ViewModel.AirtimeVM.BulkBillRechargeResponse> BulkBillPayment(BulkPaymentRequest request)
        {
            var result = new BulkBillRechargeResponse();
            var walletTransaction = new WalletTransaction();
            var invoice = new InvoiceNumberResponse();
            try
            {
                //var url = AppSetting.FlutterWaveTvOperator;
                // var res = await _thirdParty.getOperatorList(url);
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);



                if (userData == null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.USER_NOT_EXIST;
                    return result;
                }
                if (userData.WalletBalance == 0 && userData.WalletBalance < Convert.ToDouble(request.TotalAmount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                if (userData.WalletBalance < Convert.ToDouble(request.TotalAmount))
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                    return result;
                }
                var inv = await _commonService.GetInvoiceNumber(6);
                var req1 = new BulkRechargeRequest();
                var req2 = new List<BulkDatum>();
                req1.bulk_reference = inv.InvoiceNumber;
                req1.callback_url = "https://webhook.site/8d684ea0-0ce9-4665-895c-acc1805c536e";

                request.bulk_data.ForEach(x =>
              {
                  invoice = _commonService.GetInvoiceNumberForBulkPayment();
                  if (x.OperatorId > 0)
                  {

                      req2.Add(
                          new BulkDatum
                          {
                              amount = x.amount,
                              country = x.country,
                              customer = x.customer,
                              recurrence = x.recurrence,
                              reference = invoice.InvoiceNumber,
                              type = x.type,
                          });
                  }
                  else
                  {

                  }
              });
                req1.bulk_data = req2;
                var airtimeResult = await BulkPayment(req1);
                foreach (var item in request.bulk_data)
                {
                    var walletService = await _commonReporsitory.GetWalletServices(item.BillerCode, item.OperatorId);

                    if (userData != null && userData.WalletBalance > 0 && userData.WalletBalance >= Convert.ToDouble(request.TotalAmount))
                    {

                        if (airtimeResult != null)
                        {
                            if (airtimeResult.status == "error")
                            {
                                walletTransaction.TransactionStatus = (int)TransactionStatus.Failed;
                                result.RstKey = 4;
                                result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                                result.IsSuccess = true;
                            }
                            else if (airtimeResult.status == "success")
                            {
                                walletTransaction.TransactionStatus = (int)TransactionStatus.Completed;
                                result.RstKey = 3;
                                result.Message = ResponseMessages.TRANSACTION_SUCCESS;
                                result.IsSuccess = true;
                            }
                            else if (airtimeResult.status == "pending")
                            {
                                walletTransaction.TransactionStatus = (int)TransactionStatus.Pending;
                                result.RstKey = 5;
                                result.Message = ResponseMessages.PENDING_TRANSACTION;
                            }
                            walletTransaction.TotalAmount = item.amount.ToString();
                            walletTransaction.CommisionId = 0;
                            walletTransaction.CommisionAmount = "0";
                            walletTransaction.WalletAmount = userData.WalletBalance.ToString();
                            walletTransaction.ServiceTaxRate = 0;
                            walletTransaction.ServiceTax = "0";
                            walletTransaction.ServiceCategoryId = (int)walletService.Id;
                            walletTransaction.SenderId = userData.Id;
                            walletTransaction.ReceiverId = userData.Id;
                            walletTransaction.AccountNo = item.customer;
                            walletTransaction.TransactionId = airtimeResult.data.batch_reference == null ? invoice.InvoiceNumber : airtimeResult.data.batch_reference.ToString();
                            walletTransaction.IsAdminTransaction = false;
                            walletTransaction.IsActive = true;
                            walletTransaction.IsDeleted = false;
                            walletTransaction.CreatedAt = DateTime.UtcNow;
                            walletTransaction.UpdatedAt = DateTime.UtcNow;
                            walletTransaction.Comments = string.Empty;
                            walletTransaction.InvoiceNo = invoice.InvoiceNumber; ;
                            walletTransaction.TransactionType = "DEBIT";
                            walletTransaction.TransactionTypeInfo = 0;
                            walletTransaction.IsBankTransaction = false;
                            walletTransaction.BankBranchCode = "0";
                            walletTransaction.BankTransactionId = "0";
                            walletTransaction.VoucherCode = "0";
                            walletTransaction.FlatCharges = 0;
                            walletTransaction.BenchmarkCharges = 0;
                            walletTransaction.CommisionPercent = 0;
                            walletTransaction.DisplayContent = string.Empty;
                            walletTransaction.IsUpcomingBillShow = true;
                            walletTransaction.SubCategoryId = walletService.SubCategoryId;
                            var data = await _commonReporsitory.InsertWalletTransactions(walletTransaction);

                            //result.IsSuccess = true;

                        }
                        else
                        {
                            result.RstKey = 8;
                            result.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                            result.IsSuccess = false;
                        }
                        // result.airtimeOperatorResponse = data;
                    }
                    else
                    {
                        result.RstKey = 4;
                        result.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
