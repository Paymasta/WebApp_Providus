using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VerifyNinVM
{
    public class VninVerifyRequest
    {
        public string idNumber { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string vNin { get; set; }
        public string photo { get; set; }
    }

    public class VninVerifyResponse
    {
        public VninVerifyResponse()
        {
            data = new Data();
        }
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class VerifyResponse
    {
        public VerifyResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
            vninVerifyResponses = new VninVerifyResponse();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public VninVerifyResponse vninVerifyResponses { get; set; }
    }

    public class VerifyRequest
    {
       
        [Required]
        public string VninNumber { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ErrorResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string error { get; set; }
    }


}
