using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Auth;

/// <summary>
/// Request model for user registration. Contains the user's email, password, username, and password confirmation.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// The user's email address. Must be unique and in a valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's password. Must be at least 8 characters long.
    /// </summary>
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The user's desired username. Must be unique and between 3 and 255 characters.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;


    /// <summary>
    /// Confirmation of the user's password. Must match the Password field.
    /// </summary>
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [MaxLength(100)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
