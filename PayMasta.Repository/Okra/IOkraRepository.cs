using PayMasta.DBEntity.OkraCallBack;
using PayMasta.DBEntity.WidgetLinkMaster;
using PayMasta.ViewModel.OkraVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Okra
{
    public interface IOkraRepository
    {
        Task<OkraCallBackResponse> GetIncomeUrlByUserId(long userId, string CallBackType, string bankId, IDbConnection exdbConnection = null);
        Task<WidgetLinkMaster> InsertWidgetLinkDetail(WidgetLinkMaster userEntity, IDbConnection exdbConnection = null);
        Task<WidgetLinkMaster> GetWidgetLinkByUserId(long userId, IDbConnection exdbConnection = null);
        Task<List<LinkedOrUnlinkedBank>> GetLinkedOrUnLinkedBank(long userId, IDbConnection exdbConnection = null);
    }
}
