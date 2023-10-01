using PayMasta.Service.Article;
using PayMasta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    public class ArticleController : Controller
    {

        private IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetArticleList(Guid userGuid)
        {
            var result = new ArticleListResponse();

            try
            {
                result = await _articleService.GetArticleList(userGuid);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> SubmitArticleAnswer(ArticleAnsRequest request)
        {
            var result = new ApiResponseVM<Object>();

            try
            {
                result = await _articleService.SubmitArticleAnswer(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}