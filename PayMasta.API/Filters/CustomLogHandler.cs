using Newtonsoft.Json;
using PayMasta.Utilities.LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PayMasta.API.Filters
{
    /// <summary>
    /// CustomLogHandler
    /// </summary>
    public class CustomLogHandler : DelegatingHandler
    {
        private readonly ILogUtils _logUtils;

        /// <summary>
        /// Ctor
        /// </summary>
        public CustomLogHandler()
        {
            _logUtils = new LogUtils();
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logMetadata = BuildRequestMetadata(request);
            var response = await base.SendAsync(request, cancellationToken);
            logMetadata = BuildResponseMetadata(logMetadata, response);
            await SendToLog(logMetadata);
            return response;
        }

        /// <summary>
        /// BuildRequestMetadata
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private LogMetadata BuildRequestMetadata(HttpRequestMessage request)
        {
            string JsonContent = string.Empty;
            try { JsonContent = request.Content.ReadAsStringAsync().Result; } catch { }

            LogMetadata log = new LogMetadata
            {
                RequestMethod = request.Method.Method,
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString(),
                RequestBody = JsonContent
            };
            return log;
        }

        /// <summary>
        /// BuildResponseMetadata
        /// </summary>
        /// <param name="logMetadata"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private LogMetadata BuildResponseMetadata(LogMetadata logMetadata, HttpResponseMessage response)
        {
            string JsonContent = string.Empty;
            try { JsonContent = response.Content.ReadAsStringAsync().Result; } catch { }

            logMetadata.ResponseStatusCode = response.StatusCode;
            logMetadata.ResponseBody = JsonContent;
            logMetadata.ResponseTimestamp = DateTime.Now;
            try
            {
                logMetadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;
            }
            catch { }

            return logMetadata;
        }

        /// <summary>
        /// SendToLog
        /// </summary>
        /// <param name="logMetadata"></param>
        /// <returns></returns>
        private async Task<bool> SendToLog(LogMetadata logMetadata)
        {
            if (!logMetadata.RequestUri.Contains("swagger"))
            {
                // TODO: Write code here to store the logMetadata instance to a pre-configured log store...
                _logUtils.WriteTextToFile(JsonConvert.SerializeObject(logMetadata));
            }
            return true;
        }

    }

    /// <summary>
    /// LogMetadata
    /// </summary>
    public class LogMetadata
    {
        /// <summary>
        /// RequestContentType
        /// </summary>
        public string RequestContentType { get; set; }
        /// <summary>
        /// RequestUri
        /// </summary>
        public string RequestUri { get; set; }
        /// <summary>
        /// RequestMethod
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// RequestTimestamp
        /// </summary>
        public DateTime? RequestTimestamp { get; set; }
        /// <summary>
        /// RequestBody
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// ResponseContentType
        /// </summary>
        public string ResponseContentType { get; set; }
        /// <summary>
        /// ResponseStatusCode
        /// </summary>
        public HttpStatusCode ResponseStatusCode { get; set; }
        /// <summary>
        /// ResponseTimestamp
        /// </summary>
        public DateTime? ResponseTimestamp { get; set; }
        /// <summary>
        /// ResponseBody
        /// </summary>
        public string ResponseBody { get; set; }
    }
}