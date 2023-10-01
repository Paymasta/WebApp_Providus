using PayMasta.DBEntity.Support;
using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.SupportRepository
{
    public interface ISupportRepository
    {
        Task<int> InsertSupportDetail(SupportMaster supporEntity, IDbConnection exdbConnection = null);
        Task<List<SupportMasterTicketResponse>> GetSupportDetailList(long userId, int pagenumber, int pagesize, IDbConnection exdbConnection = null);
        Task<List<SupportMasterTicketResponse>> GetSupportDetailList(long userId, IDbConnection exdbConnection = null);
    }
}
