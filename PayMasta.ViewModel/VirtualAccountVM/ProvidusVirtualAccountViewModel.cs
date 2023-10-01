using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VirtualAccountVM
{
    public class CreateVirtualAccountRequest
    {
        public string account_name { get; set; }
        public string bvn { get; set; }
    }
    public class CreateVirtualAccountResponse
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bvn { get; set; }
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }

}
