using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.SupportVM
{
    public class SupportResponse
    {
        public SupportResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

    }

    public class SupportRequest
    {
        public Guid UserGuid { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string DescriptionText { get; set; }

    }

    public class SupportMasterTicketResponse
    {
        public long UserId { get; set; }
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string DescriptionText { get; set; }
        public string Status { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public int StatusId { get; set; }

        public int TotalCount { get; set; }
        public int RowNumber { get; set; }
    }

    public class SupportMasterResponse
    {
        public SupportMasterResponse()
        {
            this.IsSuccess = false;
            this.RstKey = 0;
            this.Message = string.Empty;
            supportMasterTicketResponse = new List<SupportMasterTicketResponse>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public List<SupportMasterTicketResponse> supportMasterTicketResponse { get; set; }
    }

    public class GetSupportMasterRequest
    {
        public GetSupportMasterRequest()
        {
            this.PageSize = 10;
            this.pageNumber = 1;
        }
        public Guid UserGuid { get; set; }
        public int pageNumber { get; set; }

        public int PageSize { get; set; }
    }
    public class GetSupportListRequest
    {
        public Guid UserGuid { get; set; }
      
    }
}
