using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Profile
{
    public class ProfileDto
    { 
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? PreferredCurrency { get; set; }
        public string? DefaultPaymentMethod { get; set; }
        public decimal? MonthlyBudgetLimit { get; set; }

        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        public bool SmsAlerts { get; set; }

        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int SecurityScore { get; set; }
    }
}
