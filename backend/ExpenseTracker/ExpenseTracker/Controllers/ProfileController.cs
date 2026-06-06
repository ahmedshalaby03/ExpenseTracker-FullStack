using ExpenseTracker.Application.DTOs.Profile;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpenseTracker.Api.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Profile endpoints for managing user personal information, preferences, and security.")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get current user profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _profileService.GetProfileAsync();

            if (profile is null)
                return Unauthorized("User is not authenticated");

            return Ok(profile);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update personal information")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var result = await _profileService.UpdateProfileAsync(dto);

            if (result.Data is null)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPut("preferences")]
        [SwaggerOperation(Summary = "Update user preferences")]
        public async Task<IActionResult> UpdatePreferences(UpdatePreferencesDto dto)
        {
            var result = await _profileService.UpdatePreferencesAsync(dto);

            if (result.Data is null)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPut("change-password")]
        [SwaggerOperation(Summary = "Change current user password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _profileService.ChangePasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return NoContent();
        }


        [HttpPost("avatar")]
        [SwaggerOperation(Summary = "Upload profile avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Image file is required");

            if (file.Length > 2 * 1024 * 1024)
                return BadRequest("Image size must be less than 2 MB");

            var result = await _profileService.UploadAvatarAsync(
                file.OpenReadStream(),
                file.FileName,
                file.ContentType
            );

            if (result.AvatarUrl is null)
                return BadRequest(result.Errors);

            var fullUrl = $"{Request.Scheme}://{Request.Host}{result.AvatarUrl}";

            return Ok(new
            {
                avatarUrl = result.AvatarUrl,
                fullAvatarUrl = fullUrl
            });
        }
    }