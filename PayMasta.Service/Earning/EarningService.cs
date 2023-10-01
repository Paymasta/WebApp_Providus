using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using NLog;
using PayMasta.DBEntity.AccessAmountRequest;
using PayMasta.Repository.Account;
using PayMasta.Repository.Earning;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.EarningVM;
using PayMasta.ViewModel.Employer.Dashboard;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PayMasta.Service.Earning
{
    public class EarningService : IEarningService
    {
        private readonly IEarningRepository _earningRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailUtils _emailUtils;
        private static Logger Logger = LogManager.GetLogger("Info");
        public EarningService()
        {
            _earningRepository = new EarningRepository();
            _accountRepository = new AccountRepository();
            _emailUtils = new EmailUtils();
        }

        //public async Task<EarningResponse> GetEarnings(EarningRequest request)
        //{
        //    var result = new EarningResponse();
        //    var user = await _accountRepository.GetUserByGuid(request.UserGuid);
        //    var earning = await _earningRepository.GetEarnings(user.Id);
        //    if (user != null && earning != null)
        //    {
        //        result.EarnedAmount = earning != null ? earning.EarnedAmount : 00;
        //        result.AccessedAmount = earning != null ? Convert.ToDecimal(earning.AccessedAmount) : 00;
        //        result.AvailableAmount = earning != null ? Convert.ToDecimal(earning.AvailableAmount) : 00;
        //        result.PayCycleFrom = earning != null ? earning.PayCycleFrom : "02 Jan";
        //        result.PayCycleTo = earning != null ? earning.PayCycleTo : "31 Jan";
        //        result.UserId = earning.UserId;
        //        result.RstKey = 1;
        //    }
        //    else if (user != null && user.EmployerId > 0 && user.IsEmployerRegister == true && earning == null)
        //    {
        //        var payDate = await _earningRepository.GetPayCycleDate(user.Id);
        //        result.EarnedAmount = 00;
        //        result.AccessedAmount = 00;
        //        result.AvailableAmount = 00;
        //        result.PayCycleFrom = payDate != null ? payDate.PayCycleFrom : "N/A";
        //        result.PayCycleTo = payDate != null ? payDate.PayCycleTo : "N/A";
        //        result.UserId = user.Id;
        //        result.RstKey = 1;
        //    }
        //    else
        //    {
        //        result.EarnedAmount = 00;
        //        result.AccessedAmount = 00;
        //        result.AvailableAmount = 00;
        //        result.PayCycleFrom = "N/A";
        //        result.PayCycleTo = "N/A";
        //        result.UserId = user.Id;
        //        result.RstKey = 1;
        //    }
        //    return result;
        //}
        public async Task<EarningResponseForWeb> GetEarnings(EarningRequest request)
        {
            var result = new EarningResponseForWeb();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            try
            {
                var earning = await _earningRepository.GetEarnings(user.Id);
                if (user != null && earning != null)
                {
                    result.EarnedAmount = earning != null ? earning.EarnedAmount.ToString("N") : "0.00";
                    result.AccessedAmount = earning != null ? earning.AccessedAmount.ToString("N") : "0.00";
                    result.AvailableAmount = earning != null ? earning.AvailableAmount.ToString("N") : "0.00";
                    result.PayCycleFrom = earning != null ? earning.PayCycleFrom : "02 Jan";
                    result.PayCycleTo = earning != null ? earning.PayCycleTo : "31 Jan";
                    result.UserId = earning.UserId;
                    result.RstKey = 1;
                }
                else if (user != null && user.EmployerId > 0 && user.IsEmployerRegister == true && earning == null)
                {
                    var payDate = await _earningRepository.GetPayCycleDate(user.Id);
                    result.EarnedAmount = "0.00";
                    result.AccessedAmount = "0.00";
                    result.AvailableAmount = "0.00";
                    result.PayCycleFrom = payDate != null ? payDate.PayCycleFrom : "N/A";
                    result.PayCycleTo = payDate != null ? payDate.PayCycleTo : "N/A";
                    result.UserId = user.Id;
                    result.RstKey = 1;
                }
                else
                {
                    result.EarnedAmount = "0.00";
                    result.AccessedAmount = "0.00";
                    result.AvailableAmount = "0.00";
                    result.PayCycleFrom = "N/A";
                    result.PayCycleTo = "N/A";
                    result.UserId = user.Id;
                    result.RstKey = 1;
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        public async Task<TransactionHistoryResponse> GetTransactionHistory(GetTransactionHistoryRequest request)
        {
            var result = new TransactionHistoryResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var historyResponses = await _earningRepository.GetTransactionHistory(user.Id, request.ServiceCategoryId, request.Month, request.PageNumber, request.PageSize);
            if (historyResponses.Count > 0)
            {
                result.getTransactionHistoryResponse = historyResponses;
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATA_RECEIVED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<AccessAmountResponse> AccessAmountRequest(AccessdAmountRequest request)
        {
            Logger.Info("<=============AccessAmountRequest service calling START===REQUEST=" + JsonConvert.SerializeObject(request));
            var result = new AccessAmountResponse();
            decimal usedLimit = 0;
            decimal commissionWithAmount = 0;
            decimal EwaAmount = 0;
            decimal commissionAmount = 0;
            int days = 0;
            //decimal totalLimit = 33.33;
            try
            {
                var user = await _accountRepository.GetUserByGuid(request.UserGuid);
                Logger.Info("<=============Getting User Details===RESPONSE=" + JsonConvert.SerializeObject(user));
                if (user.IsverifiedByEmployer == false)
                {
                    Logger.Info("<=============This user employer not verified===RESPONSE=" + user.IsverifiedByEmployer);
                    result.RstKey = 10;
                    result.IsSuccess = false;
                    result.Message = "Your profile is not approved by employer please contact paymasta admin.";
                    return result;
                }

                if (user != null && user.EmployerId > 0 && user.IsEmployerRegister == true)
                {

                    var usedAccessPercentage = await _earningRepository.GetAccessAmountPercentage(user.Id);
                    var employerDetail = await _earningRepository.GetEmployerDetailByUserId(user.Id);
                    var isOldEwaRequestPending = await _earningRepository.IsOldEwaRequestPending(user.Id);
                    var commisions = await _earningRepository.GetCommisions();
                    days = employerDetail.EndDate - 3;
                    int d = (int)System.DateTime.Now.Day;

                    if (usedAccessPercentage != null && usedAccessPercentage.AccessedPercentage > 0)
                    {
                        usedLimit = usedAccessPercentage.AccessedPercentage;
                    }
                    if (employerDetail.Status == 0)
                    {
                        Logger.Info("<=============Your employer account is not active so you can not request for EWA===RESPONSE=" + employerDetail.Status);
                        result.RstKey = 9;
                        result.IsSuccess = false;
                        result.Message = "Your employer account is not active so you can not request for EWA.";
                        return result;
                    }
                    if (d > days)
                    {
                        Logger.Info("<=============You can not request for EWA till next pay cycle start===RESPONSE=" + employerDetail.Status);
                        result.RstKey = 9;
                        result.IsSuccess = false;
                        result.Message = "You can not request for EWA till next pay cycle start.";
                        return result;
                    }
                    request.Amount = Convert.ToDecimal(request.Amount);
                    var earning = await _earningRepository.GetEarnings(user.Id);
                    decimal val = Convert.ToDecimal(33.3);
                    if (user != null)
                    {
                        if (request.Amount > 0)
                        {
                            if (Convert.ToDecimal(earning.AvailableAmount) >= Convert.ToDecimal(request.Amount))
                            {

                                if (isOldEwaRequestPending == null || (isOldEwaRequestPending.AdminStatus != (int)TransactionStatus.NoResponse && isOldEwaRequestPending.AdminStatus != (int)TransactionStatus.Pending))
                                {
                                    if (earning != null && earning.AvailableAmount > 0)
                                    {
                                        commisions.ForEach(e =>
                                        {
                                            if (e.AmountFrom < request.Amount && e.AmountTo >= request.Amount)
                                            {
                                                EwaAmount = request.Amount - e.CommisionPercent;
                                                //  commissionWithAmount = request.Amount + e.CommisionPercent;
                                                commissionAmount = e.CommisionPercent;
                                            }
                                        });

                                        var useableAmount = earning.AvailableAmount;// * val / 100;
                                                                                    //var useablelAmountPercent = (request.Amount / earning.AvailableAmount * 100);
                                        decimal limit = Convert.ToDecimal(33.3);
                                        if (earning.AvailableAmount > 0)
                                        {
                                            if (Convert.ToDecimal(useableAmount) >= Convert.ToDecimal(request.Amount))
                                            {
                                                var req = new AccessAmountRequest
                                                {
                                                    AccessAmount = EwaAmount,
                                                    AccessedPercentage = 0,
                                                    AvailableAmount = earning.AvailableAmount,
                                                    IsActive = true,
                                                    IsDeleted = false,
                                                    CreatedAt = DateTime.UtcNow,
                                                    CreatedBy = user.Id,
                                                    UserId = user.Id,
                                                    Status = (int)TransactionStatus.NoResponse,
                                                    AdminStatus = (int)TransactionStatus.NoResponse,
                                                    IsPaidToPayMasta = false,
                                                    CommissionCharge = commissionAmount,
                                                    TotalAmountWithCommission = request.Amount
                                                };
                                                Logger.Info("<=============InsertAccessAmountRequest===RESPONSE=" + JsonConvert.SerializeObject(req));
                                                if (await _earningRepository.InsertAccessAmountRequest(req) > 0)
                                                {
                                                    EmailUtils email = new EmailUtils();

                                                    string filename = AppSetting.EWAAlert;
                                                    var body = email.ReadEmailformats(filename);
                                                    body = body.Replace("$$employee$$", user.FirstName + " " + user.LastName);
                                                    body = body.Replace("$$employer$$", user.EmployerName);
                                                    body = body.Replace("$$Email$$", user.Email);
                                                    body = body.Replace("$$Phone$$", user.CountryCode + "-" + user.PhoneNumber);
                                                    body = body.Replace("$$Amount$$", request.Amount.ToString());

                                                    var reqEmail = new EmailModel
                                                    {
                                                        Subject = "EWA Approval Alert",
                                                        Body = body,
                                                        CC = AppSetting.EmailForEWaAlertDomino + "," + AppSetting.EmailForEWaAlertGerald, //+ ",gun11@yopmail.com",//This email for testing
                                                        TO = AppSetting.SupportEmailTo
                                                    };
                                                    await _emailUtils.SendEmailBySendGridForEWAAlert(reqEmail);
                                                    result.RstKey = 1;
                                                    result.IsSuccess = true;
                                                    result.Message = ResponseMessages.REQUEST_SENTTO_ADMIN;

                                                    Logger.Info("<=============InsertAccessAmountRequest SUCCESS ===RESPONSE=" + ResponseMessages.REQUEST_SENTTO_ADMIN);
                                                }
                                            }
                                            else
                                            {
                                                result.RstKey = 2;
                                                result.IsSuccess = false;
                                                result.Message = ResponseMessages.ACCESS_AMOUTN_ERROR;
                                            }
                                        }
                                        else
                                        {
                                            result.RstKey = 8;
                                            result.IsSuccess = false;
                                            result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                                        }
                                    }
                                    else
                                    {
                                        result.RstKey = 4;
                                        result.IsSuccess = false;
                                        result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                                    }
                                }
                                else
                                {
                                    result.RstKey = 5;
                                    result.IsSuccess = false;
                                    result.Message = ResponseMessages.EWA_REQUEST_PENDING;
                                }

                            }
                            else
                            {
                                result.RstKey = 31;
                                result.IsSuccess = false;
                                result.Message = ResponseMessages.INSUFICIENT_BALANCE;
                            }
                        }
                        else
                        {
                            result.RstKey = 8;
                            result.IsSuccess = false;
                            result.Message = "Amount can not be zero.";
                        }
                    }
                    else
                    {
                        result.RstKey = 3;
                    }
                }
                else
                {
                    result.RstKey = 9;
                    result.IsSuccess = false;
                    result.Message = "You cannot access the ewa because your employer is not registered with us.";
                    return result;
                }
            }
            catch(Exception ex)
            {
                Logger.Info("<=============Error Exception=" + ex.StackTrace);
            }
            Logger.Info("<=============AccessAmountRequest service calling END===REQUEST=" + JsonConvert.SerializeObject(result));
            return result;
        }

        public async Task<TodayTransactionHistoryResponse> GetTodaysTransactionHistory(GetTodayTransactionHistoryRequest request)
        {
            var result = new TodayTransactionHistoryResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var historyResponses = await _earningRepository.GetTodaysTransactionHistory(user.Id);
            if (historyResponses.Count > 0)
            {
                result.getTodayTransactionHistoryResponses = historyResponses;
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATA_RECEIVED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<UpComingBillsResponse> GetUpcomingBillsHistory(UpComingBillsRequest request)
        {
            var result = new UpComingBillsResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var historyResponses = await _earningRepository.GetUpcomingBillsHistory(user.Id);
            if (historyResponses.Count > 0)
            {
                foreach (var response in historyResponses)
                {
                    result.PayableAmount += Convert.ToDecimal(response.TotalAmount);
                }
                result.upComingBills = historyResponses;
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATA_RECEIVED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<RemoveUpComingBillsResponse> RemoveBillfromUpcomingBilsList(RemoveUpComingBillsRequest request)
        {
            var result = new RemoveUpComingBillsResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var transactionData = await _earningRepository.GetTransactionsByTransactionId(user.Id, request.WalletTransactionId);
            if (request.IsRemove == true)
            {
                transactionData.IsUpcomingBillShow = false;
            }

            transactionData.UpdatedAt = DateTime.UtcNow;
            if (await _earningRepository.UpdateTransactionStatusForUpcoingBills(transactionData) > 0)
            {

                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATAUPDATED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.REQUESTDATA_NOT_EXIST;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<GetDashboardGraph> GetDashboardGraphData(GetUserDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployeeGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                var dashboardData = await _earningRepository.GetDashboardGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    res.getDashboardGraphResponse = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> Invite(string email, Guid EmployeeGuid)
        {
            var res = new GetDashboardGraph();

            try
            {
                // var id = Guid.Parse(request.EmployeeGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(EmployeeGuid);
                var req = new EmailModel
                {
                    Subject = "Invite",
                    TO = email,
                    Body = "paymasta.co",

                };
                if (await _emailUtils.SendEmailBySendGrid(req))
                {

                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.SUCCESS;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardMonthlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                var dashboardData = await _earningRepository.GetDashboardMonthlyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardWeeklyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                var dashboardData = await _earningRepository.GetDashboardWeeklyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<GetDashboardGraph> GetDashboardYearlyGraphData(GetDashboardGraphRequest request)
        {
            var res = new GetDashboardGraph();

            try
            {
                var id = Guid.Parse(request.EmployerGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                var dashboardData = await _earningRepository.GetDashboardYearlyGraphData(result.Id, request.LastWeak);
                if (dashboardData != null)
                {
                    // res.getDashboardGraphResponse = dashboardData;
                    res.getDashboardMonthlyGraphResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<TransactionResponse> GetTransactionByWalletTransactionId(RemoveUpComingBillsRequest request)
        {
            var result = new TransactionResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            var accountData = await _accountRepository.GetVirtualAccountDetailByUserId(user.Id);
            var historyResponses = await _earningRepository.GetTransactionsByTransactionIdAndUserId(user.Id, request.WalletTransactionId);
            if (historyResponses != null)
            {

                result.getTransactionResponse = historyResponses;
                result.DebitFrom = accountData;
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATA_RECEIVED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<GetCommissionResponse> GetCommisions()
        {
            var result = new GetCommissionResponse();

            var commissionList = await _earningRepository.GetCommisionsList();
            if (commissionList.Count > 0)
            {
                result.getCommissions = commissionList;
                result.RstKey = 1;
                result.IsSuccess = true;
                result.Message = ResponseMessages.DATA_RECEIVED;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = ResponseMessages.DATA_NOT_RECEIVED;
                result.RstKey = 2;
            }
            return result;
        }

        public async Task<GetAddedBanListResponse> GetAddedBankList(Guid userGuid)
        {
            var res = new GetAddedBanListResponse();

            try
            {
                //  var id = Guid.Parse(request.EmployeeGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(userGuid);
                var dashboardData = await _earningRepository.GetAddedBankList(result.Id);
                if (dashboardData != null)
                {
                    res.addedBanListResponses = dashboardData;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
