using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Auth;

/// <summary>
/// Request model for user login. Contains the user's email and password.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The user's email address. Must be in a valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// The user's password. Must be at least 8 characters long and no more than 100 characters.
    /// </summary>
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
