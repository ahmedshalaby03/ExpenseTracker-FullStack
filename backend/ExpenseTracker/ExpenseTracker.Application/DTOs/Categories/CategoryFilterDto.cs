using ExpenseTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategoryFilterDto
    {
        public string? Search { get; set; }
        public TransactionType? Type { get; set; }
    }
}
