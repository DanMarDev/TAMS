using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Auth;

namespace Tams.Api.Controllers;

/// <summary>
/// Controller for handling authentication-related endpoints such as registration, login, forgot password, and reset password. This controller uses the IAuthService to perform the necessary operations for each endpoint.
/// </summary>
/// <param name="authService">The authentication service used to handle registration, login, forgot password, and reset password operations.</param>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{

    /// <summary>
    /// Registers a new user with the provided email, username, and password. If registration is successful, returns an AuthResponse containing the new user's details and a JWT token. If registration fails (e.g., email already exists), returns a BadRequest with the error message.
    /// </summary>
    /// <param name="request">The registration request containing email, username, password, and confirm password.</param>
    /// <returns>An IActionResult containing either the AuthResponse with user details and JWT token or a BadRequest with an error message.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Authenticates a user with the provided email and password. If authentication is successful, returns an AuthResponse containing the user's details and a JWT token. If authentication fails (e.g., invalid email or password), returns a BadRequest with the error message.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>An IActionResult containing either the AuthResponse with user details and JWT token or a BadRequest with an error message.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    /// <summary>
    /// Initiates the forgot password process for a user with the provided email. If the email exists, a password reset token is generated and stored, and an email with reset instructions is sent to the user. 
    /// Returns a string representing the password reset token for testing purposes. If the email does not exist, returns a BadRequest with the error message.
    /// </summary>
    /// <param name="request">The forgot password request containing the user's email.</param>
    /// <returns>An IActionResult containing either the password reset token or a BadRequest with an error message.</returns>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var response = await authService.ForgotPasswordAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Resets the password for a user with the provided reset token and new password. If the reset is successful, returns an OK response. If the reset fails (e.g., invalid token or password), returns a BadRequest with the error message.
    /// </summary>
    /// <param name="request">The reset password request containing the reset token and new password.</param>
    /// <returns>An IActionResult containing either an OK response or a BadRequest with an error message.</returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await authService.ResetPasswordAsync(request);
            return Ok(new { Message = "Password reset successful." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}