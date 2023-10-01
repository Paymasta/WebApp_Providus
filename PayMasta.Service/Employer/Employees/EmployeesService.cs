using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PayMasta.Repository.Account;
using PayMasta.Repository.Employer.Employees;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.Utilities.Common;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.LogUtils;
using PayMasta.Utilities.PushNotification;
using PayMasta.ViewModel;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Employer.EmployeesVM;
using PayMasta.ViewModel.VerifyNinVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Employer.Employees
{
    //M2
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAccountRepository _accountRepository;
        private ILogUtils _logUtils;
        private HSSFWorkbook _hssfWorkbook;
        private readonly IPushNotification _pushNotification;
        private readonly IEmailUtils _emailUtils;
        private readonly IThirdParty _thirdParty;
        public EmployeesService()
        {
            _employeesRepository = new EmployeesRepository();
            _accountRepository = new AccountRepository();
            _pushNotification = new PushNotification();
            _logUtils = new LogUtils();
            _emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<EmployeesReponse> GetEmployeesByEmployerGuid(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                //try
                //{
                //    var sr = JsonConvert.SerializeObject(request);
                //    _logUtils.WriteTextToFile(sr);
                //}
                //catch (Exception ex)
                //{

                //}

                var id = Guid.Parse(request.userGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(id);
                if (result != null)
                {
                    var employeesList = await _employeesRepository.GetEmployeesListByEmployerId(result.Id, request.pageNumber, request.PageSize, request.SearchTest, request.Status, request.FromDate, request.ToDate);
                    if (employeesList.Count > 0)
                    {
                        res.employeesListViewModel = employeesList;
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

        public async Task<EmployeesReponse> GetEmployeesByEmployerGuidError(GetEmployeesListRequest request)
        {
            var res = new EmployeesReponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                //try
                //{
                //    var sr = JsonConvert.SerializeObject(request);
                //    _logUtils.WriteTextToFile(sr);
                //}
                //catch (Exception ex)
                //{

                //}

                var id = Guid.Parse(request.userGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(id);
                if (result != null)
                {
                    var employeesList = await _employeesRepository.GetEmployeesListByEmployerIdError(result.Id, request.pageNumber, request.PageSize, request.SearchTest, request.Status, request.FromDate, request.ToDate);
                    if (employeesList.Count > 0)
                    {
                        res.employeesListViewModel = employeesList;
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
        public async Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployees(BlockUnBlockEmployeeRequest request)
        {
            var res = new BlockUnBlockEmployeeResponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.EmployeerUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employeerId);
                var userData = await _accountRepository.GetUserByGuid(employeeId);
                if (result != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (userData.Status == 1)
                        {
                            userData.Status = 0;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_BLOCKED;
                        }
                        else
                        {
                            userData.Status = 1;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_UNBLOCKED;
                        }
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                    }
                    if (request.DeleteOrBlock == 2)
                    {
                        userData.Status = 0;
                        userData.IsActive = false;
                        userData.IsDeleted = true;
                        userData.UpdatedAt = DateTime.UtcNow;
                        userData.UpdatedBy = result.Id;
                        res.Message = AdminResponseMessages.USER_DELETED;
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                    }
                    if (await _accountRepository.ChangeUserStatus(userData) > 0)
                    {

                        res.IsSuccess = true;
                        res.RstKey = 1;

                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<BlockUnBlockEmployeeResponse> DeleteEmployees(DeleteEmployeeRequest request)
        {
            var res = new BlockUnBlockEmployeeResponse();
            // var result = new GetEmployerDetailResponse();
            try
            {
                // var employeerId = Guid.Parse(request.EmployeerUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                // result = await _employeesRepository.GetEmployerDetailByGuid(employeerId);
                var userData = await _accountRepository.GetUserByGuid(employeeId);
                var bankData = await _employeesRepository.GetBankDetailByUserId(userData.Id);
                if (userData != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (userData.Status == 1)
                        {
                            userData.Status = 0;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = userData.Id;
                            res.Message = AdminResponseMessages.USER_BLOCKED;
                        }
                        else
                        {
                            userData.Status = 1;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = userData.Id;
                            res.Message = AdminResponseMessages.USER_UNBLOCKED;
                        }
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);


                    }
                    if (request.DeleteOrBlock == 2)
                    {
                        userData.Status = 0;
                        userData.IsActive = false;
                        userData.IsDeleted = true;
                        userData.UpdatedAt = DateTime.UtcNow;
                        userData.UpdatedBy = userData.Id;
                        res.Message = AdminResponseMessages.USER_DELETED;
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);

                        if (bankData.Count > 0)
                        {
                            bankData.ForEach(async x =>
                            {
                                x.IsActive = false;
                                x.IsDeleted = true;
                                x.UpdatedAt = DateTime.UtcNow;

                                await _employeesRepository.DeleteBankByBankDetailId(x);
                            });
                        }
                    }
                    if (await _accountRepository.ChangeUserStatus(userData) > 0)
                    {

                        res.IsSuccess = true;
                        res.RstKey = 1;

                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        private async Task<int> UpdateSession(LogoutRequest request, string Title, string Message)
        {
            var result = 0;
            long userId = _accountRepository.GetUserIdByGuid(request.UserGuid);
            if (userId > 0)
            {
                var session = await _accountRepository.GetSessionByUserId(userId);
                if (session != null)
                {
                    session.IsActive = false;
                    session.IsDeleted = true;
                    session.UpdatedAt = DateTime.UtcNow;
                    result = await _accountRepository.UpdateSession(session);
                    try
                    {
                        var pushModel = new PushModel
                        {
                            DeviceToken = session.DeviceToken,
                            Title = Title,
                            Message = Message
                        };
                        _pushNotification.SendPush(pushModel);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }
        public async Task<ViewEmployeesProfileResponse> ViewEmployeeProfile(ViewEmployeesProfileRequest request)
        {
            var res = new ViewEmployeesProfileResponse();

            try
            {

                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());

                var userData = await _accountRepository.GetUserByGuid(employeeId);
                if (userData != null)
                {
                    res.FirstName = userData.FirstName;
                    res.LastName = userData.LastName;
                    res.MiddleName = userData.MiddleName != "" ? userData.MiddleName : "NA";
                    res.Email = userData.Email;
                    res.CountryCode = userData.CountryCode;
                    res.PhoneNumber = userData.PhoneNumber;
                    res.StaffId = userData.StaffId != null ? userData.StaffId : "NA";
                    res.DateOfBirth = userData.DateOfBirth.ToString("dd/MM/yyyy").Replace('-', '/'); ;
                    res.Gender = userData.Gender;
                    res.NetPay = userData.NetPayMonthly;
                    res.GrossPay = userData.GrossPayMonthly;
                    res.ProfileImage = userData.ProfileImage;
                    res.IsSuccess = true;
                    res.RstKey = 1;
                    res.UserGuid = userData.Guid;
                    res.IsUserVerified = userData.IsverifiedByEmployer;
                    res.Message = AdminResponseMessages.DATA_FOUND;
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 2;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<UpdateEmployeeNetAndGrossPayResponse> UpdateNetAndGrossPay(UpdateEmployeeNetAndGrossPayRequest request)
        {
            var res = new UpdateEmployeeNetAndGrossPayResponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.EmployeerUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employeerId);
                var userData = await _accountRepository.GetUserByGuid(employeeId);
                if (result != null && userData != null)
                {
                    userData.NetPayMonthly = Convert.ToDecimal(request.NetPay);
                    userData.GrossPayMonthly = Convert.ToDecimal(request.GrossPay);
                    userData.UpdatedBy = result.Id;
                    userData.UpdatedAt = DateTime.UtcNow;
                    if (await _employeesRepository.UpdateUserNetAndGrossPay(userData) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = AdminResponseMessages.DATA_SAVED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<UpdateEmployeeNetAndGrossPayResponse> ApproveUserProfile(ApproveUserProfileRequest request)
        {
            var res = new UpdateEmployeeNetAndGrossPayResponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.EmployerGuid.ToString());
                var employeeId = Guid.Parse(request.UserGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employeerId);
                var userData = await _accountRepository.GetUserByGuid(employeeId);
                if (result != null && userData != null)
                {
                    userData.IsverifiedByEmployer = true;


                    if (await _employeesRepository.ApproveUserProfileByEmployer(userData) > 0)
                    {
                        res.IsSuccess = true;
                        res.RstKey = 1;
                        res.Message = AdminResponseMessages.USER_PROFILE_APPROVED;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<MemoryStream> ExportUserListReport(GetEmployeesListRequest request)
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

        private async Task GenerateData(GetEmployeesListRequest request)
        {

            try
            {
                ICellStyle style1 = _hssfWorkbook.CreateCellStyle();
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style1.FillPattern = FillPattern.SolidForeground;

                var id = Guid.Parse(request.userGuid.ToString());
                var result = await _employeesRepository.GetEmployerDetailByGuid(id);
                var response = await _employeesRepository.GetEmployeesListByEmployerIdCSV(result.Id, request.Status, request.FromDate, request.ToDate);

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
                C04.SetCellValue("Country");
                C04.CellStyle = style1;

                var C05 = R0.CreateCell(5);
                C05.SetCellValue("Email Id");
                C05.CellStyle = style1;

                var C06 = R0.CreateCell(6);
                C06.SetCellValue("Status");
                C06.CellStyle = style1;

                var C07 = R0.CreateCell(7);
                C07.SetCellValue("Date");
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
                    c3.SetCellValue(item.PhoneNumber.ToString());

                    var c4 = row.CreateCell(4);
                    c4.SetCellValue(item.CountryCode);

                    var c5 = row.CreateCell(5);
                    c5.SetCellValue(item.Email.ToString());

                    var c6 = row.CreateCell(6);
                    c6.SetCellValue(item.Status.ToString());

                    var c7 = row.CreateCell(7);
                    c7.SetCellValue(item.CreatedAt);

                    i++;
                }

            }
            catch (Exception ex)
            {

            }
        }


        public async Task<int> BulkUploadUsersCSV1(Guid employerGuid, string filePath)
        {
            int result = 0;
            bool IsPhoneNumberExist = false;
            bool IsemailExist = false;
            bool IsstaffIdExist = false;
            bool NinExist = false;
            var adminKeyPair = AES256.AdminKeyPair;
            //  request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            try
            {
                var resultEmployer = await _employeesRepository.GetEmployerDetailByGuid(employerGuid);

                if (resultEmployer == null)
                {
                    return result;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("FirstName");//0
                dt.Columns.Add("LastName");//1
                dt.Columns.Add("MiddleName");//2
                dt.Columns.Add("NinNo");//3
                dt.Columns.Add("DateOfBirth");//4
                dt.Columns.Add("Email");//5
                dt.Columns.Add("Password");//6
                dt.Columns.Add("CountryCode");//7
                dt.Columns.Add("PhoneNumber");//8
                dt.Columns.Add("Gender");//9
                dt.Columns.Add("State");//10
                dt.Columns.Add("City");//11
                dt.Columns.Add("Address");//12
                dt.Columns.Add("PostalCode");//13
                dt.Columns.Add("StaffId");//14
                dt.Columns.Add("NetPayMonthly");//15
                dt.Columns.Add("GrossPayMonthly");//16
                dt.Columns.Add("CountryName");//17

                XSSFWorkbook xssfwb;
                IWorkbook _workbook;
                using (FileStream file1 = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    xssfwb = new XSSFWorkbook(file1);
                    //_workbook = new HSSFWorkbook(file1);
                }

                ISheet sheet = xssfwb.GetSheetAt(0);
                if (sheet.LastRowNum < 1) //------Validation for Empty Sheet
                {
                    return 7;
                }
                var sheetHeader = new List<string>();
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        var row = sheet.GetRow(i);
                        if (row != null && row.LastCellNum > 1) //null is when the row only contains empty cells 
                        {

                            DataRow r = dt.NewRow();

                            for (int c = 0; c < row.LastCellNum; c++)
                            {
                                try
                                {
                                    if (row.GetCell(c) != null) //null is when the row only contains empty cells 
                                    {
                                        if (i == 0)
                                        {
                                            string hdcol = row.GetCell(c).StringCellValue;
                                            sheetHeader.Add(hdcol);
                                        }

                                        string col = row.GetCell(c).ToString();
                                        r[c] = col;

                                        //------Numeric field Validation----
                                        //if (i > 0 && (c == 8 || c == 14 || c == 18 || c == 20))
                                        //{
                                        //    try
                                        //    {
                                        //        var a = row.GetCell(c).NumericCellValue;
                                        //    }
                                        //    catch
                                        //    {
                                        //        return 6;
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        return 5;
                                    }
                                }
                                catch
                                {
                                }
                            }

                            //-------Check  header-------
                            if (i == 0)
                            {

                                string h = string.Join(",", sheetHeader);
                                if (!string.Equals(h, "FirstName,LastName,MiddleName,NinNo,DateOfBirth,Email,Password,CountryCode,PhoneNumber,Gender,State,City,Address,PostalCode,StaffId,NetPayMonthly,GrossPayMonthly,CountryName", StringComparison.OrdinalIgnoreCase))
                                {
                                    return 3;
                                }
                            }

                            dt.Rows.Add(r);
                        }
                    }
                    catch { }
                }

                dt.Rows.Remove(dt.Rows[0]);


                //------Check duplicate Email-------
                var duplicatesEmail = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("Email"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate Phone-------
                var duplicatesPhone = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("PhoneNumber"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate StaffId-------
                var duplicatesStaffId = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("StaffId"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate StaffId-------
                var duplicatesNIN = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("NinNo"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();

                if (duplicatesEmail.Count > 0 || duplicatesPhone.Count > 0)
                {
                    return 8;
                }
                if (duplicatesStaffId.Count > 0)
                {
                    return 9;
                }
                if (duplicatesNIN.Count > 0)
                {
                    return 11;
                }

                foreach (DataRow row in dt.Rows)
                {
                    string Email = row["Email"].ToString();
                    string PhoneNumber = row["PhoneNumber"].ToString();
                    string staffId = row["StaffId"].ToString();
                    string ninNumber = row["NinNo"].ToString();
                    IsPhoneNumberExist = _accountRepository.IsPhoneNumberExist(PhoneNumber);
                    IsemailExist = _accountRepository.IsEmailExist(Email);
                    IsstaffIdExist = _accountRepository.IsStaffIdExist(staffId);
                    NinExist = _accountRepository.IsNinExist(ninNumber);

                    var test = AES256.Encrypt(adminKeyPair.PrivateKey, row["Password"].ToString());


                    if (IsPhoneNumberExist == false && IsemailExist == false && IsstaffIdExist == false)
                    {
                        try
                        {
                            string iOSApp = "https://apps.apple.com/us/app/paymasta/id1635917623";
                            string androidApp = "https://play.google.com/store/apps/details?id=com.paymasta.paymasta";
                            var fileName = AppSetting.LoginCreds;
                            var body = _emailUtils.ReadEmailformats(fileName);
                            body = body.Replace("$$Name$$", row["FirstName"].ToString() + " " + row["LastName"].ToString());
                            body = body.Replace("$$Email$$", row["Email"].ToString());
                            body = body.Replace("$$PhoneNumber$$", row["PhoneNumber"].ToString());
                            body = body.Replace("$$Company$$", resultEmployer.OrganisationName);
                            body = body.Replace("$$Password$$", row["Password"].ToString());
                            body = body.Replace("$$AndroidApp$$", androidApp);
                            body = body.Replace("$$IosApp$$", iOSApp);
                            body = body.Replace("$$webApp$$", "https://paymasta.co");
                            var emailModel = new EmailModel
                            {
                                Body = body,
                                TO = row["Email"].ToString(),
                                Subject = "Your account has been created on paymasta"
                            };
                            await _emailUtils.SendEmailBySendGrid(emailModel);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    row["Password"] = (AES256.Encrypt(adminKeyPair.PrivateKey, row["Password"].ToString()));
                    //#region qoreid code
                    //var qoreidTokenResponse = await AuthTokenForQoreId();
                    //if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken))
                    //{
                    //    var url = AppSetting.QoreIdBvnNubanUrl;
                    //    var req = new QoreIdBvnNubanRequest
                    //    {
                    //        accountNumber = row["FirstName"].ToString(),
                    //        bankCode = request.BankCode,
                    //        firstname = row["FirstName"].ToString(),
                    //        lastname = row["LastName"].ToString(),
                    //    };
                    //    var jsonReq = JsonConvert.SerializeObject(req);
                    //    var res = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                    //    var JsonRes = JsonConvert.DeserializeObject<QoreIdBvnNubanResponse>(res);
                    //    if (JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                    //    {

                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    //#endregion



                }
                if (IsPhoneNumberExist == false && IsemailExist == false && IsstaffIdExist == false)
                {
                    if (!NinExist)
                    {

                        result = await _employeesRepository.BulkUploadUsersList(dt, resultEmployer.OrganisationName, resultEmployer.Id);
                    }
                    else
                    {
                        return 12;
                    }
                }
                else
                {
                    return 10;
                }

            }
            catch (Exception ex)
            {
                _logUtils.WriteTextToFile(ex.StackTrace);
                // return 10;
            }

            return result;
        }

        private async Task<QoreIdAuthResponse> AuthTokenForQoreId()
        {

            var result = new QoreIdAuthResponse();
            try
            {
                var url = AppSetting.QoreIdAuthTokenUrl;
                string authToken = string.Empty;
                var req = new QoreIdAuthRequest
                {
                    clientId = AppSetting.QoreIdClientId,
                    secret = AppSetting.QoreIdSecretKey,
                };
                var jsonReq = JsonConvert.SerializeObject(req);
                var res = await _thirdParty.QoreIdAuthToken(jsonReq, url);
                var obj = JsonConvert.DeserializeObject<QoreIdAuthResponse>(res);
                if (obj.accessToken != null)
                {
                    result.accessToken = obj.accessToken;
                    result.expiresIn = obj.expiresIn;
                    result.tokenType = obj.tokenType;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<BulkUploadRecords> BulkUploadUsersCSV(Guid employerGuid, string filePath)
        {
            var res = new BulkUploadRecords();
            int result = 0;
            int resultError = 0;
            bool IsPhoneNumberExist = false;
            bool IsemailExist = false;
            bool IsstaffIdExist = false;
            bool NinExist = false;
            var adminKeyPair = AES256.AdminKeyPair;
            //  request.Password = AES256.Encrypt(adminKeyPair.PrivateKey, request.Password);
            try
            {
                var resultEmployer = await _employeesRepository.GetEmployerDetailByGuid(employerGuid);

                if (resultEmployer == null)
                {
                    res.RstKey = 0;
                    return res;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("FirstName");//0
                dt.Columns.Add("LastName");//1
                dt.Columns.Add("BankName");//8
                dt.Columns.Add("BankCode");//2
                dt.Columns.Add("NubanAccountNumber");//3
                dt.Columns.Add("Email");//5
                dt.Columns.Add("Password");//6
                dt.Columns.Add("PhoneNumber");//8
                dt.Columns.Add("StaffId");//14
                dt.Columns.Add("NetPayMonthly");//15
                dt.Columns.Add("GrossPayMonthly");//16
                dt.Columns.Add("CountryCode");//16

                DataTable dtSuccess = new DataTable();
                dtSuccess.Columns.Add("FirstName");//0
                dtSuccess.Columns.Add("LastName");//1
                dtSuccess.Columns.Add("BankName");//8
                dtSuccess.Columns.Add("BankCode");//2
                dtSuccess.Columns.Add("NubanAccountNumber");//3
                dtSuccess.Columns.Add("Email");//5
                dtSuccess.Columns.Add("Password");//6
                dtSuccess.Columns.Add("PhoneNumber");//8
                dtSuccess.Columns.Add("StaffId");//14
                dtSuccess.Columns.Add("NetPayMonthly");//15
                dtSuccess.Columns.Add("GrossPayMonthly");//16
                dtSuccess.Columns.Add("CountryCode");//16

                DataTable errordt = new DataTable();
                errordt.Columns.Add("FirstName");//0
                errordt.Columns.Add("LastName");//1
                errordt.Columns.Add("BankName");//8
                errordt.Columns.Add("BankCode");//2
                errordt.Columns.Add("NubanAccountNumber");//3
                errordt.Columns.Add("Email");//5
                errordt.Columns.Add("Password");//6
                errordt.Columns.Add("PhoneNumber");//8
                errordt.Columns.Add("StaffId");//14
                errordt.Columns.Add("NetPayMonthly");//15
                errordt.Columns.Add("GrossPayMonthly");//16
                errordt.Columns.Add("CountryCode");//16
                XSSFWorkbook xssfwb;
                IWorkbook _workbook;
                using (FileStream file1 = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    xssfwb = new XSSFWorkbook(file1);
                    //_workbook = new HSSFWorkbook(file1);
                }

                ISheet sheet = xssfwb.GetSheetAt(0);
                if (sheet.LastRowNum < 1) //------Validation for Empty Sheet
                {
                    res.RstKey = 7;
                    return res;
                }
                var sheetHeader = new List<string>();
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        var row = sheet.GetRow(i);
                        if (row != null && row.LastCellNum > 1) //null is when the row only contains empty cells 
                        {

                            DataRow r = dt.NewRow();

                            for (int c = 0; c < row.LastCellNum; c++)
                            {
                                try
                                {
                                    if (row.GetCell(c) != null) //null is when the row only contains empty cells 
                                    {
                                        if (i == 0)
                                        {
                                            string hdcol = row.GetCell(c).StringCellValue;
                                            sheetHeader.Add(hdcol);
                                        }

                                        string col = row.GetCell(c).ToString();
                                        r[c] = col;

                                        //------Numeric field Validation----
                                        //if (i > 0 && (c == 8 || c == 14 || c == 18 || c == 20))
                                        //{
                                        //    try
                                        //    {
                                        //        var a = row.GetCell(c).NumericCellValue;
                                        //    }
                                        //    catch
                                        //    {
                                        //        return 6;
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        res.RstKey = 5;
                                        return res;
                                    }
                                }
                                catch
                                {
                                }
                            }

                            //-------Check  header-------
                            if (i == 0)
                            {

                                string h = string.Join(",", sheetHeader);
                                //,DateOfBirth,Gender,MiddleName
                                if (!string.Equals(h, "FirstName,LastName,BankName,BankCode,NubanAccountNumber,Email,Password,PhoneNumber,StaffId,NetPayMonthly,GrossPayMonthly,CountryCode", StringComparison.OrdinalIgnoreCase))
                                {
                                    res.RstKey = 3;
                                    return res;
                                }
                            }

                            dt.Rows.Add(r);
                        }
                    }
                    catch { }
                }

                dt.Rows.Remove(dt.Rows[0]);


                //------Check duplicate Email-------
                var duplicatesEmail = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("Email"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate Phone-------
                var duplicatesPhone = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("PhoneNumber"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate StaffId-------
                var duplicatesStaffId = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("StaffId"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();
                //------Check duplicate StaffId-------
                var duplicatesNubanAccountNumber = dt.AsEnumerable()
                       .Select(dr => dr.Field<string>("NubanAccountNumber"))
                       .GroupBy(x => x)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .ToList();

                if (duplicatesEmail.Count > 0 || duplicatesPhone.Count > 0)
                {
                    res.RstKey = 8;
                    return res;
                }
                if (duplicatesStaffId.Count > 0)
                {
                    res.RstKey = 9;
                    return res;
                }
                if (duplicatesNubanAccountNumber.Count > 0)
                {
                    res.RstKey = 11;
                    return res;
                }
                dtSuccess.Columns.Add("DateOfBirth");//16
                dtSuccess.Columns.Add("Gender");//16
                dtSuccess.Columns.Add("MiddleName");//16
                dtSuccess.Columns.Add("BVN");//16
                dtSuccess.Columns.Add("BankAccountHolderName");//16



                dt.Columns.Add("DateOfBirth");//16
                dt.Columns.Add("Gender");//16
                dt.Columns.Add("MiddleName");//16
                dt.Columns.Add("BVN");//16
                dt.Columns.Add("BankAccountHolderName");//16

                errordt.Columns.Add("DateOfBirth");//16
                errordt.Columns.Add("Gender");//16
                errordt.Columns.Add("MiddleName");//16
                errordt.Columns.Add("BVN");//16
                errordt.Columns.Add("BankAccountHolderName");//16
                //foreach (DataRow row in dt.Rows)
                //{
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow row = dt.Rows[i];
                    string Email = row["Email"].ToString();
                    string PhoneNumber = row["PhoneNumber"].ToString();
                    string customer = string.Empty;
                    string customerNumber = PhoneNumber.Substring(0, 1);
                    if (customerNumber != "0")
                    {
                        customer = "0" + PhoneNumber;
                    }
                    else
                    {
                        customer = PhoneNumber;
                    }

                    string staffId = row["StaffId"].ToString();
                    string NubanAccountNumber = row["NubanAccountNumber"].ToString();
                    IsPhoneNumberExist = _accountRepository.IsPhoneNumberExist(customer);
                    IsemailExist = _accountRepository.IsEmailExist(Email);
                    IsstaffIdExist = _accountRepository.IsStaffIdExist(staffId);
                    //NinExist = _accountRepository.IsNinExist(ninNumber);
                    NinExist = _accountRepository.IsAccountNumberExist(NubanAccountNumber);
                    var test = AES256.Encrypt(adminKeyPair.PrivateKey, row["Password"].ToString());


                    row["Password"] = (AES256.Encrypt(adminKeyPair.PrivateKey, row["Password"].ToString()));
                    row["CountryCode"] = "+234";
                    #region qoreid code
                    var qoreidTokenResponse = await AuthTokenForQoreId();
                    IsemailExist = _accountRepository.IsEmailExist(Email);
                    if (!string.IsNullOrWhiteSpace(qoreidTokenResponse.accessToken) && IsPhoneNumberExist == false && IsemailExist == false && IsstaffIdExist == false && NinExist == false)
                    {
                        var url = AppSetting.QoreIdBvnNubanUrl;
                        var req = new QoreIdBvnNubanRequest
                        {
                            accountNumber = row["NubanAccountNumber"].ToString(),
                            bankCode = row["BankCode"].ToString(),
                            firstname = row["FirstName"].ToString(),
                            lastname = row["LastName"].ToString(),
                        };
                        var jsonReq = JsonConvert.SerializeObject(req);
                        var JsonRes = new QoreIdBvnNubanResponse();
                        var JsonError = new errorQoreId();
                        var resNuban = await _thirdParty.NubanVerification(jsonReq, url, qoreidTokenResponse.accessToken);
                        try
                        {
                            JsonRes = JsonConvert.DeserializeObject<QoreIdBvnNubanResponse>(resNuban);
                        }
                        catch (Exception ex)
                        {
                            JsonError = JsonConvert.DeserializeObject<errorQoreId>(resNuban);
                        }

                        if (JsonError.status != 400 && JsonRes != null && JsonRes.status.status.ToLower() == "verified".ToLower())
                        {

                            row["DateOfBirth"] = Convert.ToDateTime(JsonRes.bvn_nuban.birthdate);
                            row["Gender"] = JsonRes.bvn_nuban.gender;
                            row["MiddleName"] = JsonRes.bvn_nuban.middlename;
                            row["BVN"] = JsonRes.bvn_nuban.bvn;
                            row["BankAccountHolderName"] = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                            dtSuccess.Rows.Add(row.ItemArray);


                            if (IsPhoneNumberExist == false && IsemailExist == false && IsstaffIdExist == false)
                            {

                                try
                                {
                                    string iOSApp = "https://apps.apple.com/us/app/paymasta/id1635917623";
                                    string androidApp = "https://play.google.com/store/apps/details?id=com.paymasta.paymasta";
                                    var fileName = AppSetting.LoginCreds;
                                    var body = _emailUtils.ReadEmailformats(fileName);
                                    body = body.Replace("$$Name$$", row["FirstName"].ToString() + " " + row["LastName"].ToString());
                                    body = body.Replace("$$Email$$", row["Email"].ToString());
                                    body = body.Replace("$$PhoneNumber$$", row["PhoneNumber"].ToString());
                                    body = body.Replace("$$Company$$", resultEmployer.OrganisationName);
                                    body = body.Replace("$$Password$$", row["Password"].ToString());
                                    body = body.Replace("$$AndroidApp$$", androidApp);
                                    body = body.Replace("$$IosApp$$", iOSApp);
                                    body = body.Replace("$$webApp$$", "https://paymasta.co");
                                    var emailModel = new EmailModel
                                    {
                                        Body = body,
                                        TO = row["Email"].ToString(),
                                        Subject = "Your account has been created on paymasta"
                                    };
                                    await _emailUtils.SendEmailBySendGrid(emailModel);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        else
                        {
                            row["DateOfBirth"] = DateTime.UtcNow;
                            row["Gender"] = "";
                            row["MiddleName"] = "";
                            row["BVN"] = "";
                            row["BankAccountHolderName"] = row["FirstName"].ToString() + " " + row["LastName"].ToString();

                            errordt.Rows.Add(row.ItemArray);
                        }
                    }
                    #endregion
                }
                //dt.AcceptChanges();
                if (IsPhoneNumberExist == false && IsemailExist == false && IsstaffIdExist == false)
                {
                    if (!NinExist)
                    {
                        result = await _employeesRepository.BulkUploadUsersList(dtSuccess, resultEmployer.OrganisationName, resultEmployer.Id);
                        resultError = await _employeesRepository.BulkUploadUsersListError(errordt, resultEmployer.OrganisationName, resultEmployer.Id);
                        if (result > 0 || resultError > 0)
                        {
                            res.IsSuccess = true;
                            res.Sucess = dtSuccess.Rows.Count;
                            res.Error = errordt.Rows.Count;
                            res.RstKey = 1;
                        }
                        else
                        {
                            res.RstKey = 2;
                            res.IsSuccess = false;
                        }
                    }
                    else
                    {
                        res.RstKey = 12;
                        return res;
                    }
                }
                else
                {
                    res.RstKey = 10;
                    return res;
                }

            }
            catch (Exception ex)
            {
                _logUtils.WriteTextToFile(ex.StackTrace);
                // return 10;
            }

            return res;
        }

        public async Task<BlockUnBlockEmployeeResponse> BlockUnBlockEmployeesError(BlockUnBlockEmployeeRequest request)
        {
            var res = new BlockUnBlockEmployeeResponse();
            var result = new GetEmployerDetailResponse();
            try
            {
                var employeerId = Guid.Parse(request.EmployeerUserGuid.ToString());
                var employeeId = Guid.Parse(request.EmployeeUserGuid.ToString());
                result = await _employeesRepository.GetEmployerDetailByGuid(employeerId);
                var userData = await _accountRepository.GetUserByGuidError(employeeId);
                if (result != null)
                {
                    if (request.DeleteOrBlock == 1)
                    {
                        if (userData.Status == 1)
                        {
                            userData.Status = 0;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_BLOCKED;
                        }
                        else
                        {
                            userData.Status = 1;
                            userData.UpdatedAt = DateTime.UtcNow;
                            userData.UpdatedBy = result.Id;
                            res.Message = AdminResponseMessages.USER_UNBLOCKED;
                        }
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        //  await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                    }
                    if (request.DeleteOrBlock == 2)
                    {
                        userData.Status = 0;
                        userData.IsActive = false;
                        userData.IsDeleted = true;
                        userData.UpdatedAt = DateTime.UtcNow;
                        userData.UpdatedBy = result.Id;
                        res.Message = AdminResponseMessages.USER_DELETED;
                        var sesReq = new LogoutRequest
                        {
                            DeviceId = "",
                            UserGuid = employeeId
                        };
                        // await UpdateSession(sesReq, AdminResponseMessages.DEACTIVATED_SUBADMIN, AdminResponseMessages.DEACTIVATED_SUBADMIN);
                    }
                    if (await _accountRepository.ChangeUserStatusError(userData) > 0)
                    {

                        res.IsSuccess = true;
                        res.RstKey = 1;

                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.RstKey = 2;
                        res.Message = AdminResponseMessages.USER_NOT_FOUND;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.RstKey = 3;
                    res.Message = AdminResponseMessages.USER_NOT_FOUND;

                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
