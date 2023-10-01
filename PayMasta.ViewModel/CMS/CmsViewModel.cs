using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.CMS
{
    public class CmsResponse
    {
        public CmsResponse()
        {
            getCms = new GetCms();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }

        public GetCms getCms { get; set; }
    }
    public class GetCms
    {
        public string Detail { get; set; }
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }

    public class FAQResponse
    {
        public FAQResponse()
        {
            this.QuestionText = string.Empty;
            this.Detail = string.Empty;
            this.FaqDetails = new List<FaqDetailResponse>();
        }
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Detail { get; set; }
        public List<FaqDetailResponse> FaqDetails { get; set; }
    }
    public class FaqDetailResponse
    {
        public FaqDetailResponse()
        {
            this.Detail = string.Empty;
        }
        public int Id { get; set; }
        public int FaqId { get; set; }
        public string Detail { get; set; }
    }

    public class RequestDemoRequest
    {
        public string FirrstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanySize { get; set; }
        public string Detail { get; set; }
    }
}
