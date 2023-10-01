using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayMasta.ViewModel.LidyaVM;
using System.Net;

namespace PayMasta.Service.ThirdParty
{
    public class LidyaThirdParty : ILidyaThirdParty
    {
        public async Task<LidyaAuthResponse> LidyaAuth(object req, string url)
        {
            var resBody = new LidyaAuthResponse();
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var data = JsonConvert.SerializeObject(req);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("application", AppSetting.LidyaAuthHeader);
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSetting.ExpressWalletSecretKey);
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    // response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        // Get the value of a specific response header
                        if (response.Headers.TryGetValues("X_AUTH", out IEnumerable<string> values))
                        {
                            string headerValue = values.First();
                            resBody.Token = headerValue;
                        }
                    }
                    resBody.Data = await response.Content.ReadAsStringAsync();
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
        public async Task<string> LidyaCreateMandate(object req, string url, string token)
        {
            string resBody = "";
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var data = JsonConvert.SerializeObject(req);

                    CookieContainer cookieContainer = new CookieContainer();
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.CookieContainer = cookieContainer;
                    if (cookieContainer.GetCookies(new Uri(url))["X_AUTH"] == null)
                    {
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Add("application", AppSetting.LidyaAuthHeader);
                        client.DefaultRequestHeaders.Add("X_AUTH", token);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        //response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            resBody = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                return resBody;
            }
        }
    }
}
