using ExpenseTracker.Application.DTOs.Reports;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ExpenseTracker.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ReportService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<ReportSummaryDto> GetSummaryAsync(int month, int year)
    {
        var userId = _currentUserService.UserId;

        var monthlyTransactions = _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.UserId == userId &&
                t.TransactionDate.Month == month &&
                t.TransactionDate.Year == year);

        var monthlyIncome = await monthlyTransactions
            .Where(t => t.Type == TransactionType.Income)
            .SumAsync(t => t.Amount);

        var monthlyExpenses = await monthlyTransactions
            .Where(t => t.Type == TransactionType.Expense)
            .SumAsync(t => t.Amount);

        var topCategory = await monthlyTransactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => new
            {
                t.CategoryId,
                CategoryName = t.Category.Name
            })
            .Select(g => new
            {
                g.Key.CategoryName,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.TotalAmount)
            .FirstOrDefaultAsync();

        var lastMonthDate = new DateTime(year, month, 1).AddMonths(-1);

        var lastMonthTopCategorySpending = 0m;

        if (topCategory is not null)
        {
            lastMonthTopCategorySpending = await _context.Transactions
                .Include(t => t.Category)
                .Where(t =>
                    t.UserId == userId &&
                    t.Type == TransactionType.Expense &&
                    t.Category.Name == topCategory.CategoryName &&
                    t.TransactionDate.Month == lastMonthDate.Month &&
                    t.TransactionDate.Year == lastMonthDate.Year)
                .SumAsync(t => t.Amount);
        }

        string insightMessage = "No spending insights available for this month.";

        if (topCategory is not null)
        {
            if (lastMonthTopCategorySpending > 0)
            {
                var differencePercentage = Math.Round(
                    ((topCategory.TotalAmount - lastMonthTopCategorySpending) / lastMonthTopCategorySpending) * 100,
                    2
                );

                if (differencePercentage > 0)
                {
                    insightMessage =
                        $"You spent {differencePercentage}% more on {topCategory.CategoryName} compared to last month. Consider setting a budget for this category.";
                }
                else if (differencePercentage < 0)
                {
                    insightMessage =
                        $"You spent {Math.Abs(differencePercentage)}% less on {topCategory.CategoryName} compared to last month. Good progress.";
                }
                else
                {
                    insightMessage =
                        $"Your spending on {topCategory.CategoryName} is the same as last month.";
                }
            }
            else
            {
                insightMessage =
                    $"{topCategory.CategoryName} is your top spending category this month.";
            }
        }

        return new ReportSummaryDto
        {
            MonthlyIncome = monthlyIncome,
            MonthlyExpenses = monthlyExpenses,
            MonthlyBalance = monthlyIncome - monthlyExpenses,
            TopCategoryName = topCategory?.CategoryName,
            TopCategoryAmount = topCategory?.TotalAmount ?? 0,
            InsightMessage = insightMessage
        };
    }

    public async Task<IEnumerable<ReportCategoryBreakdownDto>> GetCategoryBreakdownAsync(int month, int year)
    {
        var userId = _currentUserService.UserId;

        var expenses = _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.UserId == userId &&
                t.Type == TransactionType.Expense &&
                t.TransactionDate.Month == month &&
                t.TransactionDate.Year == year);

        var totalExpenses = await expenses.SumAsync(t => t.Amount);

        return await expenses
            .GroupBy(t => new
            {
                t.CategoryId,
                CategoryName = t.Category.Name
            })
            .Select(g => new ReportCategoryBreakdownDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName,
                TotalSpent = g.Sum(t => t.Amount),
                TransactionsCount = g.Count(),
                Percentage = totalExpenses > 0
                    ? Math.Round((g.Sum(t => t.Amount) / totalExpenses) * 100, 2)
                    : 0
            })
            .OrderByDescending(x => x.TotalSpent)
            .ToListAsync();
    }

    public async Task<IEnumerable<DailySpendingDto>> GetDailySpendingAsync(int month, int year)
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Where(t =>
                t.UserId == userId &&
                t.Type == TransactionType.Expense &&
                t.TransactionDate.Month == month &&
                t.TransactionDate.Year == year)
            .GroupBy(t => t.TransactionDate.Date)
            .Select(g => new DailySpendingDto
            {
                Date = g.Key,
                TotalSpent = g.Sum(t => t.Amount)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncomeVsExpensesReportDto>> GetIncomeVsExpensesAsync(int year)
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Where(t =>
                t.UserId == userId &&
                t.TransactionDate.Year == year)
            .GroupBy(t => t.TransactionDate.Month)
            .Select(g => new IncomeVsExpensesReportDto
            {
                Month = g.Key,
                Income = g
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount),
                Expenses = g
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount)
            })
            .OrderBy(x => x.Month)
            .ToListAsync();
    }

    public async Task<byte[]> ExportCsvAsync(int month, int year)
    {
        var userId = _currentUserService.UserId;

        var transactions = await _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.UserId == userId &&
                t.TransactionDate.Month == month &&
                t.TransactionDate.Year == year)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync();

        var csv = new StringBuilder();

        csv.AppendLine("Date,Description,Category,Type,Payment Method,Amount");

        foreach (var t in transactions)
        {
            var type = t.Type == TransactionType.Income ? "Income" : "Expense";

            var paymentMethod = t.PaymentMethod.ToString();

            csv.AppendLine(
                $"{t.TransactionDate:yyyy-MM-dd}," +
                $"\"{t.Description}\"," +
                $"\"{t.Category.Name}\"," +
                $"{type}," +
                $"{paymentMethod}," +
                $"{t.Amount}"
            );
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}