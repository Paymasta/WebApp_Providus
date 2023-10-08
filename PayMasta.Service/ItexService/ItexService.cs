using Newtonsoft.Json;
using PayMasta.DBEntity.WalletService;
using PayMasta.Repository.Account;
using PayMasta.Repository.CommonReporsitory;
using PayMasta.Repository.ItexRepository;
using PayMasta.Service.ThirdParty;
using PayMasta.Utilities;
using PayMasta.ViewModel.BouquetsVM;
using PayMasta.ViewModel.ElectricityVM;
using PayMasta.ViewModel.Enums;
using PayMasta.ViewModel.InternetVM;
using PayMasta.ViewModel.ItexVM;
using PayMasta.ViewModel.PlanVM;
using PayMasta.ViewModel.ValidateMultichoiceVM;
using PayMasta.ViewModel.ValidateStartimesVM;
using PayMasta.ViewModel.ValidatInternetVM;
using PayMasta.ViewModel.WalletToBankVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.ItexService
{
    public class ItexService : IItexService
    {
        private readonly IThirdParty _thirdParty;
        private readonly ICommonReporsitory _commonReporsitory;
        private readonly IItexRepository _itexRepository;
        private readonly IAccountRepository _accountRepository;
        public ItexService()
        {
            _accountRepository = new AccountRepository();
            //_iSMSUtils = new SMSUtils();
            //_emailUtils = new EmailUtils();
            _thirdParty = new ThirdPartyService();
            _commonReporsitory = new CommonReporsitory();
            //_accountRepository = new AccountRepository();
            //_commonService = new CommonService();
            _itexRepository = new ItexRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        //Airtime
        public async Task<OperatorResponse> GetAirtimeOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            //StringBuilder st=new StringBuilder();
            //st.Append("ss");
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var url = AppSetting.billersOperator;
                //string token = tokenDetail.AuthToken;
                //var res = await _thirdParty.getOperatorList(url, token);
                //var JsonResult = JsonConvert.DeserializeObject<ITextGetOperatorListResponse>(res);
                //if (JsonResult.data.Count > 0)
                //{
                //    foreach (var item in JsonResult.data)
                //    {
                //        if (item.category.ToUpper() == "Airtime".ToUpper())
                //        {
                //            item.billers.ForEach(async b =>
                //            {
                //                if (b.accountType != null)
                //                {
                //                    b.accountType.ForEach(async c =>
                //                    {
                //                        var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                        if (walletService == null)
                //                        {
                //                            var reqWalletserive = new WalletService
                //                            {
                //                                ServiceName = b.name,
                //                                BankCode = item.category,
                //                                CreatedAt = DateTime.UtcNow,
                //                                CreatedBy = 0,
                //                                SubCategoryId = 1,
                //                                IsActive = true,
                //                                IsDeleted = false,
                //                                ImageUrl = "",
                //                                HttpVerbs = "",
                //                                UpdatedAt = DateTime.UtcNow,
                //                                UpdatedBy = 0,
                //                                RawData = res,
                //                                CountryCode = "NG",
                //                                BillerName = b.service,
                //                                OperatorId = 0,
                //                                AccountType = c.name
                //                            };
                //                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                        }
                //                    });
                //                }
                //                else
                //                {
                //                    var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                    if (walletService == null)
                //                    {
                //                        var reqWalletserive = new WalletService
                //                        {
                //                            ServiceName = b.name,
                //                            BankCode = item.category,
                //                            CreatedAt = DateTime.UtcNow,
                //                            CreatedBy = 0,
                //                            SubCategoryId = 1,
                //                            IsActive = true,
                //                            IsDeleted = false,
                //                            ImageUrl = "",
                //                            HttpVerbs = "",
                //                            UpdatedAt = DateTime.UtcNow,
                //                            UpdatedBy = 0,
                //                            RawData = res,
                //                            CountryCode = "NG",
                //                            BillerName = b.service,
                //                            OperatorId = 0,
                //                            AccountType = ""
                //                        };
                //                        await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                    }
                //                }

                //            });
                //        }
                //    }
                //    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.AIRTIME);
                //    result.RstKey = 1;
                //    result.IsSuccess = true;
                //    result.operatorResponse = data;
                //}
                //else
                //{
                //    result.RstKey = 2;
                //    result.IsSuccess = true;
                //}
                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.AIRTIME);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<OperatorResponse> GetInternetOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var url = AppSetting.billersOperator;
                string token = tokenDetail.AuthToken;
                var res = await _thirdParty.getOperatorList(url, token);
                var JsonResult = JsonConvert.DeserializeObject<ITextGetOperatorListResponse>(res);
                if (JsonResult.data.Count > 0)
                {
                    foreach (var item in JsonResult.data)
                    {
                        if (item.category.ToUpper() == "Internet".ToUpper())
                        {
                            item.billers.ForEach(async b =>
                        {
                            if (b.accountType != null)
                            {
                                b.accountType.ForEach(async c =>
                                {
                                    var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                                    if (walletService == null)
                                    {
                                        var reqWalletserive = new WalletService
                                        {
                                            ServiceName = b.name,
                                            BankCode = item.category,
                                            CreatedAt = DateTime.UtcNow,
                                            CreatedBy = 0,
                                            SubCategoryId = 2,
                                            IsActive = true,
                                            IsDeleted = false,
                                            ImageUrl = "",
                                            HttpVerbs = "",
                                            UpdatedAt = DateTime.UtcNow,
                                            UpdatedBy = 0,
                                            RawData = res,
                                            CountryCode = "NG",
                                            BillerName = b.service,
                                            OperatorId = 0,
                                            AccountType = c.name
                                        };
                                        await _commonReporsitory.InsertWalletServices(reqWalletserive);
                                    }
                                });
                            }
                            else
                            {
                                var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                                if (walletService == null)
                                {
                                    var reqWalletserive = new WalletService
                                    {
                                        ServiceName = b.name,
                                        BankCode = item.category,
                                        CreatedAt = DateTime.UtcNow,
                                        CreatedBy = 0,
                                        SubCategoryId = 2,
                                        IsActive = true,
                                        IsDeleted = false,
                                        ImageUrl = "",
                                        HttpVerbs = "",
                                        UpdatedAt = DateTime.UtcNow,
                                        UpdatedBy = 0,
                                        RawData = res,
                                        CountryCode = "NG",
                                        BillerName = b.service,
                                        OperatorId = 0,
                                        AccountType = ""
                                    };
                                    await _commonReporsitory.InsertWalletServices(reqWalletserive);
                                }
                            }

                        });
                        }
                    }
                    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.INTERNET);
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.operatorResponse = data;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<OperatorResponse> GetTvOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var url = AppSetting.billersOperator;
                //string token = tokenDetail.AuthToken;
                //var res = await _thirdParty.getOperatorList(url, token);
                //var JsonResult = JsonConvert.DeserializeObject<ITextGetOperatorListResponse>(res);
                //if (JsonResult.data.Count > 0)
                //{
                //    foreach (var item in JsonResult.data)
                //    {
                //        if (item.category.ToUpper() == "CableTv".ToUpper())
                //        {
                //            item.billers.ForEach(async b =>
                //            {
                //                if (b.accountType != null)
                //                {
                //                    b.accountType.ForEach(async c =>
                //                    {
                //                        var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                        if (walletService == null)
                //                        {
                //                            var reqWalletserive = new WalletService
                //                            {
                //                                ServiceName = b.name,
                //                                BankCode = item.category,
                //                                CreatedAt = DateTime.UtcNow,
                //                                CreatedBy = 0,
                //                                SubCategoryId = 3,
                //                                IsActive = true,
                //                                IsDeleted = false,
                //                                ImageUrl = "",
                //                                HttpVerbs = "",
                //                                UpdatedAt = DateTime.UtcNow,
                //                                UpdatedBy = 0,
                //                                RawData = res,
                //                                CountryCode = "NG",
                //                                BillerName = b.service,
                //                                OperatorId = 0,
                //                                AccountType = c.name
                //                            };
                //                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                        }
                //                    });
                //                }
                //                else
                //                {
                //                    var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                    if (walletService == null)
                //                    {
                //                        var reqWalletserive = new WalletService
                //                        {
                //                            ServiceName = b.name,
                //                            BankCode = item.category,
                //                            CreatedAt = DateTime.UtcNow,
                //                            CreatedBy = 0,
                //                            SubCategoryId = 3,
                //                            IsActive = true,
                //                            IsDeleted = false,
                //                            ImageUrl = "",
                //                            HttpVerbs = "",
                //                            UpdatedAt = DateTime.UtcNow,
                //                            UpdatedBy = 0,
                //                            RawData = res,
                //                            CountryCode = "NG",
                //                            BillerName = b.service,
                //                            OperatorId = 0,
                //                            AccountType = ""
                //                        };
                //                        await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                    }
                //                }
                //            });
                //        }
                //    }
                //    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.TV);
                //    result.RstKey = 1;
                //    result.IsSuccess = true;
                //    result.operatorResponse = data;
                //}
                //else
                //{
                //    result.RstKey = 2;
                //    result.IsSuccess = true;
                //}
                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.TV);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<OperatorResponse> GetElectricityOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var url = AppSetting.billersOperator;
                //string token = tokenDetail.AuthToken;
                //var res = await _thirdParty.getOperatorList(url, token);
                //var JsonResult = JsonConvert.DeserializeObject<ITextGetOperatorListResponse>(res);
                //if (JsonResult.data.Count > 0)
                //{
                //    foreach (var item in JsonResult.data)
                //    {
                //        if (item.category.ToUpper() == "Electricity".ToUpper())
                //        {
                //            item.billers.ForEach(async b =>
                //            {
                //                if (b.accountType != null)
                //                {
                //                    b.accountType.ForEach(async c =>
                //                    {
                //                        var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                        if (walletService == null)
                //                        {
                //                            var reqWalletserive = new WalletService
                //                            {
                //                                ServiceName = b.name,
                //                                BankCode = item.category,
                //                                CreatedAt = DateTime.UtcNow,
                //                                CreatedBy = 0,
                //                                SubCategoryId = 4,
                //                                IsActive = true,
                //                                IsDeleted = false,
                //                                ImageUrl = "",
                //                                HttpVerbs = "",
                //                                UpdatedAt = DateTime.UtcNow,
                //                                UpdatedBy = 0,
                //                                RawData = res,
                //                                CountryCode = "NG",
                //                                BillerName = b.service,
                //                                OperatorId = 0,
                //                                AccountType = c.name
                //                            };
                //                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                        }
                //                    });
                //                }
                //                else
                //                {
                //                    var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                    if (walletService == null)
                //                    {
                //                        var reqWalletserive = new WalletService
                //                        {
                //                            ServiceName = b.name,
                //                            BankCode = item.category,
                //                            CreatedAt = DateTime.UtcNow,
                //                            CreatedBy = 0,
                //                            SubCategoryId = 4,
                //                            IsActive = true,
                //                            IsDeleted = false,
                //                            ImageUrl = "",
                //                            HttpVerbs = "",
                //                            UpdatedAt = DateTime.UtcNow,
                //                            UpdatedBy = 0,
                //                            RawData = res,
                //                            CountryCode = "NG",
                //                            BillerName = b.service,
                //                            OperatorId = 0,
                //                            AccountType = ""
                //                        };
                //                        await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                    }
                //                }

                //            });
                //        }
                //    }
                //    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                //    result.RstKey = 1;
                //    result.IsSuccess = true;
                //    result.operatorResponse = data;
                //}
                //else
                //{
                //    result.RstKey = 2;
                //    result.IsSuccess = true;
                //}

                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.Power);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<OperatorResponse> GetDataOperatorList(OperatorRequest request)
        {
            var result = new OperatorResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var url = AppSetting.billersOperator;
                string token = tokenDetail.AuthToken;
                //var res = await _thirdParty.getOperatorList(url, token);
                //var JsonResult = JsonConvert.DeserializeObject<ITextGetOperatorListResponse>(res);
                //if (JsonResult != null && JsonResult.data.Count > 0)
                //{
                //    foreach (var item in JsonResult.data)
                //    {
                //        if (item.category.ToUpper() == "Data".ToUpper())
                //        {
                //            item.billers.ForEach(async b =>
                //            {
                //                if (b.accountType != null)
                //                {
                //                    b.accountType.ForEach(async c =>
                //                    {
                //                        var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                        if (walletService == null)
                //                        {
                //                            var reqWalletserive = new WalletService
                //                            {
                //                                ServiceName = b.name,
                //                                BankCode = item.category,
                //                                CreatedAt = DateTime.UtcNow,
                //                                CreatedBy = 0,
                //                                SubCategoryId = 5,
                //                                IsActive = true,
                //                                IsDeleted = false,
                //                                ImageUrl = "",
                //                                HttpVerbs = "",
                //                                UpdatedAt = DateTime.UtcNow,
                //                                UpdatedBy = 0,
                //                                RawData = res,
                //                                CountryCode = "NG",
                //                                BillerName = b.service,
                //                                OperatorId = 0,
                //                                AccountType = c.name
                //                            };
                //                            await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                        }
                //                    });
                //                }
                //                else
                //                {
                //                    var walletService = await _commonReporsitory.IsWalletServicesExists(item.category, b.name);
                //                    if (walletService == null)
                //                    {
                //                        var reqWalletserive = new WalletService
                //                        {
                //                            ServiceName = b.name,
                //                            BankCode = item.category,
                //                            CreatedAt = DateTime.UtcNow,
                //                            CreatedBy = 0,
                //                            SubCategoryId = 5,
                //                            IsActive = true,
                //                            IsDeleted = false,
                //                            ImageUrl = "",
                //                            HttpVerbs = "",
                //                            UpdatedAt = DateTime.UtcNow,
                //                            UpdatedBy = 0,
                //                            RawData = res,
                //                            CountryCode = "NG",
                //                            BillerName = b.service,
                //                            OperatorId = 0,
                //                            AccountType = ""
                //                        };
                //                        await _commonReporsitory.InsertWalletServices(reqWalletserive);
                //                    }
                //                }
                //            });
                //        }
                //    }
                //    var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.DATABUNDLE);
                //    result.RstKey = 1;
                //    result.IsSuccess = true;
                //    result.operatorResponse = data;
                //}
                //else
                //{
                //    result.RstKey = 2;
                //    result.IsSuccess = true;
                //}
                var data = await _itexRepository.GetWalletServicesListBySubcategoryId((int)WalletTransactionSubTypes.DATABUNDLE);
                result.RstKey = 1;
                result.IsSuccess = true;
                result.operatorResponse = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetPlanListResponses> GetDataOperatorPlanList(OperatorPlanRequest request)
        {
            var result = new GetPlanListResponses();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndService(request.SubCategoryId, request.service);
                var url = AppSetting.billersPlans;
                var planReq = new PlanListRequest();
                string token = tokenDetail.AuthToken;

                if (request.service == "9 Mobile" && request.SubCategoryId == 5)
                {
                    planReq.channel = "MOBILE";
                    planReq.service = "9mobiledata";
                }
                else
                {
                    planReq.channel = "MOBILE";
                    planReq.service = walletServiceData.ServiceName.ToLower() + "data".ToLower();
                }

                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<PlanListResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.planListResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetValidateMeterResponseResponses> MeterValidate(MeterValidateRequest request)
        {
            var result = new GetValidateMeterResponseResponses();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.meterValidate;
                string token = tokenDetail.AuthToken;

                var planReq = new ValidateMeter
                {
                    accountType = request.AccountType,
                    amount = request.Amount,
                    meterNo = request.MeterNo,
                    channel = "MOBILE",
                    service = walletServiceData.BillerName.ToLower(),
                };
                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidateMeterResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetBouquestResponses> MultichoiceBouquets(GetBouquetDataRequest request)
        {
            var result = new GetBouquestResponses();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.Bouquets;
                string token = tokenDetail.AuthToken;

                var planReq = new BouquetsRequest
                {
                    type = request.AccountType,
                    service = walletServiceData.BillerName.ToLower(),
                };
                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<BouquetsResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetValidateBouquestResponses> ValidateMultichoice(GetValidatBouquetDataRequest request)
        {
            var result = new GetValidateBouquestResponses();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.Validatemultichoice;
                string token = tokenDetail.AuthToken;

                var planReq = new ValidatRequest
                {
                    type = request.AccountType,
                    service = walletServiceData.BillerName.ToLower(),
                    account = request.Account,
                    amount = request.Amount,
                    channel = "MOBILE",
                    smartCardCode = request.SmartCardCode,
                };
                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidatResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetValidateStarTimesResponses> ValidateStartimes(GetValidatStarTimesDataRequest request)
        {
            var result = new GetValidateStarTimesResponses();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.Validatestartimes;
                string token = tokenDetail.AuthToken;

                var planReq = new ValidatStarTimesRequest
                {
                    type = request.AccountType,
                    service = walletServiceData.BillerName.ToLower(),
                    account = request.Account,
                    amount = request.Amount,
                    channel = "MOBILE",
                    smartCardCode = request.SmartCardCode,
                };
                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidatStarTimesResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateMeterResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public async Task<GetInternetBundlesResponse> GetInternetBundles(GetInternetBundlesRequest request)
        {
            var result = new GetInternetBundlesResponse();
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.InternetBunldes;
                string token = tokenDetail.AuthToken;



                var planReq = new InternetBundlesRequest
                {
                    type = request.AccountType,
                    service = walletServiceData.BillerName.ToLower(),
                    account = request.Account,
                    channel = "MOBILE"
                };
                var req = JsonConvert.SerializeObject(planReq);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<InternetBundlesResponse>(res);
                if (JsonResult.code == "00")
                {
                    // result.productCode = validateRes.validateInternetResponse.data.productCode;
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.internetBundlesResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }


            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<GetValidateInternetResponse> GetInternetValidation(ValidateInternetRequest request, string token)
        {
            var result = new GetValidateInternetResponse();
            try
            {
                //var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                //var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                //var walletServiceData = await _itexRepository.GetWalletServicesListBySubcategoryIdAndServiceForElectricity(request.SubCategoryId, request.Service, request.AccountType);
                var url = AppSetting.InternetValidation;
                // string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1VTRVIiLCJleHAiOjE2NjIxMjQzODR9.s0goU6ituGFf8pML6svrskFGrup77kCmULwXpcWFtjYfUHirSgfNiQLU9eGViT7a12f2MAzGpv9rVPMNxKTRUA";


                var req = JsonConvert.SerializeObject(request);
                var res = await _thirdParty.postOperatorList(req, url, token);
                var JsonResult = JsonConvert.DeserializeObject<ValidateInternetResponse>(res);
                if (JsonResult.code == "00")
                {

                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.validateInternetResponse = JsonResult;
                }
                else
                {
                    result.RstKey = 2;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<BankListResponse> GetBankList(OperatorRequest request)
        {
            var result = new BankListResponse();
            //StringBuilder st=new StringBuilder();
            //st.Append("ss");
            string token = String.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var url = AppSetting.BankListUrl;
                if (tokenDetail != null)
                {
                    token = tokenDetail.AuthToken;
                }
                else
                {
                    token = "";
                }
                var res = await _thirdParty.getOperatorList(url, token);
                var JsonData = JsonConvert.DeserializeObject<PouchiBankListResponse>(res);
                if (JsonData.code == "00")
                {
                    result.IsSuccess = true;
                    result.RstKey = 1;
                    result.pouchiBankListResponse = JsonData;
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<LatestBankListResponse> GetLatestBankList(OperatorRequest request)
        {
            var result = new LatestBankListResponse();
            string token = String.Empty;
            try
            {
                var userData = await _accountRepository.GetUserByGuid(request.UserGuid);
                var tokenDetail = await _itexRepository.GetVirtualAccountDetailByUserId(userData.Id);
                var url = AppSetting.LatestBankListUrl;
                if (tokenDetail != null)
                {
                    token = tokenDetail.AuthToken;
                    var res = await _thirdParty.getOperatorList(url, token);
                    var JsonData = JsonConvert.DeserializeObject<CashConnectResponse>(res);
                    var bankData = JsonConvert.DeserializeObject<List<CashConnectBanks>>(JsonData.data);
                    if (JsonData.code == "00")
                    {
                        result.IsSuccess = true;
                        result.RstKey = 1;
                        result.pouchiBankListResponse = bankData;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.RstKey = 2;

                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.RstKey = 2;
                }
                //token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIrMjM0OTAzNTA2MTYxMSIsImF1dGgiOiJST0xFX1BBWU1BU1RBX0FETUlOIiwiZXhwIjoxNjcxMDM2NzQ0fQ.M4_iX9vHdWAjizoD2rJw-efokbW0rFdV0qQfXmzlQMsgMYTBRCsfDK1wgCcoN84s_fgzzzmXJNqhT0txaYT18g";
               

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
