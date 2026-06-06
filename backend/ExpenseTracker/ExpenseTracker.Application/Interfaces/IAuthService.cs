using ExpenseTracker.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(AuthResponseDto? Data, IEnumerable<string>? Errors)> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

    }
}
