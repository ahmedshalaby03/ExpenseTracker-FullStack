using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategorySummaryDto
    {
        public int ExpenseCategoriesCount { get; set; }
        public int IncomeCategoriesCount { get; set; }
        public int UsedCategoriesCount { get; set; }
        public int TotalCategoriesCount { get; set; }
        public decimal UtilizationPercentage { get; set; }
    }
}
