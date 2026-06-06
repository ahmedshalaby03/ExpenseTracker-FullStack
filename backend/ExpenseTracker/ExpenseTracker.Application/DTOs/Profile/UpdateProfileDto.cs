using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? PreferredCurrency { get; set; }
    }
}
