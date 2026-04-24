using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Auth;

/// <summary>
/// Request model for resetting a user's password. Contains the password reset token, the new password, and confirmation of the new password.
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// The password reset token that was generated during the forgot password process. This token is used to verify the user's identity and authorize the password reset operation.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The new password that the user wants to set. Must be at least 8 characters long and no more than 100 characters. This field is required and must match the ConfirmNewPassword field.
    /// </summary>
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation of the new password. Must match the NewPassword field.
    /// </summary>
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    [MaxLength(100)]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}