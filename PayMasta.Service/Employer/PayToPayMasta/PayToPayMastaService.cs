using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.DBEntity.AccessAmountRequest;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Repository.Employer.PayToPayMasta;
using PayMasta.Service.BankTransfer;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.ViewModel.BankTransferVM;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.Employer.EWAVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.PayToPayMasta
{
    public class PayToPayMastaService : IPayToPayMastaService
    {
        private readonly IPayToPayMastaRepository _payToPayMastaRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IBankTransferService _bankTransferService;
        private readonly IEmailUtils _emailUtils;
        private HSSFWorkbook _hssfWorkbook;
        public PayToPayMastaService()
        {
            _payToPayMastaRepository = new PayToPayMastaRepository();
            _employeesRepository = new EmployeesRepository();
            _bankTransferService = new BankTransferService();
            _emailUtils = new EmailUtils();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<AccessAmountViewModelResponse> GetEmployeesEwaRequestList(PayToPayMastaRequest request)
        {
            var res = new AccessAmountViewModelResponse();

            try
            {
                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employerEWAList = await _payToPayMastaRepository.GetEmployeesEwaRequestList(request.pageNumber, request.PageSize, request.Status, request.FromDate, request.ToDate, request.SearchTest, result.Id);
                if (employerEWAList.Count > 0)
                {
                    var days = result.EndDate - 3;

                    int d = (int)System.DateTime.Now.Day;

                    if (d > days)
                    {
                        res.isInvoiceDownload = true;
                    }
                    else
                    {
                        res.isInvoiceDownload = false;
                    }

                    res.accessAmountViewModels = employerEWAList;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<PayableAmountResponse> GetPayAbleAmount(Guid userGuid)
        {
            var res = new PayableAmountResponse();

            try
            {
                var id = Guid.Parse(userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employerEWAList = await _payToPayMastaRepository.GetPayAbleAmount(result.Id);
                if (employerEWAList != null)
                {
                    res.payableAmount = employerEWAList;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<MemoryStream> ExportUserListReport(PayToPayMastaRequest request)
        {
            InitializeWorkbook();
            await GenerateData(request);
            return GetExcelStream();
        }
        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            _hssfWorkbook.Write(file);
            return file;
        }
        void InitializeWorkbook()
        {
            _hssfWorkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            _hssfWorkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            _hssfWorkbook.SummaryInformation = si;
        }

        private async Task GenerateData(PayToPayMastaRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _payToPayMastaRepository.GetEmployeesEwaRequestListForCsv(request.Status, request.FromDate, request.ToDate, request.SearchTest, result.Id);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaTransactionLog");
                sheet1.SetColumnWidth(0, 1500);
                sheet1.SetColumnWidth(1, 4000);
                sheet1.SetColumnWidth(2, 4000);
                sheet1.SetColumnWidth(3, 8000);
                sheet1.SetColumnWidth(4, 8000);
                sheet1.SetColumnWidth(5, 8000);
                sheet1.SetColumnWidth(6, 4000);
                sheet1.SetColumnWidth(7, 8000);
                //----------Create Header-----------------
                var R0 = sheet1.CreateRow(0);

                var C00 = R0.CreateCell(0);
                C00.SetCellValue("S.No");
                C00.CellStyle = style1;

                var C01 = R0.CreateCell(1);
                C01.SetCellValue("Name");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("StaffId");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("MobileNo");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("Email Id");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Access Amount");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Date");
                C06.CellStyle = style1;

                var C07 = R0.CreateCell(7);
                C07.SetCellValue("Status");
                C07.CellStyle = style1;

                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.FirstName + " " + item.LastName);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.StaffId);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CountryCode.ToString() + "-" + item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.Email);

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.AccessAmount.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.CreatedAt.ToString());

                    var c7 = row.CreateCell(7);
                    c7.SetCellValue(item.IsPaidToPayMasta);

                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<ProvidusFundResponse> PayToPayMastaAccount(ProvidusFundTransferRequest request)
        {
            var res = new ProvidusFundResponse();

            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employerEWAList = await _payToPayMastaRepository.GetPayAbleAmount(result.Id);
                if (employerEWAList != null)
                {
                    var bankResult = await _bankTransferService.PayToPaymastaFundTransfer(request.DebitAccount, employerEWAList.TotalAmount.ToString());
                    if (bankResult.RstKey == 1)
                    {
                        await _payToPayMastaRepository.UpdatePayToPayMastaFlag(result.Id);
                        res.transferResponse = bankResult.transferResponse;
                        res.Status = true;
                        res.RstKey = 1;
                        res.Message = ResponseMessages.SUCCESS;
                    }
                    else
                    {
                        res.Status = false;
                        res.RstKey = 2;
                        res.Message = ResponseMessages.AGGREGATOR_FAILED_ERROR;
                    }
                }
                else
                {
                    res.Status = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.REQUESTDATA_NOT_EXIST;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<string> Invoice(Guid userGuid)
        {
            string file = string.Empty;

            try
            {
                var id = Guid.Parse(userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employerEWAList = await _payToPayMastaRepository.GetPayAbleAmount(result.Id);
                //EmailUtils email = new EmailUtils();
                var fileName = AppSetting.Invoice;
                var body = _emailUtils.ReadEmailformats(fileName);
                body = body.Replace("$$EmployerName$$", result.OrganisationName);
                body = body.Replace("$$Address$$", result.Address);
                body = body.Replace("$$PhoneNumber$$", result.PhoneNumber);
                body = body.Replace("$$EmployerId$$", result.Id.ToString());
                body = body.Replace("$$Date$$", DateTime.UtcNow.ToString("dd-MM-yyyy"));
                body = body.Replace("$$TotalAmount$$", employerEWAList.TotalAmount.ToString());
                body = body.Replace("$$Fee$$", employerEWAList.TotalFee.ToString());
                body = body.Replace("$$TotalTransactions$$", employerEWAList.TotalUser.ToString());

                file = body;

            }
            catch (Exception ex)
            {

            }
            return file;
        }

        private string GeneratePdfContent(GetEmployerDetailResponse orderData,PayableAmount payableAmount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }
    }
}
