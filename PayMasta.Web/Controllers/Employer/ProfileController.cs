using PayMasta.Service.Account;
using PayMasta.Service.Employer.EmployerProfile;
using PayMasta.Service.ThirdParty;
using PayMasta.ViewModel;
using PayMasta.ViewModel.EmployerVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers.Employer
{
    [CustomAuthorize(Roles = "Employer")]
    [SessionExpireFilter]
    public class ProfileController : MyBaseController
    {
        private IAccountService _accountService;
        private IThirdParty _thirdParty;
        private IEmployerProfileService _employerProfileService;
        public ProfileController(IAccountService accountService, IThirdParty thirdParty, IEmployerProfileService employerProfileService)
        {
            _thirdParty = thirdParty;
            _accountService = accountService;
            _employerProfileService = employerProfileService;
        }
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdateEmployerProfile()
        {
            return View();
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
        [SessionExpireFilter]
        public async Task<JsonResult> MyProfile(string guid)
        {
            var res = new UserModel();
            var id = Guid.Parse(guid.ToString());
            res = await _accountService.GetProfile(id);
            return Json(res);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<JsonResult> GetEmployerProfileForUpdate(string guid)
        {
            var res = new GetEmployerProfile();

            res = await _employerProfileService.GetEmployerProfileForUpdate(guid);
            return Json(res);
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
        [SessionExpireFilter]
        public async Task<JsonResult> UpdateEmployerProfile(UpdateEmployerRequest request)
        {
            var res = new EmployerUpdateProfileResponse();

            try
            {
                res = await _employerProfileService.UpdateEmployerProfile(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

    }
}