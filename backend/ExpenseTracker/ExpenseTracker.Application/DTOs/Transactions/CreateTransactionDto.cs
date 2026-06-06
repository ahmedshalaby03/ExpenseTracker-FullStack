using System;
using System.Collections.Generic;
using System.Text;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.DTOs.Transactions
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int CategoryId { get; set; }
    }
}
