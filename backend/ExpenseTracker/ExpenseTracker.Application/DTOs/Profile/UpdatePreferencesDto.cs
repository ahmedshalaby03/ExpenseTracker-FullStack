using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Profile
{
    public class UpdatePreferencesDto
    {
        public string? DefaultPaymentMethod { get; set; }
        public decimal? MonthlyBudgetLimit { get; set; }

        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        public bool SmsAlerts { get; set; }
    }
}
