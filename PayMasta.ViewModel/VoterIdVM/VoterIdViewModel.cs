using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.VoterIdVM
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
        public bool dob { get; set; }
    }

    public class VoterIdResponse
    {
        public VoterIdResponse()
        {
            applicant = new Applicant();
            summary = new Summary();
            status = new Status();
            voters_card = new VotersCard();
        }
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public VotersCard voters_card { get; set; }
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
            voters_card_check = new VotersCardCheck();
        }
        public VotersCardCheck voters_card_check { get; set; }
    }

    public class VotersCard
    {
        public string fullname { get; set; }
        public string vin { get; set; }
        public string gender { get; set; }
        public string occupation { get; set; }
        public string pollingUnitCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class VotersCardCheck
    {
        public VotersCardCheck()
        {
            fieldMatches = new FieldMatches();
        }
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }


}
