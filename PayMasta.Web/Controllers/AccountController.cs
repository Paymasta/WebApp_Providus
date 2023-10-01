using Newtonsoft.Json;
using PayMasta.Service.Account;
using PayMasta.Service.BankTransfer;
using PayMasta.Service.Employer.CommonEmployerService;
using PayMasta.Service.Okra;
using PayMasta.Service.ThirdParty;
using PayMasta.ViewModel;
using PayMasta.ViewModel.BankTransferVM;
using PayMasta.ViewModel.EmployerVM;
using PayMasta.ViewModel.OkraBankVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PayMasta.Web.Controllers
{
    //[Authorize]
    /// <summary>
    /// AccountController
    /// </summary>
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        private ICommonEmployerService _commonEmployerService;
        private IBankTransferService _bankTransferService;
        private IOkraService _okraService;
        private IThirdParty _thirdParty;
        public AccountController(IAccountService accountService, ICommonEmployerService commonEmployerService, IBankTransferService bankTransferService, IOkraService okraService, IThirdParty thirdParty)
        {
            //  _logUtils = logUtils;
            _accountService = accountService;
            _commonEmployerService = commonEmployerService;
            _bankTransferService = bankTransferService;
            _okraService = okraService;
            _thirdParty = thirdParty;
        }

        // GET: Account
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string returnReason)
        {
            FormsAuthentication.SignOut();
            TempData["ReturnReason"] = returnReason;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginRequest request)
        {
            var res = new LoginResponse();
            string name = string.Empty;
            List<string> listUserRoles = new List<string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Password) || !string.IsNullOrWhiteSpace(request.DeviceToken) || !string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    if (ModelState.IsValid)
                    {
                        res = await _accountService.Login(request);
                        if (res.RstKey == 1)
                        {
                            if (res.RoleId == 4)
                            {
                                listUserRoles.Add("Employee");
                            }
                            else if (res.RoleId == 3)
                            {
                                listUserRoles.Add("Employer");

                            }
                            //else if (res.RoleId == 5)
                            //{
                            //    listUserRoles.Add("Vendor");

                            //}
                            // listUserRoles.ToArray();
                            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
                            {
                                UserId = res.UserId,
                                FirstName = res.FirstName == null ? res.FirstName = res.Email : res.FirstName,
                                RoleId = res.RoleId,
                                Token = res.Token,
                                roles = listUserRoles.ToArray()
                            };

                            Session["User"] = res;
                            if (res.FirstName == null)
                            {
                                name = res.Email;
                            }
                            else
                            {
                                name = res.FirstName;
                            }
                            string userData = JsonConvert.SerializeObject(serializeModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(10), false, userData);
                            string encTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                            //if (authTicket.IsPersistent)
                            //{
                            //    faCookie.Expires = authTicket.Expiration;
                            //}
                            Response.Cookies.Add(faCookie);
                        }
                    }

                    //var jsonResult = JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> SignUp(SignUpRequest request)
        {
            var res = new SignupResponse();
            List<string> listUserRoles = new List<string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Password) || !string.IsNullOrWhiteSpace(request.DeviceToken) || !string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    if (ModelState.IsValid)
                    {
                        res = await _accountService.SignUp(request);
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
                            var otpres = await _accountService.SendOTP(otpReq);

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
                    //var jsonResult = JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [CustomAuthorize(Roles = "Employee")]
        [SessionExpireFilter]
        public async Task<ActionResult> CompleteProfile()
        {

            //if (Request.IsAuthenticated)
            //{
            //    //var users = (CustomPrincipal)(User);
            //    //if (users.RoleId == 4)
            //    //    return RedirectToAction("Create", "FootageRequest");
            //    //else
            //    //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    return View();
            //}
            return View();
        }

        [SessionExpireFilter]
        [HttpPost]
        public async Task<JsonResult> CompleteProfile(UpdateUserRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.FirstName) || !string.IsNullOrWhiteSpace(request.LastName) || !string.IsNullOrWhiteSpace(request.NinNo) || request.UserGuid != null)
                {
                    res = await _accountService.UpdateProfile(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> VerifyOTP(VerifyOTPRequest request)
        {
            var result = new LoginResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.MobileNumber) || !string.IsNullOrWhiteSpace(request.OtpCode))
                {
                    //if (ModelState.IsValid)
                    //{
                    result = await _accountService.VerifyOTPWeb(request);
                    // }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetCountry(string id)
        {
            var result = new GetCountryResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetCountry();

                }
                catch (Exception ex)
                {

                }
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetStateByCountryGuid(string guid)
        {

            var result = new GetSateResponse();
            var id = Guid.Parse(guid.ToString());
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetState(id);

                }
                catch (Exception ex)
                {

                }
            }

            return Json(result);
        }

        [HttpPost]

        public async Task<JsonResult> GetCityByStateGuid(string guid)
        {

            var result = new GetCityResponse();
            var id = Guid.Parse(guid.ToString());
            if (ModelState.IsValid)
            {
                try
                {
                    result = await _accountService.GetCityByStateGuid(id);

                }
                catch (Exception ex)
                {

                }
            }

            return Json(result);
        }

        [SessionExpireFilter]
        public async Task<ActionResult> MyProfile()
        {
            return View();
        }

        public async Task<ActionResult> CompleteEmployerProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> CompleteEmployerProfile(UpdateEmployerRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.OrganisationName) || !string.IsNullOrWhiteSpace(request.State) || !string.IsNullOrWhiteSpace(request.Country) || request.UserGuid != null)
                {
                    res = await _accountService.UpdateEmployerProfile(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetEmployerList(string id)
        {
            var result = new GetEmployerResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    result = await _commonEmployerService.GetEmployerList();

                }
                catch (Exception ex)
                {

                }
            }
            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordRequest request)
        {
            string res = string.Empty;

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) && request.Type > 0)
                {
                    res = await _accountService.ForgotPassword(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEmployerByUserGuid(UpdateEmployerByUserGuidRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmployerName) && request.EmployerId > 0)
                {
                    res = await _accountService.UpdateEmployer(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordRequest request)
        {
            var res = new OtpResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) || !string.IsNullOrWhiteSpace(request.OtpCode) || !string.IsNullOrWhiteSpace(request.Password))
                {
                    res = await _accountService.ResetPassword(request);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> VerifyForgetPasswordOTP(VerifyForgetPasswordOTPRequest request)
        {
            var result = new LoginResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(request.EmailorPhone) || !string.IsNullOrWhiteSpace(request.OtpCode) || request.Type > 0)
                {
                    //if (ModelState.IsValid)
                    //{
                    result = await _accountService.VerifyForgetPasswordOTP(request);
                    // }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<JsonResult> MyProfile(string guid)
        {
            var res = new UserModel();
            var id = Guid.Parse(guid.ToString());
            res = await _accountService.GetProfile(id);
            return Json(res);
        }

        [SessionExpireFilter]
        public async Task<ActionResult> ViewProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetBankList(int id)
        {
            var res = new GetBankListResponse();

            res = await _bankTransferService.GetBanks();
            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> GetBankListByUserGuid(string guid)
        {
            var res = new GetBanksListResponse();
            var id = Guid.Parse(guid.ToString());
            res = await _accountService.GetBankListByUserGuid(id);
            return Json(res);
        }

        // [SessionExpireFilter]
        [HttpPost]
        public JsonResult Logout(LogoutRequest request)
        {
            int res = 0;
            if (request.UserGuid != null)
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                res = 1;
            }

            return Json(res);
        }
        public ActionResult LogoutEmployer(LogoutRequest request)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
            //  return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordRequest request)
        {
            int result = 0;
            result = await _accountService.ChangePassword(request);
            //if (ModelState.IsValid)
            //{
            //    result = await _accountService.ChangePassword(request);
            //}
            //else
            //{
            //    result = 2;
            //}
            return Json(result);
        }
        public ActionResult UpdateProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> UpdateUserProfile(UpdateUserProfileRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();
            res = await _accountService.UpdateUserProfile(request);
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetOkraBankList(int id)
        {
            var res = new OkraBankResponse();

            res = await _okraService.GetOkraBankList();
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBankByBankDetailId(DeleteBankRequest request)
        {
            var res = new EmployeeUpdateProfileResponse();

            if (ModelState.IsValid)
            {
                res = await _accountService.DeleteBankByBankDetailId(request);
            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ResendOtp(ResendOTPRequest request)
        {
            int result = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    result = await _accountService.ResendOTP(request);

                }
            }

            catch (Exception ex)
            {

            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUserProfileRequest(UpdateUserProfileRequest request)
        {
            var result = new EmployeeUpdateProfileResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    result = await _accountService.UpdateUserProfileRequest(request);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> UploadFiles(HttpPostedFileBase file, string guid)
        {
            string fileName = "";
            var res = new UploadProfileImageResponse();
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        var id = Guid.Parse(guid.ToString());
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        fileName = Guid.NewGuid().ToString("n") + "." + file.FileName.Split('.')[1];
                        var imageUrl = await _thirdParty.UploadFiles(path, fileName);
                        if (imageUrl != null)
                        {
                            var req = new UploadProfileImageRequest { ImageUrl = imageUrl, UserGuid = id };
                            res = await _accountService.UploadProfileImage(req);

                        }
                        string path1 = Server.MapPath("~/UploadedFiles/" + file.FileName);
                        FileInfo file1 = new FileInfo(path1);
                        if (file1.Exists)//check file exsit or not  
                        {
                            file1.Delete();

                        }
                    }
                }
                catch (Exception ex)
                {
                    res.RstKey = 3;
                    res.Message = ex.Message;
                }
            }
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> AddBanks(AddBankListRequest request)
        {
            var result = new AddBankListResponse();
            try
            {
                result = await _accountService.InsertBank(request);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Invite(InviteRequest request)
        {
            var result = new OtpResponse();
            try
            {
                result = await _accountService.Invite(request);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GeneratePasscode(PasscodeRequest request)
        {
            var result = new PasscodeResponse();
            try
            {
                result = await _accountService.GeneratePasscode(request);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> VerifyPasscode(PasscodeRequest request)
        {
            var result = new PasscodeResponse();
            try
            {
                result = await _accountService.VerifyPasscode(request);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreateD2CEmployer(D2CEmployerRequest request)
        {
            var result = new D2CEmployerResponse();
            try
            {
                result = await _accountService.CreateD2CEmployer(request);
            }
            catch (Exception ex)
            {

            }

            return Json(result);
        }


        public ActionResult Test()
        {
           // readon
            return View();
        }
    }
}