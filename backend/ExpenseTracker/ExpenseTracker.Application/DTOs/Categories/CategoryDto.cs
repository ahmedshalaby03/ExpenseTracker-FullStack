using ExpenseTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int TransactionsCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
