using PayMasta.Utilities;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using PayMasta.Repository.Budget;
using PayMasta.ViewModel.BudgetVM;
using PayMasta.DBEntity.UserBudget;
using PayMasta.Repository.Account;
using PayMasta.ViewModel.Common;
using System.Data.Common;
using System.Collections.Generic;

namespace PayMasta.Service.Budget
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IAccountRepository _accountRepository;
        public BudgetService()
        {
            _budgetRepository = new BudgetRepository();
            _accountRepository = new AccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<ServiceCategoryResponse> GetServiceList(Guid userGuid)
        {
            var result = new ServiceCategoryResponse();

            using (var dbConnection = Connection)
            {
                var serviceList = await _budgetRepository.GetServiceList(dbConnection);
                if (serviceList.Count > 0)
                {
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.serviceCategories = serviceList;
                }
                else
                {

                }

            }
            return result;
        }
        public async Task<SaveBudgetResponse> SaveBudget(CreateBudgetRequest request)
        {
            var result = new SaveBudgetResponse();
            var userData = await _accountRepository.GetUserByGuid(request.UserGuid);

            var currentBudget = await _budgetRepository.GetCurrentMonthBudget(userData.Id, request.CategoryId);
            if (userData != null)
            {
                if (currentBudget == null)
                {
                    using (var dbConnection = Connection)
                    {
                        var userBudget = new UserBudgeting
                        {
                            BudgetAmount = request.Amount,
                            CategoryId = request.CategoryId,
                            UserId = userData.Id,
                            CreatedAt = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            UsedPercentage = 0,
                            UpdatedAt = DateTime.UtcNow,
                        };
                        var budgetRes = await _budgetRepository.SaveBudget(userBudget, dbConnection);
                        if (budgetRes.Id > 0)
                        {
                            result.RstKey = 1;
                            result.IsSuccess = true;
                            result.Message = ResponseMessages.BUDGET_SAVED;
                        }
                        else
                        {
                            result.RstKey = 2;
                            result.IsSuccess = false;
                            result.Message = ResponseMessages.DATA_NOT_SAVED;
                        }
                    }
                }
                else
                {
                    result.RstKey = 4;
                    result.IsSuccess = false;
                    result.Message = ResponseMessages.CATEGORY_EXISTS;
                }

            }
            else
            {
                result.RstKey = 3;
                result.IsSuccess = false;
                result.Message = ResponseMessages.USER_NOT_EXIST;
            }

            return result;
        }

        public async Task<ExpenseTrackResponse> GetExpenses(Guid userGuid)
        {
            var result = new ExpenseTrackResponse();
            var res = new ExpenseTrack();
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            using (var dbConnection = Connection)
            {
                var expense = await _budgetRepository.GetExpenses(userData.Id, dbConnection);
                if (expense != null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.expenseTrack = expense;
                }
                else
                {
                    result.RstKey = 2;

                }

            }
            return result;
        }
        public async Task<ExpenseTrackResponse> GetBudget(Guid userGuid)
        {
            var result = new ExpenseTrackResponse();
            var res = new ExpenseTrack();
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            using (var dbConnection = Connection)
            {
                var expense = await _budgetRepository.GetBudget(userData.Id, dbConnection);
                if (expense != null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.expenseTrack = expense;
                }
                else
                {
                    result.RstKey = 2;

                }

            }
            return result;
        }

        public async Task<DailyExpenseTrackResponse> GetDailyExpense(Guid userGuid)
        {
            var result = new DailyExpenseTrackResponse();
            var res = new List<DailyExpenseAmount>();
            var userData = await _accountRepository.GetUserByGuid(userGuid);
            using (var dbConnection = Connection)
            {
                var expense = await _budgetRepository.GetDailyExpense(userData.Id, dbConnection);
                if (expense != null)
                {
                    result.RstKey = 1;
                    result.IsSuccess = true;
                    result.dailyExpenseAmounts = expense;
                }
                else
                {
                    result.RstKey = 2;

                }

            }
            return result;
        }
    }
}
