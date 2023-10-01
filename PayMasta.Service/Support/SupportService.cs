using PayMasta.DBEntity.Support;
using PayMasta.Repository.Account;
using PayMasta.Repository.SupportRepository;
using PayMasta.Utilities;
using PayMasta.Utilities.EmailUtils;
using PayMasta.Utilities.SMSUtils;
using PayMasta.ViewModel.Common;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.SupportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Support
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepository;
        private readonly ISMSUtils _iSMSUtils;
        private readonly IEmailUtils _emailUtils;
        private readonly IAccountRepository _accountRepository;
        public SupportService()
        {
            _accountRepository = new AccountRepository();
            _iSMSUtils = new SMSUtils();
            _emailUtils = new EmailUtils();
            _supportRepository = new SupportRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public async Task<SupportResponse> InsertSupportTicket(SupportRequest request)
        {
            var res = new SupportResponse();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null && user.UserType == (int)EnumUserType.Customer)
            {
                if (user.Id > 0)
                {
                    var req = new SupportMaster
                    {
                        TicketNumber = request.TicketNumber,
                        Title = request.Title,
                        DescriptionText = request.DescriptionText,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Id,
                        Status = (int)EnumSupportStatus.Pending,
                        UserId = user.Id,
                    };
                    var result = await _supportRepository.InsertSupportDetail(req);
                    if (result > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 2;
                    }
                }


            }
            else if (user != null && user.UserType == (int)EnumUserType.Employer)
            {
                if (user.Id > 0)
                {
                    var req = new SupportMaster
                    {
                        TicketNumber = request.TicketNumber,
                        Title = request.Title,
                        DescriptionText = request.DescriptionText,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Id,
                        Status = (int)EnumSupportStatus.Pending,
                        UserId = user.Id,
                    };
                    var result = await _supportRepository.InsertSupportDetail(req);
                    if (result > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = ResponseMessages.DATA_SAVED;
                        res.RstKey = 1;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = ResponseMessages.DATA_NOT_SAVED;
                        res.RstKey = 2;
                    }
                }


            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.DATA_NOT_SAVED;
                res.RstKey = 3;
            }
            return res;
        }

        public async Task<SupportMasterResponse> GetSupportDetailList(GetSupportMasterRequest request)
        {
            var res = new SupportMasterResponse();
            var result = new List<SupportMasterTicketResponse>();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                result = await _supportRepository.GetSupportDetailList(user.Id,request.pageNumber,request.PageSize);
                if (result.Count > 0)
                {
                    res.supportMasterTicketResponse = result;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = AdminResponseMessages.DATA_NOT_FOUND_GENERIC;
                    res.RstKey = 2;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                res.RstKey = 3;
            }
            return res;
        }
        public async Task<SupportMasterResponse> GetSupportDetailList(GetSupportListRequest request)
        {
            var res = new SupportMasterResponse();
            var result = new List<SupportMasterTicketResponse>();
            var user = await _accountRepository.GetUserByGuid(request.UserGuid);
            if (user != null)
            {
                result = await _supportRepository.GetSupportDetailList(user.Id);
                if (result.Count > 0)
                {
                    res.supportMasterTicketResponse = result;
                    res.IsSuccess = true;
                    res.Message = ResponseMessages.DATA_RECEIVED;
                    res.RstKey = 1;
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = AdminResponseMessages.DATA_NOT_FOUND_GENERIC;
                    res.RstKey = 2;
                }

            }
            else
            {
                res.IsSuccess = false;
                res.Message = ResponseMessages.DATA_NOT_RECEIVED;
                res.RstKey = 3;
            }
            return res;
        }
    }
}
