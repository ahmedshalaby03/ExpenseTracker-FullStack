namespace ExpenseTracker.Application.DTOs.Reports;

public class ReportCategoryBreakdownDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public decimal TotalSpent { get; set; }
    public decimal Percentage { get; set; }

    public int TransactionsCount { get; set; }
}