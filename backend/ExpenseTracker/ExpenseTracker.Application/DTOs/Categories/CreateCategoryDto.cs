using ExpenseTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
