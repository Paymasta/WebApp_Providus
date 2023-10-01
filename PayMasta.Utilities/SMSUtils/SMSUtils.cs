using PayMasta.Utilities.Common;
using PayMasta.Utilities.Extention;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PayMasta.Utilities.SMSUtils
{
    public class SMSUtils : ISMSUtils
    {
        private string path = AppSetting.SMS_Path;

        //public async Task<bool> SendSms(SMSModel model)
        //{
        //    try
        //    {
        //        //var request = new
        //        //{
        //        //    username = AppSetting.SMS_UserName,
        //        //    password = AppSetting.SMS_Password,
        //        //    msg = model.Message,
        //        //    texttype = AppSetting.SMS_TextType,
        //        //    numbers = model.CountryCode.Replace("+", string.Empty) + model.PhoneNumber,
        //        //    sender = AppSetting.SMS_Sender
        //        //};

        //        //path = path +"?"+
        //        //    "username=" + request.username +
        //        //    "&password=" + request.password +
        //        //    "&msg=" + request.msg +
        //        //    "&texttype=" + request.texttype +
        //        //    "&numbers=" + request.numbers +
        //        //    "&sender=" + request.sender;

        //        var request = new
        //        {
        //            application = AppSetting.SMS_application,
        //            password = AppSetting.SMS_Password,
        //            content = model.Message,
        //            destination = model.CountryCode.Replace("+", string.Empty) + model.PhoneNumber,
        //            source = AppSetting.SMS_source,
        //            mask = AppSetting.SMS_mask
        //        };

        //        path = path + "?" +
        //            "application=" + request.application +
        //            "&password=" + request.password +
        //            "&content=" + request.content +
        //            "&destination=" + request.destination +
        //            "&source=" + request.source +
        //            "&mask=" + request.mask;

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(path);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(
        //                new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = await client.GetAsync(new Uri(path));
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = await response.Content.ReadAsStringAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return true;
        //}

        //public async Task<bool> SendSms(SMSModel model)
        //{
        //    // Find your Account SID and Auth Token at twilio.com/console
        //    // and set the environment variables. See http://twil.io/secure
        //    //string accountSid = Environment.GetEnvironmentVariable("ACa815467594285aa18c946ad8734f260d");
        //    //string authToken = Environment.GetEnvironmentVariable("35b1c210528bcb48a9455af111862a34");
        //    try
        //    {
        //        // string accountSid = AppSetting.AccountSidTwilio;//"AC0128ecdbd5047ca493eb374b334951fd";
        //        // string authToken = AppSetting.AuthTokenTwilio;//"9ab95d1de70d4abf71f5326fe81fa9ef";

        //        string accountSid = "AC0128ecdbd5047ca493eb374b334951fd";
        //        string authToken = "9ab95d1de70d4abf71f5326fe81fa9ef";

        //        TwilioClient.Init(accountSid, authToken);

        //        var message = MessageResource.Create(
        //            body: model.Message,
        //            from: new Twilio.Types.PhoneNumber("+19034598720"),
        //            to: new Twilio.Types.PhoneNumber(model.CountryCode + model.PhoneNumber)
        //        );
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }


        //}


        public async Task<bool> SendSms(SMSModel model)
        {
            try
            {
                var accountSid = AppSetting.AccountSidTwilio;
                var authToken = AppSetting.AuthTokenTwilio;
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(model.CountryCode + model.PhoneNumber));
                messageOptions.MessagingServiceSid = AppSetting.MessagingServiceSid.ToString();
                messageOptions.Body = model.Message;

                var message = await MessageResource.CreateAsync(messageOptions);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task<bool> SendSms(SMSModel requestModel)
        //{
        //    // HubtelMessageRequest PostData = new HubtelMessageRequest();
        //    RouteMobileMessageRequest PostData = new RouteMobileMessageRequest();
        //    PostData.message = requestModel.Message;
        //    PostData.destination = requestModel.CountryCode + requestModel.PhoneNumber.IgnoreZero();

        //    // PostData.RegisteredDelivery = true;


        //    bool IsSuccess = false;
        //    HubtelMessageResponse res = new HubtelMessageResponse();
        //    string responseString = string.Empty;
        //    try
        //    {
        //        // string Url = ConfigurationManager.AppSettings["HubtelEndPoint"];
        //        string Url = ConfigurationManager.AppSettings["RouteMobileEndPoint"];
        //        String QueryString = new CommonMethods().GetQueryString(PostData);

        //        QueryString = QueryString.Replace("%2b", "");

        //        ASCIIEncoding ascii = new ASCIIEncoding();
        //        byte[] postBytes = ascii.GetBytes(QueryString.ToString());
        //        // set up request object
        //        HttpWebRequest request;
        //        string encoded = HttpUtility.UrlEncode(Url + "?" + QueryString, Encoding.UTF8);
        //        try
        //        {
        //            request = (HttpWebRequest)HttpWebRequest.Create(Url + "?" + QueryString);
        //        }
        //        catch (UriFormatException)
        //        {
        //            request = null;
        //        }

        //        request.KeepAlive = false;
        //        request.Method = "POST";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = postBytes.Length;
        //        // add post data to request
        //        using (Stream postStream = request.GetRequestStream())
        //        {
        //            postStream.Write(postBytes, 0, postBytes.Length);
        //            postStream.Flush();
        //            postStream.Close();
        //            using (var _response = (HttpWebResponse)request.GetResponse())
        //            {
        //                using (var result = new StreamReader(_response.GetResponseStream()))
        //                {

        //                    responseString = result.ReadToEnd();
        //                    var OtpResult = responseString.Split('|');
        //                    string MessageId = OtpResult[1];
        //                    // res = JsonConvert.DeserializeObject<HubtelMessageResponse>(responseString);
        //                    if (OtpResult != null && !string.IsNullOrEmpty(MessageId))
        //                    {
        //                        IsSuccess = true;
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return IsSuccess;

        //}

    }
}
