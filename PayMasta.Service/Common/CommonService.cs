using Newtonsoft.Json;
using PayMasta.DBEntity.ErrorLog;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Utilities;
using PayMasta.ViewModel.CMS;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.NotificationsVM;
using PayMasta.ViewModel.ProvidusBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using PayMasta.Utilities.EmailUtils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using PayMasta.Service.ThirdParty;
using PayMasta.ViewModel.ZealvendBillsVM;

namespace PayMasta.Service.Common
{
    public class CommonService : ICommonService
    {
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailUtils _emailUtils;
        private readonly IExpressWalletThirdParty _thirdParty;
        //singleton design pattern
        //private static CommonService commonService = null;
        //public static CommonService getin
        //{
        //    get
        //    {
        //        if(commonService == null)
        //            commonService=new CommonService();
        //        return commonService;
        //    }
        //}

        public CommonService()
        {
            _commonReporsitory = new CommonReporsitory();
            _accountRepository = new AccountRepository();
            _emailUtils = new EmailUtils();
            _thirdParty = new ExpressWalletThirdParty();
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<InvoiceNumberResponse> GetInvoiceNumber(int digit = 6)
        {
            var result = new InvoiceNumberResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var Invoice = await _commonReporsitory.GetInvoiceNumber(dbConnection);
                    if (Invoice != null)
                    {
                        result.Id = Invoice.Id;
                        result.InvoiceNumber = Invoice.InvoiceNumber;
                        int power = digit - result.Id.ToString().Length;
                        var str = Math.Pow(10, power).ToString().Replace("1", "");
                        result.AutoDigit = str + result.Id.ToString();
                        result.Guid = Invoice.Guid;
                    }
                }
            }
            catch (Exception ex)
            {
                result.InvoiceNumber = CommonSetting.GetUniqueNumber();
            }
            return result;
        }

