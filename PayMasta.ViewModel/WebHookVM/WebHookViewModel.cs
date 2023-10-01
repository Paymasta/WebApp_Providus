using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.WebHookVM
{
    public class WebHookRequest
    {
        public string MyProperty { get; set; }
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyPropert3 { get; set; }
    }
    public class WebHookResponse
    {
        public string responseMessage { get; set; }
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyPropert3 { get; set; }
    }
}
