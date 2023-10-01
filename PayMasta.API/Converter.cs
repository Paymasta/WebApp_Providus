using Newtonsoft.Json;
using PayMasta.Utilities.Common;
using PayMasta.Utilities.LogUtils;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PayMasta.API
{
    /// <summary>
    /// Converter
    /// </summary>
    public class Converter : ApiController
    {
        /// <summary>
        /// ApiResponseMessage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public IHttpActionResult ApiResponseMessage<T>(Response<T> dataObject, HttpStatusCode statusCode) where T : class
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            IHttpActionResult responseIHttpActionResult;
            httpResponseMessage.Content = new ObjectContent<Response<T>>(dataObject, new JsonMediaTypeFormatter());

            httpResponseMessage.StatusCode = statusCode;
            responseIHttpActionResult = ResponseMessage(httpResponseMessage);
            return responseIHttpActionResult;
        }
        //public IHttpActionResult ApiResponseMessage<T>(Response<T> dataObject, HttpStatusCode statusCode) where T : class
        //{
        //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        //    IHttpActionResult responseIHttpActionResult;
        //    httpResponseMessage.Content = new ObjectContent<Response<T>>(dataObject, new JsonMediaTypeFormatter());
        //    string responseSrting = JsonConvert.SerializeObject(dataObject);
        //    string resultString = new EncrDecr<Response<T>>().Encrypt(responseSrting);
        //    httpResponseMessage.Content = new ObjectContent<string>(resultString, new JsonMediaTypeFormatter());
        //    httpResponseMessage.StatusCode = statusCode;
        //    responseIHttpActionResult = ResponseMessage(httpResponseMessage);
        //    return responseIHttpActionResult;
        //}

        #region UploadFile

        private HttpContextWrapper GetHttpContext(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]);
            }
            else if (HttpContext.Current != null)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
    public class EncrDecr<T>
    {
        public string Encryptword(string Encryptval)
        {
            var key = "ccfb4ff9-8e89-43ae-8f5c-c9a9b15ed02b";
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(Encryptval);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
        public T Decryptword(string DecryptText)
        {
            var key = "ccfb4ff9-8e89-43ae-8f5c-c9a9b15ed02b";
            byte[] SrctArray;
            byte[] DrctArray = Convert.FromBase64String(DecryptText);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();
            SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objmdcript.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateDecryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);
            objt.Clear();
           
            return JsonConvert.DeserializeObject<T>(UTF8Encoding.UTF8.GetString(resArray));
        }
        public string Encrypt(string Encryptval)
        {
            try
            {
                string textToEncrypt = Encryptval;
                string ToReturn = "";
                string publickey = "rajdeeps";
                string secretkey = "engineer";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public T Decrypt(string request)
        {
            try
            {
                string textToDecrypt = request;
                string ToReturn = "";
                string publickey = "rajdeeps";
                string privatekey = "engineer";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(privatekey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return JsonConvert.DeserializeObject<T>(ToReturn);
               // return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
    }
    //public class EncrDecr<T>
    //{
    //    private readonly ILogUtils _logUtils;
    //    public EncrDecr()
    //    {
    //        _logUtils = new LogUtils();
    //    }
    //    public T Decrypt(string value, bool isTempToken = false, HttpRequestMessage httpRequestMessage = null)
    //    {
    //        if (GlobalData.RoleId == 1)
    //        {
    //            if (isTempToken)
    //            {
    //                var keys = new TokenService().KeysByTempToken();
    //                value = new Cryptography2(keys.PrivateKey).Decrypt(value);
    //            }
    //            else
    //            {
    //                var keys = new TokenService().KeysBySessionToken();
    //                value = new Cryptography2(keys.PrivateKey).Decrypt(value);
    //                // var d = JsonConvert.DeserializeObject<T>(value);
    //                // var d = httpRequestMessage.Headers.GetValues("token").FirstOrDefault();
    //            }


    //            return JsonConvert.DeserializeObject<T>(value);
    //        }
    //        else
    //        {
    //            if (isTempToken)
    //            {
    //                var keys = new TokenService().KeysByTempToken();
    //                value = AES256.Decrypt(keys.PrivateKey, value);
    //                return JsonConvert.DeserializeObject<T>(value);
    //            }
    //            else
    //            {
    //                var keys = new TokenService().KeysBySessionToken();
    //                value = AES256.Decrypt(keys.PrivateKey, value);
    //                return JsonConvert.DeserializeObject<T>(value);
    //            }
    //        }
    //    }

    //    public string Encrypt(T response, bool isTempToken = false, HttpRequestMessage httpRequestMessage = null)
    //    {
    //        if (GlobalData.RoleId == 1)
    //        {
    //            if (isTempToken)
    //            {
    //                var keys = new TokenService().KeysByTempToken();
    //                string responseSrting = JsonConvert.SerializeObject(response);
    //                return new Cryptography2(keys.PrivateKey).Encrypt(responseSrting);
    //            }
    //            else
    //            {
    //                var keys = new TokenService().KeysBySessionToken();
    //                string responseSrting = JsonConvert.SerializeObject(response);
    //                return new Cryptography2(keys.PrivateKey).Encrypt(responseSrting);
    //            }
    //        }
    //        else
    //        {
    //            if (isTempToken)
    //            {
    //                var keys = new TokenService().KeysByTempToken();
    //                string responseSrting = JsonConvert.SerializeObject(response);
    //                return AES256.Encrypt(keys.PrivateKey, responseSrting);
    //            }
    //            else
    //            {
    //                var keys = new TokenService().KeysBySessionToken();
    //                string responseSrting = JsonConvert.SerializeObject(response);
    //                return AES256.Encrypt(keys.PrivateKey, responseSrting);
    //            }
    //        }
    //    }

    //    //private void SendToLog(LogMetadata logMetadata)
    //    //{
    //    //    if (!logMetadata.RequestUri.Contains("swagger"))
    //    //    {
    //    //        // TODO: Write code here to store the logMetadata instance to a pre-configured log store...
    //    //        _logUtils.WriteTextToFile(JsonConvert.SerializeObject(logMetadata));
    //    //    }
    //    //}
    //    //private void SendToLogApps(LogMetadata logMetadata)
    //    //{
    //    //    if (!logMetadata.RequestUri.Contains("swagger"))
    //    //    {
    //    //        // TODO: Write code here to store the logMetadata instance to a pre-configured log store...
    //    //        _logUtils.WriteTextToFileApps(JsonConvert.SerializeObject(logMetadata));
    //    //    }
    //    //}

    //    //private void CreateLogRequest(HttpRequestMessage httpRequestMessage, string value)
    //    //{
    //    //    if (GlobalData.AppId == (int)DeviceTypes.Admin)
    //    //    {
    //    //        if (httpRequestMessage != null)
    //    //        {
    //    //            try
    //    //            {
    //    //                if (httpRequestMessage.RequestUri.ToString().Contains("Login"))
    //    //                {
    //    //                    var d = JsonConvert.DeserializeObject<LoginRequest>(value);
    //    //                    d.Password = "**********";
    //    //                    var changePass = JsonConvert.SerializeObject(d);
    //    //                    LogMetadata log = new LogMetadata
    //    //                    {
    //    //                        Type = "Request",
    //    //                        RequestMethod = httpRequestMessage.Method.Method,
    //    //                        RequestTimestamp = DateTime.Now,
    //    //                        RequestUri = httpRequestMessage.RequestUri.ToString(),
    //    //                        RequestBody = changePass
    //    //                    };
    //    //                    SendToLog(log);
    //    //                }
    //    //                else
    //    //                {
    //    //                    var d = JsonConvert.DeserializeObject<dynamic>(value);
    //    //                    d.Password = "**********";
    //    //                    var changePass = JsonConvert.SerializeObject(d);
    //    //                    LogMetadata log = new LogMetadata
    //    //                    {
    //    //                        Type = "Request",
    //    //                        RequestMethod = httpRequestMessage.Method.Method,
    //    //                        RequestTimestamp = DateTime.Now,
    //    //                        RequestUri = httpRequestMessage.RequestUri.ToString(),
    //    //                        RequestBody = changePass
    //    //                    };
    //    //                    SendToLog(log);
    //    //                }

    //    //            }
    //    //            catch { }
    //    //        }
    //    //    }
    //    //    else if (GlobalData.AppId == 2 || GlobalData.AppVersion == 2)
    //    //    {
    //    //        if (httpRequestMessage != null)
    //    //        {
    //    //            try
    //    //            {
    //    //                if (httpRequestMessage.RequestUri.ToString().Contains("Login"))
    //    //                {
    //    //                    var d = JsonConvert.DeserializeObject<LoginRequest>(value);
    //    //                    d.Password = "**********";
    //    //                    var changePass = JsonConvert.SerializeObject(d);
    //    //                    LogMetadata log = new LogMetadata
    //    //                    {
    //    //                        Type = "Request",
    //    //                        RequestMethod = httpRequestMessage.Method.Method,
    //    //                        RequestTimestamp = DateTime.Now,
    //    //                        RequestUri = httpRequestMessage.RequestUri.ToString(),
    //    //                        RequestBody = changePass
    //    //                    };
    //    //                    SendToLogApps(log);
    //    //                }
    //    //                else
    //    //                {
    //    //                    var d = JsonConvert.DeserializeObject<dynamic>(value);
    //    //                    d.Password = "**********";
    //    //                    var changePass = JsonConvert.SerializeObject(d);
    //    //                    LogMetadata log = new LogMetadata
    //    //                    {
    //    //                        Type = "Request",
    //    //                        RequestMethod = httpRequestMessage.Method.Method,
    //    //                        RequestTimestamp = DateTime.Now,
    //    //                        RequestUri = httpRequestMessage.RequestUri.ToString(),
    //    //                        RequestBody = changePass
    //    //                    };
    //    //                    SendToLogApps(log);
    //    //                }

    //    //            }
    //    //            catch { }
    //    //        }
    //    //    }
    //    //}
    //}
}