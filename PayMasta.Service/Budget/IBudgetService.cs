using PayMasta.ViewModel.BudgetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Budget
{
    public interface IBudgetService
    {
        Task<ServiceCategoryResponse> GetServiceList(Guid userGuid);
        Task<SaveBudgetResponse> SaveBudget(CreateBudgetRequest request);
        Task<ExpenseTrackResponse> GetExpenses(Guid userGuid);
        Task<ExpenseTrackResponse> GetBudget(Guid userGuid);
        Task<DailyExpenseTrackResponse> GetDailyExpense(Guid userGuid);
    }
}
