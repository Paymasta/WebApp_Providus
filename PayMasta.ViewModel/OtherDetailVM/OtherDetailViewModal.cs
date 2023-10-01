using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.OtherDetailVM
{
    #region DLVerificationRequest
    public class DLVerificationRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class DriversLicense
    {
        public string driversLicense { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthdate { get; set; }
        public string photo { get; set; }
        public string issued_date { get; set; }
        public string expiry_date { get; set; }
        public string state_of_issue { get; set; }
        public string gender { get; set; }
    }

    public class DriversLicenseCheck
    {
        public DriversLicenseCheck()
        {
            fieldMatches = new FieldMatches();
        }
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class DLVerificationResponse
    {
        public DLVerificationResponse()
        {
            applicant = new Applicant();
            summary = new Summary();
            status = new Status();
            drivers_license = new DriversLicense();
        }
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public DriversLicense drivers_license { get; set; }
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
            drivers_license_check = new DriversLicenseCheck();
        }
        public DriversLicenseCheck drivers_license_check { get; set; }
    }

    #endregion DLVerificationRequest

    #region 
    public class VCVerificationRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
    }
    #endregion
}
