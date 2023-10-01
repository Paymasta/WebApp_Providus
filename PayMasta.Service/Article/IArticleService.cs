using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Article
{
    public interface IArticleService
    {
        Task<ApiResponseVM<ArticleViewModel>> GetArticleById(long articleId);
        Task<ArticleListResponse> GetArticleList(Guid userGuid);
        Task<ApiResponseVM<Object>> SaveArticle(ArticleViewModel request);
        Task<ApiResponseVM<object>> SubmitArticleAnswer(ArticleAnsRequest request);
    }
}
