//using PayMasta.Service.FlutterWave;
using PayMasta.Service.ItexRechargeService;
using PayMasta.Service.ItexService;
using PayMasta.ViewModel.AirtimeVM;
using PayMasta.ViewModel.BouquetsVM;
using PayMasta.ViewModel.DataRechargeVM;
using PayMasta.ViewModel.ElectricityPurchaseVM;
using PayMasta.ViewModel.InternetVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.MultiChoicePurchaseVM;
using PayMasta.ViewModel.PayAirtimeAndOtherBillsVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.PurchaseInternetVM;
using PayMasta.ViewModel.PurchaseStarLineVM;
using PayMasta.ViewModel.ValidatInternetVM;
using PayMasta.ViewModel.WalletToBankVM;
//using PayMasta.ViewModel.FlutterWaveVM;
using PayMasta.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayMasta.Web.Controllers
{
    [CustomAuthorize(Roles = "Employee")]
    [SessionExpireFilter]
    public class BillAndPaymentController : MyBaseController
    {
        // GET: BillAndPayment
        private IItexService _itexService;
        private IItexRechargeService _itexRechargeService;
        public BillAndPaymentController(IItexService itexService, IItexRechargeService itexRechargeService)
        {
            _itexService = itexService;
            _itexRechargeService = itexRechargeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAirtimeOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _itexService.GetAirtimeOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetDataOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _itexService.GetDataOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetDataOperatorPlanList(OperatorPlanRequest request)
        {
            var res = new GetPlanListResponses();

            try
            {
                res = await _itexService.GetDataOperatorPlanList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> DataRechargePayment(DataBillPaymentRequest request)
        {
            var res = new GetDataRechargeResponse();

            try
            {
                res = await _itexRechargeService.DataRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        //step1 internet
        [HttpPost]
        public async Task<JsonResult> GetWifiInternetOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _itexService.GetInternetOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        //step2 internet
        [HttpPost]
        public async Task<JsonResult> GetInternetBundles(GetInternetBundlesRequest request)
        {
            var res = new GetInternetBundlesResponse();

            try
            {
                res = await _itexService.GetInternetBundles(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        //step3 internet
        //[HttpPost]
        //public async Task<JsonResult> GetInternetValidation(GetInternetValidateRequest request)
        //{
        //    var res = new GetValidateInternetResponse();

        //    try
        //    {
        //        res = await _itexService.GetInternetValidation(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}

        [HttpPost]
        public async Task<ActionResult> GetTVOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _itexService.GetTvOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = res, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public async Task<ActionResult> MultichoiceBouquets(GetBouquetDataRequest request)
        {
            var res = new GetBouquestResponses();

            try
            {
                res = await _itexService.MultichoiceBouquets(request);
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = res, MaxJsonLength = int.MaxValue };
        }
        [HttpPost]
        public async Task<ActionResult> MultiChoiceRechargePayment(MultiChoicePurchaseBillPaymentRequest request)
        {
            var res = new GetMultiChoiceRechargeResponse();

            try
            {
                res = await _itexRechargeService.MultiChoiceRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = res, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public async Task<JsonResult> GetElectricityOperatorList(OperatorRequest request)
        {
            var res = new OperatorResponse();

            try
            {
                res = await _itexService.GetElectricityOperatorList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> AirtimePayment(VTUBillPaymentRequest request)
        {
            var res = new GetVTUResponse();

            try
            {
                res = await _itexRechargeService.AirtimePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        //[HttpPost]
        //public async Task<JsonResult> FilterOperatorByBillerCode(string BillerCode)
        //{
        //    var res = new ProductResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.FilterOperatorByBillerCode(BillerCode);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}

        //[HttpPost]
        //public async Task<JsonResult> BillPayment(AirtimeRechargeRequest request)
        //{
        //    var res = new AirtimeRechargeResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.AirtimeRecharge(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}

        //[HttpPost]
        //public async Task<JsonResult> TVRecharge(AirtimeRechargeRequest request)
        //{
        //    var res = new PayMasta.ViewModel.AirtimeVM.TVRechargeResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.TVRecharge(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}
        //[HttpPost]
        //public async Task<JsonResult> InternetRecharge(AirtimeRechargeRequest request)
        //{
        //    var res = new PayMasta.ViewModel.AirtimeVM.InternetRechargeResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.InternetRecharge(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}

        //[HttpPost]
        //public async Task<JsonResult> DataBundleRecharge(AirtimeRechargeRequest request)
        //{
        //    var res = new PayMasta.ViewModel.AirtimeVM.DataBundleRechargeResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.DataBundleRecharge(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}
        //[HttpPost]
        //public async Task<JsonResult> ElectriCityBillRecharge(AirtimeRechargeRequest request)
        //{
        //    var res = new PayMasta.ViewModel.AirtimeVM.ElectriCityBillRechargeResponse();

        //    try
        //    {
        //        res = await _flutterWaveService.ElectriCityBillRecharge(request);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(res);
        //}

        [HttpPost]
        public async Task<JsonResult> InternetRechargePayment(InternetPurchaseBillPaymentRequest request)
        {
            var res = new GetInternetPurchaseRechargeResponse();

            try
            {
                res = await _itexRechargeService.InternetRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> StarlineRechargePayment(StarLinePurchaseBillPaymentRequest request)
        {
            var res = new GetStarLinePurchaseRechargeResponse();

            try
            {
                res = await _itexRechargeService.StarlineRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> ElectricityRechargePayment(ElectricityBillPaymentRequest request)
        {
            var res = new GetElectricityRechargeResponse();

            try
            {
                res = await _itexRechargeService.ElectricityRechargePayment(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetBankList(OperatorRequest request)
        {
            var res = new BankListResponse();

            try
            {
                res = await _itexService.GetBankList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> WalletToBankTransfer(WalletToBankPaymentRequest request)
        {
            var res = new GetWalletToBankResponse();

            try
            {
                res = await _itexRechargeService.WalletToBankTransfer(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> VerifyAccount(VerifyAccountRequest request)
        {
            var res = new GetVerifyAccountResponse();

            try
            {
                res = await _itexRechargeService.VerifyAccount(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }


        [HttpPost]
        public async Task<JsonResult> GetLatestBankList(OperatorRequest request)
        {
            var res = new LatestBankListResponse();

            try
            {
                res = await _itexService.GetLatestBankList(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
        [HttpPost]
        public async Task<JsonResult> WalletToWalletTransfer(WalletToBankPaymentRequest request)
        {
            var res = new GetWalletToBankResponse();

            try
            {
                res = await _itexRechargeService.WalletToWalletTransfer(request);
            }
            catch (Exception ex)
            {

            }

            return Json(res);
        }
    }
}