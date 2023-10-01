using PayMasta.Service.Okra;
using PayMasta.ViewModel.OkraVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    public class OkraController : Controller
    {
        private IOkraService _okraService;
        public OkraController(IOkraService okraService)
        {
            _okraService = okraService;
        }
        // GET: Okra
       
    }
}