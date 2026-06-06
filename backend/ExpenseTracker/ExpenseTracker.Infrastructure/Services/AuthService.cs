using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<(AuthResponseDto? Data, IEnumerable<string>? Errors)> RegisterAsync(RegisterDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.FullName))
            errors.Add("Full name is required");

        if (string.IsNullOrWhiteSpace(dto.Email))
            errors.Add("Email is required");

        if (string.IsNullOrWhiteSpace(dto.Password))
            errors.Add("Password is required");

        if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            errors.Add("Confirm password is required");

        if (!string.IsNullOrWhiteSpace(dto.Password) &&
            !string.IsNullOrWhiteSpace(dto.ConfirmPassword) &&
            dto.Password != dto.ConfirmPassword)
        {
            errors.Add("Password and confirm password do not match");
        }

        if (errors.Any())
            return (null, errors);

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser is not null)
            return (null, new[] { "Email already exists" });

        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
        {
            var identityErrors = result.Errors.Select(e => e.Description);
            return (null, identityErrors);
        }

        var token = await _jwtService.GenerateTokenAsync(
            user.Id,
            user.Email!,
            user.FullName
        );

        var response = new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!
        };

        return (response, null);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
            return null;

        var token = await _jwtService.GenerateTokenAsync(
            user.Id,
            user.Email!,
            user.FullName
        );

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!
        };
    }
}