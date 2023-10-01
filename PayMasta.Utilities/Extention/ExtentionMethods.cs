using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.Extention
{
    public static class ExtensionMethods
    {
        public static string IgnoreZero(this string MobileNo)
        {
            try
            {

                if (MobileNo.Substring(0, 1) == "0")
                {
                    return MobileNo.Substring(1, MobileNo.Length - 1);
                }
                else
                {
                    return MobileNo;
                }
            }
            catch
            {

                return MobileNo;
            }
        }
       
    }
}
