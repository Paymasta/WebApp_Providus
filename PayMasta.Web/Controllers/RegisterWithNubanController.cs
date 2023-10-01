using Newtonsoft.Json;
using PayMasta.Service.Account;
using PayMasta.Service.BankTransfer;
using PayMasta.Service.Employer.CommonEmployerService;
using PayMasta.Service.Okra;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.VerifyNin;
using PayMasta.ViewModel;
using PayMasta.ViewModel.VerifyNinVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PayMasta.Web.Controllers
{
    public class RegisterWithNubanController : Controller
    {
        private IVerifyNinService _verifyNinService;

        public RegisterWithNubanController(IVerifyNinService verifyNinService)
        {
            _verifyNinService = verifyNinService;
        }
        // GET: RegisterWithNuban
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RegisterWithNuban(RegisterByNubanRequest request)
        {
            var res = new QoreIdBvnNubanResult();
            List<string> listUserRoles = new List<string>();
            try
            {
                res = await _verifyNinService.VerifyNuban(request);
                if (res.RstKey == 11)
                {
                    if (res.RoleId == 4)
                    {
                        listUserRoles.Add("Employee");
                    }
                    else if (res.RoleId == 3)
                    {
                        listUserRoles.Add("Employer");

                    }
                    var otpReq = new OtpRequest
                    {
                        Email = request.Email,
                        IsdCode = request.CountryCode,
                        MobileNo = request.PhoneNumber
                    };

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
                    {
                        UserId = res.UserId,
                        FirstName = res.Email,
                        RoleId = res.RoleId,
                        Token = res.Token,
                        roles = listUserRoles.ToArray()
                    };

                    Session["User"] = res;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, res.MobileNumber, DateTime.Now, DateTime.Now.AddMinutes(10), false, userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //if (authTicket.IsPersistent)
                    //{
                    //    faCookie.Expires = authTicket.Expiration;
                    //}
                    Response.Cookies.Add(faCookie);
                }
            }
            catch (Exception ex)
            {
                res.RstKey = 999;
                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return Json(res);
        }

        //This list from qoreid and systemspecs both
        [HttpPost]
        public async Task<JsonResult> BankListForRegister(int guid)
        {
            var res = new BankListResult();
            try
            {
                res = await _verifyNinService.BankListForRegister();
            }
            catch (Exception ex)
            {
                res.RstKey = 999;
                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> VerifyOTPWeb(VerifyOTPRequest request)
        {
            var res = new IsOtpVerifiedResponse();
            try
            {
                res = await _verifyNinService.VerifyOTPWeb(request);
            }
            catch (Exception ex)
            {
                res.RstKey = 999;
            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> AddUserOtherDetail(AddOtherDetailPRequest request)
        {
            var res = new AddOtherDetailResponse();
            try
            {
                res = await _verifyNinService.AddUserOtherDetail(request);
            }
            catch (Exception ex)
            {
                res.RstKey = 999;
            }

            return Json(res);
        }
    }
}