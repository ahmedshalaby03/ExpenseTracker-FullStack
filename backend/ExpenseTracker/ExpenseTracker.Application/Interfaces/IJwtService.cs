using System;
using System.Collections.Generic;
using System.Text;


namespace ExpenseTracker.Application.Interfaces

{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(string userId, string email, string fullName);
    }
}
