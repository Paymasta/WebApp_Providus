using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.ManageFinanceVM
{
    public class ManageFinanceRequest
    {
        public Guid UserGuid { get; set; }
    }

    public class GetManageFinance
    {
        public string AIRTIME { get; set; }
        public string CABLE { get; set; }
        public string INTERNET { get; set; }
        public string Bills { get; set; }
        public string Ecom { get; set; }
        public string Travel { get; set; }
        public string AvgPerDaySpend { get; set; }
        public int OkToSpend { get; set; }
    }

    public class GetManageFinanceResponse
    {
        public GetManageFinanceResponse()
        {
            manageFinance = new GetManageFinance();
        }
        public GetManageFinance manageFinance { get; set; }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
       
    }
}
