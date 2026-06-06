using ExpenseTracker.Application.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto?> GetProfileAsync();
        Task<(ProfileDto? Data, IEnumerable<string>? Errors)> UpdateProfileAsync(UpdateProfileDto dto);
        Task<(ProfileDto? Data, IEnumerable<string>? Errors)> UpdatePreferencesAsync(UpdatePreferencesDto dto);
        Task<(bool Success, IEnumerable<string>? Errors)> ChangePasswordAsync(ChangePasswordDto dto);
        Task<(string? AvatarUrl, IEnumerable<string>? Errors)> UploadAvatarAsync(Stream fileStream, string fileName, string contentType);
    }
}
