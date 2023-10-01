using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Repository.Employer.EwaRequests;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.EWAVM;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.EwaRequests
{
    public class EwaRequestsServices : IEwaRequestsServices
    {
        private readonly IEwaRequestsRepository _ewaRequestsRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private HSSFWorkbook _hssfWorkbook;
        public EwaRequestsServices()
        {
            _ewaRequestsRepository = new EwaRequestsRepository();
            _employeesRepository = new EmployeesRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<PayMasta.ViewModel.Employer.EWAVM.EmployeesReponse> GetEmployeeListbyEmployerGuid(GetEmployerListRequest request)
        {
            var res = new PayMasta.ViewModel.Employer.EWAVM.EmployeesReponse();

            try
            {
                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var employerList = await _ewaRequestsRepository.GetEmployeesListByEmployerId(result.Id, request.pageNumber, request.PageSize, request.SearchTest, request.Status, request.FromDate, request.ToDate);
                if (employerList.Count > 0)
                {
                    res.employeesListViewModel = employerList;
                    res.IsEwaApprovalAccess = employerList[0].IsEwaApprovalAccess;
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

        public async Task<UpdateEWAStatusResponse> UpdateAccessAmountRequestById(UpdateEWAStatusRequest request)
        {
            var res = new UpdateEWAStatusResponse();
            int result2 = 0;
            try
            {
                var id = Guid.Parse(request.UserGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var ids = request.AccessAmountIds.Split(',');
                foreach (var item in ids)
                {
                    var accessAmountId = Convert.ToInt32(item);
                    var accessAmountData = await _ewaRequestsRepository.GetAccessAmountRequestById(accessAmountId);
                    if (request.Status == (int)TransactionStatus.Completed)
                    {
                        accessAmountData.Status = (int)TransactionStatus.Completed;
                    }
                    else if (request.Status == (int)TransactionStatus.Rejected)
                    {
                        accessAmountData.Status = (int)TransactionStatus.Rejected;

                    }
                    else if (request.Status == (int)TransactionStatus.Hold)
                    {
                        accessAmountData.Status = (int)TransactionStatus.Hold;
                    }
                    accessAmountData.UpdatedBy = result.Id;
                    accessAmountData.UpdatedAt = DateTime.UtcNow;
                    result2 = await _ewaRequestsRepository.UPdateAccessAmountRequestById(accessAmountData);
                }

                if (result2 > 0)
                {
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.Message = ResponseMessages.DATAUPDATED;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = ResponseMessages.REQUEST_SENT_FAILD;
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<MemoryStream> ExportUserListReport(GetEmployerListRequest request)
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

        private async Task GenerateData(GetEmployerListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _ewaRequestsRepository.DownloadCsvEmployeesListByEmployerId(result.Id, request.Status, request.FromDate, request.ToDate);

                ISheet sheet1 = _hssfWorkbook.CreateSheet("PayMastaTransactionLog");
                sheet1.SetColumnWidth(0, 1500);
                sheet1.SetColumnWidth(1, 4000);
                sheet1.SetColumnWidth(2, 4000);
                sheet1.SetColumnWidth(3, 8000);
                sheet1.SetColumnWidth(4, 8000);
                sheet1.SetColumnWidth(5, 8000);
                sheet1.SetColumnWidth(6, 4000);
                sheet1.SetColumnWidth(7, 8000);
                sheet1.SetColumnWidth(8, 8000);
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
                C04.SetCellValue("Country");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Email Id");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Amount");
                C06.CellStyle = style1;

                var C07 = R0.CreateCell(7);
                C07.SetCellValue("Date");
                C07.CellStyle = style1;

                var C08 = R0.CreateCell(8);
                C08.SetCellValue("Status");
                C08.CellStyle = style1;

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
                    c3.SetCellValue(item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.CountryCode);

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.Email.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.AccessAmount.ToString());

                    var c7 = row.CreateCell(7);
                    c7.SetCellValue(item.CreatedAt.ToString());

                    var c8 = row.CreateCell(8);
                    c8.SetCellValue(item.Status);
                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
