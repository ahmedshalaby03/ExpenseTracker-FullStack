namespace ExpenseTracker.Application.DTOs.Reports;

public class ReportSummaryDto
{
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal MonthlyBalance { get; set; }

    public string? TopCategoryName { get; set; }
    public decimal TopCategoryAmount { get; set; }

    public string InsightMessage { get; set; } = string.Empty;
}