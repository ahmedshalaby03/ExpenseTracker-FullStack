namespace ExpenseTracker.Application.DTOs.Reports;

public class DailySpendingDto
{
    public DateTime Date { get; set; }
    public decimal TotalSpent { get; set; }
}