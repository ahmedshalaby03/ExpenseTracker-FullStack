using ExpenseTracker.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 5);
        Task<IEnumerable<ExpenseByCategoryDto>> GetExpensesByCategoryAsync();
        Task<IEnumerable<MonthlyIncomeExpenseDto>> GetMonthlyIncomeExpenseAsync(int year);
        Task<TopCategoryDto?> GetTopCategoryAsync();

    }
}
