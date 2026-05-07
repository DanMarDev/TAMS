using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Users;

namespace Tams.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UserController(IUserService userService) : AppControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var profile = await userService.GetProfileAsync(GetUserIdFromClaims());
            if (profile is null) return NotFound("User not found.");
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        try
        {
            var updated = await userService.UpdateProfileAsync(GetUserIdFromClaims(), request);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("me/sell-threshold")]
    public async Task<IActionResult> UpdateSellThreshold([FromBody] UpdateSellThresholdRequest request)
    {
        try
        {
            var updated = await userService.UpdateSellThresholdAsync(GetUserIdFromClaims(), request);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            await userService.ChangePasswordAsync(GetUserIdFromClaims(), request);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
