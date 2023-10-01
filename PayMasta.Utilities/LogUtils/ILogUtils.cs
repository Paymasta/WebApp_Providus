using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.LogUtils
{
    public interface ILogUtils
    {
        void WriteTextToFile(string message);
        void WriteTextToFileForPush(string message);
    }
}
