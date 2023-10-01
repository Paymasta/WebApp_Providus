using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.BudgetVM
{
    public class BudgetServiceCategory
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string CategoryName { get; set; }
    }

    public class ServiceCategoryResponse
    {
        public ServiceCategoryResponse()
        {
            serviceCategories = new List<BudgetServiceCategory>();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }

        public List<BudgetServiceCategory> serviceCategories { get; set; }
    }

    public class CreateBudgetRequest
    {
        public int CategoryId { get; set; }
        public Guid UserGuid { get; set; }
        public decimal Amount { get; set; }
    }

    public class SaveBudgetResponse
    {
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }

    public class ExpenseTrack
    {
        public string AIRTIME { get; set; }
        public string CABLE { get; set; }
        public string INTERNET { get; set; }
        public string ELECTICITY { get; set; }
        public string DATABUNDLE { get; set; }
    }

    public class ExpenseTrackResponse
    {
        public ExpenseTrackResponse()
        {
            expenseTrack = new ExpenseTrack();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }

        public ExpenseTrack expenseTrack { get; set; }
    }

    public class DailyExpenseTrackResponse
    {
        public DailyExpenseTrackResponse()
        {
            dailyExpenseAmounts = new List<DailyExpenseAmount>();
        }
        public int RstKey { get; set; }
        public bool IsSuccess { get; set; }

        public List<DailyExpenseAmount> dailyExpenseAmounts { get; set; }
    }

    public class DailyExpenseAmount
    {
        public string TotalAmount { get; set; }
    }
}
