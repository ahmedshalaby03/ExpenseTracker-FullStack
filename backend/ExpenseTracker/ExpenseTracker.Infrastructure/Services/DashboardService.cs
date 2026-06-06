using ExpenseTracker.Application.DTOs.Dashboard;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DashboardService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var userId = _currentUserService.UserId;

        var transactions = _context.Transactions
            .Where(t => t.UserId == userId);

        var totalIncome = await transactions
            .Where(t => t.Type == TransactionType.Income)
            .SumAsync(t => t.Amount);

        var totalExpenses = await transactions
            .Where(t => t.Type == TransactionType.Expense)
            .SumAsync(t => t.Amount);

        var balance = totalIncome - totalExpenses;

        var savingRate = totalIncome > 0
            ? Math.Round((balance / totalIncome) * 100, 2)
            : 0;

        var totalTransactions = await transactions.CountAsync();

        return new DashboardSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Balance = balance,
            SavingRate = savingRate,
            TotalTransactions = totalTransactions
        };
    }

    public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 5)
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.Id)
            .Take(count)
            .Select(t => new RecentTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                TransactionDate = t.TransactionDate,
                PaymentMethod = t.PaymentMethod,
                CategoryName = t.Category.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ExpenseByCategoryDto>> GetExpensesByCategoryAsync()
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .GroupBy(t => new
            {
                t.CategoryId,
                CategoryName = t.Category.Name
            })
            .Select(g => new ExpenseByCategoryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.TotalAmount)
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlyIncomeExpenseDto>> GetMonthlyIncomeExpenseAsync(int year)
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Where(t => t.UserId == userId && t.TransactionDate.Year == year)
            .GroupBy(t => new
            {
                t.TransactionDate.Year,
                t.TransactionDate.Month
            })
            .Select(g => new MonthlyIncomeExpenseDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalIncome = g
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount),
                TotalExpenses = g
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount)
            })
            .OrderBy(x => x.Month)
            .ToListAsync();
    }

    public async Task<TopCategoryDto?> GetTopCategoryAsync()
    {
        var userId = _currentUserService.UserId;

        return await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .GroupBy(t => new
            {
                t.CategoryId,
                CategoryName = t.Category.Name
            })
            .Select(g => new TopCategoryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.TotalAmount)
            .FirstOrDefaultAsync();
    }

}