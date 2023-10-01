using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.ProvidusVirtualAccountDetail
{
    public class ProvidusVirtualAccountDetail : BaseEntity
    {

        public long UserId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public bool IsRequestSuccessful { get; set; }
        public string ResponseMessage { get; set; }
        public int ResponseCode { get; set; }
        public string Bvn { get; set; }
        public string InitiationTranRef { get; set; }

    }
}
