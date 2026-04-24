using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Auth;

/// <summary>
/// Request model for forgot password. Contains the user's email address to send the password reset link to if it exists in the system.
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// The user's email address. Must be in a valid email format. This is the email address that the password reset link will be sent to if it exists in the system.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
