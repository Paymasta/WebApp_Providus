using PayMasta.Service.Employer.CommonEmployerService;
using PayMasta.ViewModel.EmployerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    public class CommonEmployerController : Controller
    {
        // GET: CommonEmployer

        private ICommonEmployerService _commonEmployerService;

        public CommonEmployerController(ICommonEmployerService commonEmployerService)
        {
            _commonEmployerService = commonEmployerService;
        }

        [HttpPost]
        public async Task<JsonResult> GetNonRegisteredEmployerList(string searchText)
        {
            var result = new GetNonRegisterEmployerResponse();
            try
            {
                result = await _commonEmployerService.GetNonRegisteredEmployerList(searchText);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }
    }
}