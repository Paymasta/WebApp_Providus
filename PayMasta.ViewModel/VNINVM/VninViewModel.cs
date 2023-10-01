using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VNINVM
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class VninResponse
    {
        public VninResponse()
        {
            applicant = new Applicant();
            summary = new Summary();
            status = new Status();
            v_nin = new VNin();
        }
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public VNin v_nin { get; set; }
    }

    public class Status
    {
        public string state { get; set; }
        public string status { get; set; }
    }

    public class Summary
    {
        public Summary()
        {
            v_nin_check = new VNinCheck();
        }
        public VNinCheck v_nin_check { get; set; }
    }

    public class VNin
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string vNin { get; set; }
        public string photo { get; set; }
    }

    public class VNinCheck
    {
        public VNinCheck()
        {
            fieldMatches = new FieldMatches();
        }
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }


}
