using ExpenseTracker.Application.DTOs.Reports;

namespace ExpenseTracker.Application.Interfaces;

public interface IReportService
{
    Task<ReportSummaryDto> GetSummaryAsync(int month, int year);
    Task<IEnumerable<ReportCategoryBreakdownDto>> GetCategoryBreakdownAsync(int month, int year);
    Task<IEnumerable<DailySpendingDto>> GetDailySpendingAsync(int month, int year);
    Task<IEnumerable<IncomeVsExpensesReportDto>> GetIncomeVsExpensesAsync(int year);
    Task<byte[]> ExportCsvAsync(int month, int year);
}