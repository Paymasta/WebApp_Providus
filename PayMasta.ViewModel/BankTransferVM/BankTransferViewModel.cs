using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.BankTransferVM
{

    public class Bank
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
    }

    public class BankTransferViewModel
    {
        public BankTransferViewModel()
        {
            banks = new List<Bank>();
        }
        public List<Bank> banks { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }

    public class GetBankListResponse
    {
        public GetBankListResponse()
        {
            banks = new List<Bank>();
        }
        public List<Bank> banks { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public int RstKey { get; set; }
    }

    public class GetNIPAccountRequest
    {
        [Required]
        public string accountNumber { get; set; }
        public string beneficiaryBank { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class GetNIPAccountResponse
    {
        public string bankCode { get; set; }
        public string accountName { get; set; }
        public string transactionReference { get; set; }
        public string bvn { get; set; }
        public string responseMessage { get; set; }
        public string accountNumber { get; set; }
        public string responseCode { get; set; }
    }
    public class GetAccountResponse
    {
        public GetAccountResponse()
        {
            getNIPAccountResponse = new GetNIPAccountResponse();
        }
        public GetNIPAccountResponse getNIPAccountResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }

    public class GetAccountRequest
    {
        [Required]
        public string AccountNumber { get; set; }
        public string BeneficiaryBank { get; set; }
        [Required]
        public Guid UserGuid { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class NIPFundTransferRequest
    {
        public string beneficiaryAccountName { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string sourceAccountName { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryBank { get; set; }
        public string transactionReference { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class FundTransferRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string beneficiaryAccountName { get; set; }
        [Required]
        public string transactionAmount { get; set; }
        [Required]
        public string sourceAccountName { get; set; }
        [Required]
        public string beneficiaryAccountNumber { get; set; }
        [Required]
        public string beneficiaryBank { get; set; }
        //public string e { get; set; }

    }
    public class FundTransferResponse
    {
        public FundTransferResponse()
        {
            nIPFundTransferResponse = new NIPFundTransferResponse();
        }
        public NIPFundTransferResponse nIPFundTransferResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }

    public class NIPFundTransferResponse
    {
        public string transactionReference { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }

    public class ProvidusFundTransfer
    {
        public string creditAccount { get; set; }
        public string debitAccount { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string transactionReference { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class ProvidusFundTransferResponse
    {
        public string amount { get; set; }
        public string transactionReference { get; set; }
        public string currency { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }
    public class ProvidusFundTransferRequest
    {
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string DebitAccount { get; set; }
        [Required]
        public string TransactionAmount { get; set; }
        public string AccountHolderName { get; set; }
        public string BVN { get; set; }
    }

    public class ProvidusFundResponse
    {
        public ProvidusFundResponse()
        {
            transferResponse = new ProvidusFundTransferResponse();
        }
        public ProvidusFundTransferResponse transferResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }
    public class GetProvidusAccount
    {
        public string accountNumber { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class GetProvidusAccountResponse
    {
        public string accountStatus { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string accountName { get; set; }
        public string bvn { get; set; }
        public string accountNumber { get; set; }
        public string cbaCustomerID { get; set; }
        public string responseMessage { get; set; }
        public string availableBalance { get; set; }
        public string responseCode { get; set; }
    }
    public class ProvidusAccountResponse
    {
        public ProvidusAccountResponse()
        {
            accountResponse = new GetProvidusAccountResponse();
        }
        public GetProvidusAccountResponse accountResponse { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int RstKey { get; set; }
    }

}
