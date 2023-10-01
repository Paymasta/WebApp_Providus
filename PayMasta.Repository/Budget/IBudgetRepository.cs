using PayMasta.DBEntity.ServiceCategory;
using PayMasta.DBEntity.UserBudget;
using PayMasta.ViewModel.BudgetVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Budget
{
    public interface IBudgetRepository
    {
        Task<List<BudgetServiceCategory>> GetServiceList(IDbConnection exdbConnection = null);
        Task<UserBudgeting> SaveBudget(UserBudgeting userBudgeting, IDbConnection exdbConnection = null);
        Task<UserBudgeting> GetCurrentMonthBudget(long userId, long categoryId, IDbConnection exdbConnection = null);
        Task<ExpenseTrack> GetExpenses(long userId, IDbConnection exdbConnection = null);
        Task<ExpenseTrack> GetBudget(long userId, IDbConnection exdbConnection = null);
        Task<List<DailyExpenseAmount>> GetDailyExpense(long userId, IDbConnection exdbConnection = null);
    }
}
