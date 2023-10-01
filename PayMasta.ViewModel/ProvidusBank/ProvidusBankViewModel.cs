using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ProvidusBank
{
    public class ProvidusBankRequest
    {
        public string sessionId { get; set; }
        public string accountNumber { get; set; }
        public string tranRemarks { get; set; }
        public string transactionAmount { get; set; }
        public string settledAmount { get; set; }
        public string feeAmount { get; set; }
        public string vatAmount { get; set; }
        public string currency { get; set; }
        public string initiationTranRef { get; set; }
        public string settlementId { get; set; }
        public string sourceAccountNumber { get; set; }
        public string sourceAccountName { get; set; }
        public string sourceBankName { get; set; }
        public string channelId { get; set; }
        public string tranDateTime { get; set; }
    }
    public class GetProvidusResponse
    {
        public bool requestSuccessful { get; set; }
        public string sessionId { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }

    }
}
