using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.DTOs.Transactions;

public class TransactionFilterDto
{
    public string? Search { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public TransactionType? Type { get; set; }
    public int? CategoryId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}