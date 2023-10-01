using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Enums
{
    public enum EnumUserType
    {
        All = 0,
        SuperAdmin = 1,
        SubAdmin,
        Employer,
        Customer
    }

    public enum EnumUserRoleGroup
    {
        SuperAdmin = 1,
        SubAdmin,
        Supplier,
        Wholesaler,
        Retailer,
        Merchandiser,
        Driver,
        POS,
        Customer,
        SupplierStaff,
        RetailerStaff
    }

    public enum EnumOtpType
    {
        SignUp = 1,
        ForgotPassword
    }

    public enum EnumLanguage
    {
        en = 1,
        ar
    }

    public enum EnumPosSession
    {
        Open = 1,
        Close
    }

    public enum EnumDocType
    {
        QID = 1,
        DrivingLicence
    }

    public enum EnumSupportStatus
    {
        Pending,
        Verified,
        Rejected,
        Resubmit
    }
}
