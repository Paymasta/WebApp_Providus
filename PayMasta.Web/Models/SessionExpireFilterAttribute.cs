using PayMasta.Service.TokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayMasta.Web.Models
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        private ITokenService _tokenService;
        public SessionExpireFilterAttribute()
        {
            _tokenService = new TokenService();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;

            //string token = httpContext.User["Token"];

            //var IsValidToken = _tokenService.IsSessionValid(token);
            CustomPrincipal customPrincipal = (CustomPrincipal)httpContext.User;
            var IsValidToken = _tokenService.IsSessionValid(customPrincipal.Token.ToString());
            if (IsValidToken)
            {
                if (httpContext.User == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Index" }));
                    return;
                }
                if (httpContext.User.Identity.IsAuthenticated == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Index" }));
                    return;
                }


                base.OnActionExecuting(filterContext);
            }
            else
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Index"  }));
                filterContext.Result =
          new RedirectToRouteResult(new RouteValueDictionary
            {
             { "action", "Index" },
            { "controller", "Account" },
            { "returnReason","Token Expire"}
             });
                return;
            }
        }
    }
}