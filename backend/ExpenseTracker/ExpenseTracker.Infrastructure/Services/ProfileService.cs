using ExpenseTracker.Application.DTOs.Profile;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ProfileService(
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ProfileDto?> GetProfileAsync()
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
            return null;

        return MapToProfileDto(user);
    }

    public async Task<(ProfileDto? Data, IEnumerable<string>? Errors)> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
            return (null, new[] { "User not found" });

        if (string.IsNullOrWhiteSpace(dto.FullName))
            return (null, new[] { "Full name is required" });

        user.FullName = dto.FullName;
        user.PreferredCurrency = dto.PreferredCurrency;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return (null, result.Errors.Select(e => e.Description));

        return (MapToProfileDto(user), null);
    }

    public async Task<(ProfileDto? Data, IEnumerable<string>? Errors)> UpdatePreferencesAsync(UpdatePreferencesDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
            return (null, new[] { "User not found" });

        user.DefaultPaymentMethod = dto.DefaultPaymentMethod;
        user.MonthlyBudgetLimit = dto.MonthlyBudgetLimit;
        user.EmailNotifications = dto.EmailNotifications;
        user.PushNotifications = dto.PushNotifications;
        user.SmsAlerts = dto.SmsAlerts;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return (null, result.Errors.Select(e => e.Description));

        return (MapToProfileDto(user), null);
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
            return (false, new[] { "User not found" });

        if (dto.NewPassword != dto.ConfirmNewPassword)
            return (false, new[] { "New password and confirm password do not match" });

        var result = await _userManager.ChangePasswordAsync(
            user,
            dto.CurrentPassword,
            dto.NewPassword
        );

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        return (true, null);
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(userId))
            return null;

        return await _userManager.FindByIdAsync(userId);
    }

    private static ProfileDto MapToProfileDto(ApplicationUser user)
    {
        var securityScore = 60;

        if (!string.IsNullOrWhiteSpace(user.Email))
            securityScore += 10;

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            securityScore += 10;

        if (user.TwoFactorEnabled)
            securityScore += 20;

        return new ProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PreferredCurrency = user.PreferredCurrency,
            DefaultPaymentMethod = user.DefaultPaymentMethod,
            MonthlyBudgetLimit = user.MonthlyBudgetLimit,
            EmailNotifications = user.EmailNotifications,
            PushNotifications = user.PushNotifications,
            SmsAlerts = user.SmsAlerts,
            AvatarUrl = user.AvatarUrl,
            CreatedAt = user.CreatedAt,
            SecurityScore = securityScore
        };
    }

    public async Task<(string? AvatarUrl, IEnumerable<string>? Errors)> UploadAvatarAsync(
     Stream fileStream,
     string fileName,
     string contentType)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
            return (null, new[] { "User not found" });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

        if (!allowedTypes.Contains(contentType))
            return (null, new[] { "Only JPG, PNG, and WEBP images are allowed" });

        var extension = Path.GetExtension(fileName).ToLower();

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        if (!allowedExtensions.Contains(extension))
            return (null, new[] { "Invalid image extension" });

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var newFileName = $"{user.Id}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, newFileName);

        await using var outputStream = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(outputStream);

        user.AvatarUrl = $"/uploads/avatars/{newFileName}";

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return (null, result.Errors.Select(e => e.Description));

        return (user.AvatarUrl, null);
    }
}