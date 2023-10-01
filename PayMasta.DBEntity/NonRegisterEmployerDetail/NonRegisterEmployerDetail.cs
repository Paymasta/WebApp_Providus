using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.NonRegisterEmployerDetail
{
    public class NonRegisterEmployerDetail : BaseEntity
    {
        public string OrganisationName { get; set; }
        public string OrganisationCode { get; set; }
        public int Status { get; set; }
    }

}
