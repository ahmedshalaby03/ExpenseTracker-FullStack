namespace ExpenseTracker.Application.DTOs.Reports;

public class IncomeVsExpensesReportDto
{
    public int Month { get; set; }
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
}