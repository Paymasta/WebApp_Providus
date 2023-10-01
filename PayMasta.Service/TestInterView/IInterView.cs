using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.TestInterView
{
    public interface IInterView
    {
        Task<string> RefFunction(int refOutValue);
        Task<string> OutFunction(int refOutValue);
        Task<string> ReverseString(string str);
    }
}
