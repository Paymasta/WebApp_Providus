using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Repository.Account;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Repository.Employer.EmployeeTransaction;
using PayMasta.Utilities;
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

namespace PayMasta.Service.Employer.EmployeeTransaction
{
    public class EmployeeTransactionService : IEmployeeTransactionService
    {
        private readonly IEmployeeTransactionRepository _employeeTransactionRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAccountRepository _accountRepository;
        private HSSFWorkbook _hssfWorkbook;
        public EmployeeTransactionService()
        {
            _employeeTransactionRepository = new EmployeeTransactionRepository();
            _employeesRepository = new EmployeesRepository();
            _accountRepository = new AccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<EmployeesListForTransactionsReponse> GetEmployeesList(GetEmployeesListForTransactionRequest request)
        {
            var res = new EmployeesListForTransactionsReponse();

            try
            {
                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employeesList = await _employeeTransactionRepository.GetEmployeesList(result.Id, request.pageNumber, request.PageSize, request.month, request.FromDate, request.ToDate, request.SearchTest);
                if (employeesList.Count > 0)
                {
                    res.employeesListForTransactions = employeesList;
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
        public async Task<EmployeesListForTransactionsReponse> GetEmployeesListWeb(GetEmployeesListForTransactionRequest request)
        {
            var res = new EmployeesListForTransactionsReponse();

            try
            {
                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employeesList = await _employeeTransactionRepository.GetEmployeesListWeb(result.Id, request.pageNumber, request.PageSize, request.month, request.FromDate, request.ToDate, request.SearchTest);
                if (employeesList.Count > 0)
                {
                    res.employeesListForTransactions = employeesList;
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

        public async Task<EmployeesWithdrawlsResponse> GetEmployeesWithdrwals(EmployeesWithdrawlsRequest request)
        {
            var res = new EmployeesWithdrawlsResponse();
            //var result = new EmployeesWithdrawlsResponse();
            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                if (result != null)
                {
                    var employeesWithdrawlsList = await _employeeTransactionRepository.GetEmployeesWithdrwalsForEmployer(result.Id, request.Month, request.PageSize, request.PageNumber);
                    if (employeesWithdrawlsList.Count > 0)
                    {
                        res.employeesWithdrawls = employeesWithdrawlsList;
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
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<EmployeeEwaDetailReponse> GetEmployeesEwaRequestDetail(EmployeesEWAWithdrawlsRequest request)
        {
            var res = new EmployeeEwaDetailReponse();

            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var employerEWADetail = await _employeeTransactionRepository.GetEmployeesEwaRequestDetail(userData.Id);
                if (employerEWADetail != null)
                {
                    res.employeeEwaDetail = employerEWADetail;
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

        public async Task<MemoryStream> ExportUserListReport(DownloadLogReportRequest request)
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

        private async Task GenerateData(DownloadLogReportRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _employeeTransactionRepository.GetEmployeesListForCsv(result.Id, request.Month, request.DateFrom, request.DateTo);

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
                C01.SetCellValue("Created Date");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Name");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Email Id");
                C03.CellStyle = style1;

                var C04 = R0.CreateCell(4);
                C04.SetCellValue("MobileNo");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Country");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Employer Name");
                C06.CellStyle = style1;

                var C07 = R0.CreateCell(7);
                C07.SetCellValue("StaffId");
                C07.CellStyle = style1;

                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.CreatedAt);

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.FirstName);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.Email.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.PhoneNumber);

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.CountryCode.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.EmployerName.ToString());

                    var c7 = row.CreateCell(7);
                    c7.SetCellValue(item.StaffId);

                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<MemoryStream> ExportWithdrawlsListReport(EmployeesWithdrawlsRequest request)
        {
            InitializeWorkbook();
            await GenerateDataForWithDrawls(request);
            return GetExcelStream();
        }

        private async Task GenerateDataForWithDrawls(EmployeesWithdrawlsRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                var response = await _employeeTransactionRepository.GetEmployeesWithdrwalsForDownload(result.Id, request.Month);

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
                C01.SetCellValue("Access Amount");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Status");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Date");
                C03.CellStyle = style1;


                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.AccessAmount.ToString());

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.Status);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CreatedAt);



                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<MemoryStream> ExportWithdrawlsListReportForPayCycle(EmployeesWithdrawlsRequest request)
        {
            InitializeWorkbook();
            await GenerateDataForWithDrawlsForPayCycle(request);
            return GetExcelStream();
        }

        private async Task GenerateDataForWithDrawlsForPayCycle(EmployeesWithdrawlsRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _employeeTransactionRepository.GetEmployeesWithdrwalsForPayCycle(result.Id);

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
                C01.SetCellValue("Access Amount");
                C01.CellStyle = style1;

                var C02 = R0.CreateCell(2);
                C02.SetCellValue("Status");
                C02.CellStyle = style1;

                var C03 = R0.CreateCell(3);
                C03.SetCellValue("Date");
                C03.CellStyle = style1;


                int i = 1;
                foreach (var item in response)
                {

                    IRow row = sheet1.CreateRow(i);

                    var C0 = row.CreateCell(0);
                    C0.SetCellValue(item.RowNumber);

                    var C1 = row.CreateCell(1);
                    C1.SetCellValue(item.AccessAmount.ToString());

                    var C2 = row.CreateCell(2);
                    C2.SetCellValue(item.Status);

                    var c3 = row.CreateCell(3);
                    c3.SetCellValue(item.CreatedAt);



                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<EmployeesWithdrawls>> IsDataExists(EmployeesWithdrawlsRequest request)
        {
            var id = Guid.Parse(request.UserGuid.ToString());
            var result = await _employeesRepository.GetEmployerDetailByGuid(id);
            var response = await _employeeTransactionRepository.GetEmployeesWithdrwalsForPayCycle(result.Id);

            return response;
        }


        public async Task<EmployeesWithdrawlsResponseForApp> GetEmployeesWithdrwalsForApp(EmployeesWithdrawlsRequest request)
        {
            var res = new EmployeesWithdrawlsResponseForApp();
            //var result = new EmployeesWithdrawlsResponse();
            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _accountRepository.GetUserByGuid(id);
                if (result != null)
                {
                    var employeesWithdrawlsList = await _employeeTransactionRepository.GetEmployeesWithdrwalsForApp(result.Id, request.Month, request.PageSize, request.PageNumber);
                    if (employeesWithdrawlsList.Count > 0)
                    {
                        res.employeesWithdrawls = employeesWithdrawlsList;
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
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = ResponseMessages.INVALID_USER_TYPE;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
