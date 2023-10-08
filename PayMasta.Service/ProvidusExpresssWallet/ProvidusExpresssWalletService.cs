using PayMasta.Repository.Account;
using PayMasta.Repository.VirtualAccountRepository;
using PayMasta.Service.FlutterWave;
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
using PayMasta.DBEntity.VirtualAccountDetail;
using PayMasta.ViewModel.VirtualAccountVM;
using System.Device.Location;
using PayMasta.Repository.ExpressWalletRepository;
using PayMasta.ViewModel.ProvidusExpresssWalletVM;
using PayMasta.DBEntity.BankDetail;
using PayMasta.DBEntity.ExpressWallet;
using CSharpFunctionalExtensions;
using PayMasta.ViewModel.Common;
using PayMasta.DBEntity.WalletTransaction;
using PayMasta.ViewModel.Enums;
using PayMasta.Service.Common;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.WalletToBankVM;

namespace PayMasta.Service.ProvidusExpresssWallet
{
    public class ProvidusExpresssWalletService : IProvidusExpresssWalletService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IExpressWalletThirdParty _expressWalletThirdParty;
        private readonly IExpressWalletRepository _expressWalletRepository;
        private readonly ICommonService _commonService;
        public ProvidusExpresssWalletService()
        {
            _expressWalletThirdParty = new ExpressWalletThirdParty();
            _expressWalletRepository = new ExpressWalletRepository();
            _accountRepository = new AccountRepository();
            _commonService = new CommonService();
            // _virtualAccountRepository = new VirtualAccountRepository();
            // _thirdParty = new ThirdPartyService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<bool> CreateVirtualAccount(Guid userGuid)
        {
            var bankdetail = new BankDetail();
            string url = AppSetting.ExpressWalletBaseUrl + AppSetting.CreateExpressWallet;
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            var virtualAccountDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
            if (userData != null)
            {
                bankdetail = await _accountRepository.GetBankDetailByUserId(userData.Id);
            }
            bool res = false;
            try
            {

                if (virtualAccountDetail == null && bankdetail != null && userData.IsvertualAccountCreated == false)
                {
                    var reqBody = new WalletCreateRequest
                    {
                        bvn = bankdetail.BVN,
                        address = userData.Address,
                        dateOfBirth = userData.DateOfBirth.ToString("yyyy-MM-dd"),
                        email = userData.Email,
                        firstName = userData.FirstName,
                        lastName = userData.LastName,
                        phoneNumber = userData.PhoneNumber,
                        metadata = new MetadataCreate
                        {
                            additionaldata = userData.Id.ToString(),
                            evenmore = userData.Guid.ToString(),
                        }
                    };
                    var jsonReq = JsonConvert.SerializeObject(reqBody);
                    var resData = await _expressWalletThirdParty.PostData(jsonReq, url);
                    //var resData = "{\"status\":true,\"wallet\":{\"id\":\"852bbce2-688b-4fa3-b791-4e87bb7ab390\",\"mode\":\"SANDBOX\",\"email\":\"rajdeep1@gmail.com\",\"status\":\"ACTIVE\",\"bankName\":\"XpressWallet\",\"bankCode\":\"999270\",\"walletId\":\"b1f2e3b6-dcbf-4385-9e61-8f2f65d30d3a\",\"walletType\":\"Customers\",\"accountName\":\"LastMile-Mathewwade\",\"accountNumber\":\"4414919805\",\"bookedBalance\":0,\"availableBalance\":0,\"accountReference\":\"ftoOIOFwsT5TVlVKrN4jKx1F2p6GfMnSEGka\"},\"customer\":{\"id\":\"b1f2e3b6-dcbf-4385-9e61-8f2f65d30d3a\",\"metadata\":{\"even-more\":\"Otherdata\",\"additional-data\":\"somemoredata\"},\"bvn\":\"22181029362\",\"dateOfBirth\":\"1992-05-16\",\"phoneNumber\":\"2348020245366\",\"currency\":\"NGN\",\"email\":\"rajdeep1@gmail.com\",\"lastName\":\"wade\",\"firstName\":\"Mathew\",\"address\":\"No10,AdewaleAjasinUniversity\",\"BVNLastName\":\"wade\",\"BVNFirstName\":\"Mathew\",\"nameMatch\":true,\"mode\":\"SANDBOX\",\"MerchantId\":\"d6361acb-13f6-403f-bc1e-fedda1001e28\",\"tier\":\"TIER_3\",\"updatedAt\":\"2023-01-11T15:59:23.399Z\",\"createdAt\":\"2023-01-11T15:59:23.399Z\"}}";// await _expressWalletThirdParty.PostData(jsonReq, url);
                    var walletData = JsonConvert.DeserializeObject<WalletCreateResponse>(resData);
                    if (walletData != null && walletData.status == true && resData != null && walletData != null)
                    {
                        var saveData = new ExpressVirtualAccountDetail
                        {
                            AccountName = walletData.wallet.accountName,
                            AccountNumber = walletData.wallet.accountNumber,
                            AccountReference = walletData.wallet.accountReference,
                            Address = walletData.customer.address,
                            AuthJson = resData,
                            BankCode = walletData.wallet.bankCode,
                            BankName = walletData.wallet.bankName,
                            Bvn = walletData.customer.bvn,
                            CreatedAt = DateTime.UtcNow,
                            CurrentBalance = walletData.wallet.availableBalance.ToString(),
                            CustomerId = walletData.customer.id,
                            DateOfBirth = walletData.customer.dateOfBirth,
                            Email = walletData.customer.email,
                            IsActive = true,
                            IsDeleted = false,
                            MerchantId = walletData.customer.MerchantId,
                            NameMatch = walletData.customer.nameMatch,
                            PhoneNumber = walletData.customer.phoneNumber,
                            UserId = userData.Id,
                            VirtualAccountId = walletData.wallet.id,
                            WalletId = walletData.wallet.id,
                            WalletType = walletData.wallet.walletType,
                            JsonData = resData

                        };
                        if (await _expressWalletRepository.InsertVirtualAccountDetail(saveData) > 0)
                        {
                            res = true;
                        }
                    }
                }
                else if (virtualAccountDetail != null)
                {

                    res = true;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<CustomerWalletDetailResponse> GetVirtualAccount(Guid userGuid)
        {
            var res = new CustomerWalletDetailResponse();
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            var virtualAccountDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
            if (virtualAccountDetail != null)
            {
                string url = AppSetting.ExpressWalletBaseUrl + AppSetting.GetExpressWallet + virtualAccountDetail.CustomerId;
                if (virtualAccountDetail != null)
                {

                    var resData = await _expressWalletThirdParty.GetDate(url);
                    res = JsonConvert.DeserializeObject<CustomerWalletDetailResponse>(resData);

                }
            }

            return res;
        }

        public async Task<GetExpressWalletToBankResponse> WalletToWalletTransfer(WalletToWalletRequest request)
        {
            var result = new GetExpressWalletToBankResponse();
            var reqTran = new WalletTransaction();
            var reqTranCredit = new WalletTransaction();
            string url = AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressWalletToWallet;
            var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
            var virtualAccountDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
            var receiverDetails = await _expressWalletRepository.GetVirtualAccountDetailByWalletAccount(request.WalletAccountNumber);
            var walletServiceData = await _expressWalletRepository.GetWalletServices(request.SubCategoryId, request.Service);
            bool res = false;
            try
            {
                if (virtualAccountDetail == null)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.SENDER_NOT_EXIST;
                    return result;
                }
                if (receiverDetails != null && receiverDetails.UserId == userData.Id)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.SELF_WALLET;
                    return result;
                }
                if (receiverDetails == null)
                {
                    result.IsSuccess = false;
                    result.RstKey = 6;
                    result.Message = ResponseMessages.RECEIVER_NOT_EXIST;
                    return result;
                }

                if (virtualAccountDetail != null && walletServiceData != null)
                {
                    var reqBody = new ProvidusWalletToWalletRequest
                    {
                        amount = request.Amount,
                        fromCustomerId = virtualAccountDetail.CustomerId,
                        toCustomerId = receiverDetails.CustomerId
                    };
                    var jsonReq = JsonConvert.SerializeObject(reqBody);
                    var resData = await _expressWalletThirdParty.PostData(jsonReq, url);
                    //var resData = "{\"status\":true,\"wallet\":{\"id\":\"852bbce2-688b-4fa3-b791-4e87bb7ab390\",\"mode\":\"SANDBOX\",\"email\":\"rajdeep1@gmail.com\",\"status\":\"ACTIVE\",\"bankName\":\"XpressWallet\",\"bankCode\":\"999270\",\"walletId\":\"b1f2e3b6-dcbf-4385-9e61-8f2f65d30d3a\",\"walletType\":\"Customers\",\"accountName\":\"LastMile-Mathewwade\",\"accountNumber\":\"4414919805\",\"bookedBalance\":0,\"availableBalance\":0,\"accountReference\":\"ftoOIOFwsT5TVlVKrN4jKx1F2p6GfMnSEGka\"},\"customer\":{\"id\":\"b1f2e3b6-dcbf-4385-9e61-8f2f65d30d3a\",\"metadata\":{\"even-more\":\"Otherdata\",\"additional-data\":\"somemoredata\"},\"bvn\":\"22181029362\",\"dateOfBirth\":\"1992-05-16\",\"phoneNumber\":\"2348020245366\",\"currency\":\"NGN\",\"email\":\"rajdeep1@gmail.com\",\"lastName\":\"wade\",\"firstName\":\"Mathew\",\"address\":\"No10,AdewaleAjasinUniversity\",\"BVNLastName\":\"wade\",\"BVNFirstName\":\"Mathew\",\"nameMatch\":true,\"mode\":\"SANDBOX\",\"MerchantId\":\"d6361acb-13f6-403f-bc1e-fedda1001e28\",\"tier\":\"TIER_3\",\"updatedAt\":\"2023-01-11T15:59:23.399Z\",\"createdAt\":\"2023-01-11T15:59:23.399Z\"}}";// await _expressWalletThirdParty.PostData(jsonReq, url);
                    var walletData = JsonConvert.DeserializeObject<ProvidusWalletToWalletResponse>(resData);
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    if (walletData != null && walletData.status == true)
                    {

                        #region sender 
                        reqTran.AccountNo = request.WalletAccountNumber;
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
                        reqTran.TotalAmount = request.Amount.ToString();
                        reqTran.TransactionId = invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                        #endregion sender
                        #region receiver 
                        reqTranCredit.AccountNo = request.WalletAccountNumber;
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
                        reqTranCredit.ReceiverId = receiverDetails.UserId;
                        reqTranCredit.SenderId = receiverDetails.UserId; ;
                        reqTranCredit.ServiceCategoryId = (int)walletServiceData.Id;
                        reqTranCredit.ServiceTax = string.Empty;
                        reqTranCredit.ServiceTaxRate = 0;
                        reqTranCredit.SubCategoryId = walletServiceData.SubCategoryId;
                        reqTranCredit.TotalAmount = request.Amount.ToString();
                        reqTranCredit.TransactionId = invoiceNumber.InvoiceNumber;
                        reqTranCredit.TransactionType = "CREDIT";
                        reqTranCredit.TransactionTypeInfo = 0;
                        reqTranCredit.UpdatedAt = DateTime.UtcNow;
                        reqTranCredit.UpdatedBy = userData.Id;
                        reqTranCredit.VoucherCode = "";
                        reqTranCredit.WalletAmount = string.Empty;
                        await _expressWalletRepository.InsertTransactions(reqTran);
                        await _expressWalletRepository.InsertTransactions(reqTranCredit);
                        #endregion receiver
                    }
                    else if (walletData != null && walletData.status == false)
                    {
                        result.IsSuccess = false;
                        result.RstKey = 6;
                        result.Message = walletData.message;
                        return result;
                    }
                    else
                    {
                        reqTran.AccountNo = request.WalletAccountNumber;
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
                        reqTran.TotalAmount = request.ToString();
                        reqTran.TransactionId = invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                        await _expressWalletRepository.InsertTransactions(reqTran);
                    }
                    if (walletData.status == true)
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.vTUResponse = walletData;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.vTUResponse = walletData;
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

            }
            return result;
        }

        public async Task<GetExpressWalletToBankRes> WalletToBankTransfer(WalletToBankPaymentRequest request)
        {
            var result = new GetExpressWalletToBankRes();
            var reqTran = new WalletTransaction();
            var responseTran = new WalletToWalletTransferResponse();
            string customer = string.Empty;
            double comissionAmt = 0;
            double comissionAmtWithOutVat = 0;
            //st.Append("ss");
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _expressWalletRepository.GetWalletServices(request.SubCategoryId, request.Service);
                string url = AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressWalletToBankTransfer;

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
                if (requestedAmt > 0)
                {
                    var verifyData = await VerifyAccount(request.destBankCode, request.AccountNumber);
                    var invoiceNumber = await _commonService.GetInvoiceNumber();
                    var walletToBank = new ExpressWalletToBankRequest
                    {
                        accountName = verifyData.account.accountName,
                        accountNumber = request.AccountNumber,
                        amount = Convert.ToDecimal(request.Amount),
                        customerId = tokenDetail.CustomerId,
                        sortCode = request.destBankCode,
                        // narration = "Just kidding",
                        narration = invoiceNumber.InvoiceNumber,
                        metadata = new WalletToBankMetadata
                        {
                            customerdata = userData.Guid.ToString()
                        }
                    };
                    var req = JsonConvert.SerializeObject(walletToBank);
                    var res = await _expressWalletThirdParty.PostData(req, url);
                    var JsonResult = JsonConvert.DeserializeObject<ExpressWalletToBankResponse>(res);
                    //if (JsonResult.status == false)
                    //{
                    //    result.IsSuccess = false;
                    //    result.RstKey = 2;
                    //    result.expressWalletToBankResponse = JsonResult;
                    //    return result;
                    //}
                    //else
                    //{
                    //    // var JsonResult1 = JsonConvert.DeserializeObject<WalletToBankResponse>(res);

                    //}

                    if (JsonResult != null && JsonResult.status == true)
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
                        reqTran.TransactionId = JsonResult.transfer.transactionReference;
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
                        reqTran.TransactionId = JsonResult != null && JsonResult.transfer != null ? JsonResult.transfer.transactionReference : invoiceNumber.InvoiceNumber;
                        reqTran.TransactionType = "DEBIT";
                        reqTran.TransactionTypeInfo = 0;
                        reqTran.UpdatedAt = DateTime.UtcNow;
                        reqTran.UpdatedBy = userData.Id;
                        reqTran.VoucherCode = "";
                        reqTran.WalletAmount = string.Empty;
                    }
                    var saveRes = await _expressWalletRepository.InsertTransactions(reqTran);
                    if (JsonResult.status == true && saveRes > 0)
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.expressWalletToBankResponse = JsonResult;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;
                        result.expressWalletToBankResponse = JsonResult;
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

        public async Task<GetExpressBankListResponse> GetExpressBankList(Guid userGuid)
        {
            var res = new GetExpressBankListResponse();
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            var virtualAccountDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
            if (virtualAccountDetail != null)
            {
                string url = AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressBankList;
                if (virtualAccountDetail != null)
                {

                    var resData = await _expressWalletThirdParty.GetDate(url);
                    var bankRes = JsonConvert.DeserializeObject<ExpressBankList>(resData);
                    if (bankRes != null && bankRes.status == true)
                    {
                        res.RstKey = 1;
                        res.IsSuccess = true;
                        res.expressBankList = bankRes;
                    }

                }
            }

            return res;
        }

        public async Task<VerifyExpressBankAccount> VerifyAccount(string bankCode, string accountNumber)
        {
            var res = new VerifyExpressBankAccount();

            if (accountNumber != null)
            {
                string url = AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressWalletToBankVerify + "sortCode=" + bankCode + "&accountNumber=" + accountNumber;

                var resData = await _expressWalletThirdParty.GetDate(url);
                var bankRes = JsonConvert.DeserializeObject<VerifyExpressBankAccount>(resData);
                if (bankRes.status == true)
                {
                    res = bankRes;
                }
            }

            return res;
        }
        public async Task<bool> DebitCustomerWalletForBills(Guid userGuid, string amount, string invoiveNumbe)
        {
            bool res = false;
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            var virtualAccountDetail = await _expressWalletRepository.GetVirtualAccountDetailByUserId(userData.Id);
            if (virtualAccountDetail != null && !string.IsNullOrWhiteSpace(virtualAccountDetail.CustomerId) && Convert.ToDecimal(amount) > 0)
            {
                string url = AppSetting.ExpressWalletBaseUrl + AppSetting.ExpressWalletDebit;

                var req = new DebitCustomerWalletRequest
                {
                    amount = Convert.ToDecimal(amount),
                    customerId = virtualAccountDetail.CustomerId,
                    reference = invoiveNumbe,
                    metadata = new DebitWalletMetadata
                    {
                        moredata = userData.Email,
                        somedata = userData.Id.ToString()
                    }
                };
                var jsonReq = JsonConvert.SerializeObject(req);
                var resData = await _expressWalletThirdParty.PostData(jsonReq, url);
                var bankRes = JsonConvert.DeserializeObject<DebitCustomerWalletResponse>(resData);
                if (bankRes != null && bankRes.status == true)
                {
                    res = true;
                }
            }

            return res;
        }

    }
}
