using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PayMasta.Service.BankTransfer;
using PayMasta.Service.Employer.PayToPayMasta;
using PayMasta.ViewModel.BankTransferVM;
using PayMasta.ViewModel.Employer.EWAVM;
using PayMasta.Web.Models;
using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PayMasta.Web.Controllers.Employer
{
    [CustomAuthorize(Roles = "Employer")]
    [SessionExpireFilter]
    public class PayToPayMastaController : MyBaseController
    {
        private IPayToPayMastaService _payToPayMastaService;
        private IBankTransferService _bankTransferService;
        public PayToPayMastaController(IPayToPayMastaService payToPayMastaService, IBankTransferService bankTransferService)
        {
            _payToPayMastaService = payToPayMastaService;
            _bankTransferService = bankTransferService;
        }
        // GET: PayToPayMasta
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetEmployeesEWARequestList(PayToPayMastaRequest request)
        {
            var result = new AccessAmountViewModelResponse();

            try
            {
                result = await _payToPayMastaService.GetEmployeesEwaRequestList(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetPayAbleAmount(Guid request)
        {
            var result = new PayableAmountResponse();

            try
            {
                result = await _payToPayMastaService.GetPayAbleAmount(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ExportCsvReport(PayToPayMastaRequest request)
        {
            // int langId = AppUtils.GetLangId(Request);
            string filename = "PayMastaLog";
            MemoryStream memoryStream = null;
            FileContentResult robj;
            memoryStream = await _payToPayMastaService.ExportUserListReport(request);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(memoryStream.ToArray())
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue
                      ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition =
                   new ContentDispositionHeaderValue("attachment")
                   {
                       FileName = $"{filename}_{DateTime.Now.Ticks.ToString()}.xls"
                   };
            //response.Content.Headers.ContentLength = stream.Length;
            memoryStream.WriteTo(memoryStream);
            memoryStream.Close();
            robj = File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "TeamMembers.xlsx");
            return Json(robj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetBankList(int id)
        {
            var res = new GetBankListResponse();

            res = await _bankTransferService.GetBanks();
            return Json(res);
        }


        [HttpPost]
        public async Task<JsonResult> PayToPaymasta(ProvidusFundTransferRequest request)
        {
            var result = new ProvidusFundResponse();

            try
            {
                result = await _payToPayMastaService.PayToPayMastaAccount(request);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }

        //[HttpPost]
        //public async Task<JsonResult> InvoiceGenerate(Guid request)
        //{
        //    // int langId = AppUtils.GetLangId(Request);
        //    string filename = "PayMastaLog";
        //    var data = await _payToPayMastaService.Invoice(request);

        //    var bytes = GeneratePDF("<h1>Test</h1>");
        //    return Json(bytes.ToArray(), JsonRequestBehavior.AllowGet);
        //}


        public async Task<FileResult> InvoiceGenerate(Guid request)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var data = await _payToPayMastaService.Invoice(request);
                StringReader sr = new StringReader(data);
                Document pdfDoc = new Document(iTextSharp.text.PageSize.A4, 0f, 0f, 0f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "invoice_" + DateTime.UtcNow + ".pdf");
            }
        }
    }
}