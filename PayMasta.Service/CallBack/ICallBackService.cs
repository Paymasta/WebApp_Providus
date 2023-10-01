using PayMasta.ViewModel.CallBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.CallBack
{
    public interface ICallBackService
    {
        Task<WidgetResponse> WidgetCallBack(AuthWidgetResponse request);
    }
}