        public InvoiceNumberResponse GetInvoiceNumberForBulkPayment(int digit = 6)
        {
            var result = new InvoiceNumberResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var Invoice = _commonReporsitory.GetInvoiceNumberForBulkPayment(dbConnection);
                    if (Invoice != null)
                    {
                        result.Id = Invoice.Id;
                        result.InvoiceNumber = Invoice.InvoiceNumber;
                        int power = digit - result.Id.ToString().Length;
                        var str = Math.Pow(10, power).ToString().Replace("1", "");
                        result.AutoDigit = str + result.Id.ToString();
                        result.Guid = Invoice.Guid;
                    }
                }
            }
            catch (Exception ex)
            {
                result.InvoiceNumber = CommonSetting.GetUniqueNumber();
            }
            return result;
        }
        public async Task<GetCategoryResponse> GetCategories()
        {
            var result = new GetCategoryResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var categoryResponses = await _commonReporsitory.GetCategories(true, dbConnection);
                    if (categoryResponses.Count > 0)
                    {
                        result.categoryResponse = categoryResponses;
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
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetNotificationsResponse> GetNotificationByUserGuid(NotificationsRequest notificationsRequest)
        {
            var result = new GetNotificationsResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var userData = await _accountRepository.GetUserByGuid(notificationsRequest.UserGuid);
                    var notifications = await _commonReporsitory.GetNotifications(userData.Id, dbConnection);
                    var notificationsCount = await _commonReporsitory.GetNotificationsCount(userData.Id, dbConnection);
                    if (notifications.Count > 0)
                    {
                        result.TotalCount = notificationsCount;
                        result.notificationsResponses = notifications;
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
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<UpdateNotificationsResponse> UpdateNotificationByUserGuid(NotificationsRequest notificationsRequest)
        {
            var result = new UpdateNotificationsResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var userData = await _accountRepository.GetUserByGuid(notificationsRequest.UserGuid);
                    var notifications = await _commonReporsitory.UpdateUserNotificationIsRead(userData.Id, dbConnection);
                    if (notifications > 0)
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
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<UpdateNotificationsResponse> InsertDailyEarningByScheduler()
        {
            var result = new UpdateNotificationsResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var data = await _commonReporsitory.GetSchedulerCurrentDate(dbConnection);

                    var currentDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
                    var lastDate = data != null ? data.UpdatedAt.ToString("MM/dd/yyyy") : DateTime.UtcNow.ToString("MM/dd/yyyy");

                    if (data == null)
                    {

                        var notifications = await _commonReporsitory.InsertDailyEarningByScheduler(dbConnection);
                        if (notifications == -1)
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
                    }
                    else if (data != null && currentDate != lastDate)
                    {
                        var notifications = await _commonReporsitory.InsertDailyEarningByScheduler(dbConnection);
                        if (notifications == -1)
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
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<FAQResponse>> GetFaq()
        {
            var faqResponses = new List<FAQResponse>();
            var itemEntity = new List<FaqDetailResponse>();
            try
            {
                //var dd = await FaqTest();
                using (var dbConnection = Connection)
                {
                    faqResponses = await _commonReporsitory.FAQ(dbConnection);
                    if (faqResponses.Count > 0)
                    {
                        if (faqResponses != null && faqResponses.Count > 0)
                        {
                            foreach (var item in faqResponses)
                            {
                                var details = await _commonReporsitory.FAQAnswers(item.Id);
                                if (details != null && details.Count > 0)
                                {
                                    item.FaqDetails = details;
                                }
                            }
                        }

                        //result.FaqDetails = faqResponses;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return faqResponses;
        }
        public async Task<CmsResponse> GetPrivacyPolicy()
        {
            var result = new CmsResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var faqResponses = await _commonReporsitory.GetPrivacyPolicy(dbConnection);
                    if (faqResponses != null)
                    {
                        result.getCms = faqResponses;
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
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<CmsResponse> GetTermAndCondition()
        {
            var result = new CmsResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var faqResponses = await _commonReporsitory.GetTandC(dbConnection);
                    if (faqResponses != null)
                    {
                        result.getCms = faqResponses;
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
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetProvidusResponse> GetProvidusBankResponse(ProvidusBankRequest request)
        {
            var result = new GetProvidusResponse();
            try
            {
                var json = JsonConvert.SerializeObject(request);
                using (var dbConnection = Connection)
                {
                    var req = new ErrorLog
                    {
                        ClassName = request.settlementId,
                        CreatedDate = DateTime.Now,
                        ErrorMessage = json,
                        JsonData = json,
                        MethodName = "GetProvidusBankResponse"
                    };
                    var data = await _commonReporsitory.GetProvidusBankResponseBySessionId(request.settlementId);
                    if (data == null)
                    {
                        var notifications = await _commonReporsitory.InsertProvidusBankResponse(req, dbConnection);
                        if (notifications > 0)
                        {

                            result.responseCode = "00";
                            result.requestSuccessful = true;
                            result.sessionId = request.sessionId;
                            result.responseMessage = "success";
                        }
                    }
                    else
                    {
                        result.responseCode = "01";
                        result.requestSuccessful = true;
                        result.sessionId = request.sessionId;
                        result.responseMessage = "duplicate transaction";
                        //duplicate transaction
                    }
                }
            }
            catch (Exception ex)
            {
                result.responseCode = "02";
                result.requestSuccessful = true;
                result.sessionId = request.sessionId;
                result.responseMessage = "rejected transaction";
            }
            return result;
        }

        public async Task<int> GetProvidusBankResponse(string request)
        {
            int result = 0;
            try
            {
                var json = JsonConvert.SerializeObject(request);
                using (var dbConnection = Connection)
                {
                    var req = new ErrorLog
                    {
                        ClassName = "Function name GetProvidusBankResponse",
                        CreatedDate = DateTime.Now,
                        ErrorMessage = json,
                        JsonData = json,
                        MethodName = "GetProvidusBankResponse"
                    };
                    var notifications = await _commonReporsitory.InsertProvidusBankResponse(req, dbConnection);
                    if (notifications > 0)
                    {
                        result = notifications;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<string> FaqTest()
        {
            string res = string.Empty;
            try
            {

                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                object miss = System.Reflection.Missing.Value;
                //string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                object path = @"D:\Projects\PayMasta\paymasta.web\PayMasta.Web\FileUpload\privacy_and_policy.docx";
                object readOnly = true;
                Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                string totaltext = "";
                for (int i = 0; i < docs.Paragraphs.Count; i++)
                {
                    totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
                }
                res = totaltext;
                docs.Close();
                word.Quit();

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<List<string>> MovieList()
        {
            List<string> movies = new List<string>();
            try
            {


                movies.Add("Movies1");
                movies.Add("Movies2");
                movies.Add("Movies3");

            }
            catch (Exception ex)
            {

            }
            return movies;
        }

        public async Task<UpdateNotificationsResponse> RequestDemo(RequestDemoRequest request)
        {
            var result = new UpdateNotificationsResponse();
            try
            {
                var fileName = AppSetting.RequestDemo;
                var body = _emailUtils.ReadEmailformats(fileName);
                body = body.Replace("$$Name$$", request.FirrstName + " " + request.LastName);
                body = body.Replace("$$Email$$", request.Email);
                body = body.Replace("$$PhoneNumber$$", request.PhoneNumber);
                body = body.Replace("$$Company$$", request.CompanyName);
                body = body.Replace("$$CompanySize$$", request.CompanySize);
                body = body.Replace("$$JobTitle$$", request.JobTitle);
                body = body.Replace("$$Description$$", request.Detail);
                var req = new EmailModel
                {
                    Body = body,
                    TO = AppSetting.SupportEmailTo.ToString(),
                    Subject = "Request a Demo"

                };

                var data = _emailUtils.SendEmailBySendGrid(req);
                result.IsSuccess = true;
                result.RstKey = 1;
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public async Task<string> WalletToWalletTransfer(long userId, double amount)
        {
            string result = string.Empty;
            try
            {
                var userData = await _accountRepository.GetUserById(userId);
                var accountData = await _accountRepository.GetVirtualAccountDetailByUserId1(userId);
                var invoiceNumber = await GetInvoiceNumber();
                var req = new WalletToWalletTransferRequest
                {
                    accountNumber = AppSetting.SourceAccountNumberPouchii,
                    amount = Convert.ToDecimal(amount),
                    channel = "wallettowallet",
                    sourceAccountNumber = accountData.AccountNumber,
                    beneficiaryName = "",
                    destBankCode = "",
                    isToBeSaved = true,
                    pin = userData.Id.ToString(),
                    sourceBankCode = "",
                    transRef = invoiceNumber.InvoiceNumber
                };
                var jsonReq = JsonConvert.SerializeObject(req);
                result = await FundTransaction(jsonReq, AppSetting.Fundwallet, accountData.AuthToken);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<string> FundTransaction(string req, string url, string token)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var content = new StringContent(req, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    // response.EnsureSuccessStatusCode();
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

        public async Task<string> YourMethod()
        {
            //Create a list of your parameters
            var postParams = new List<KeyValuePair<string, object>>(){
                new KeyValuePair<string, object>("grant_type", "authorization_code") ,
                new KeyValuePair<string, object>("client_id", "77nkft9unoq4rw"),
                  new KeyValuePair<string, object>("client_secret", "FJaI4z0pPajuTJ7q"),
                  new KeyValuePair<string, object>("code", "AQRmzBdPjL5dQWxq6YCc5aF4qZLzQ9XW03kk7cOAAUm8IM76lcYgFiMFVi3akYFY98Zvv7Q0nk-9_ne9zGdePk1Oig5kfSvYfVxZS40jabOtHLca_CNmKZDDwsgR26Ts_z49oKvuwiJJPTDPFpEoThGJxOx95IGY8g_cHhS6Ua3ULcP2eLxh-bmczWVdlu53cKn-sHlaIX7dSOGMzLA"),
                    new KeyValuePair<string, object>("redirect_uri", "http://localhost:3000")
            };

            //Join KVPs into a x-www-formurlencoded string
            var formString = string.Join("&", postParams.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
            //output: FirstParamter=FirstValue&SecondParamter=SecondValue

            //Encode form string to bytes
            var bytes = Encoding.UTF8.GetBytes(formString);

            //Create a POST webrequest
            var request = WebRequest.CreateHttp("https://www.linkedin.com/oauth/v2/accessToken");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //Open request stream
            using (var reqStream = await request.GetRequestStreamAsync())
            {
                //Write bytes to the request stream
                await reqStream.WriteAsync(bytes, 0, bytes.Length);
            }

            //Try to get response
            WebResponse response = null;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                //Something went wrong with our request. Return the exception message.
                return e.Message;
            }

            //Get response stream
            using (var respStream = response.GetResponseStream())
            {
                //Create a streamreader to read the website's response, then return it as a string
                using (var reader = new StreamReader(respStream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<string> GetZealvendAccessToken()
        {
            var result = string.Empty;

            try
            {
                var tokenRequest = new ZealvendTokenRequest
                {
                    Email = AppSetting.ZealEmail,
                    Password = AppSetting.ZealPassword,
                };
                var req = JsonConvert.SerializeObject(tokenRequest);
                var res = await _thirdParty.PostDataZealvend(req, AppSetting.ZealVendAuthTokenEndpoint);
                if (res.StatusCode == 200)
                {
                    var JsonResult = JsonConvert.DeserializeObject<ZealvendTokenResponse>(res.JsonString);
                    if (JsonResult != null && JsonResult.Status == "ok")
                    {
                        result = JsonResult.Token;
                    }
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
