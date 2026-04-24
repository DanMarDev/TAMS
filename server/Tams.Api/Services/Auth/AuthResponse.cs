namespace Tams.Api.Services.Auth;

/// <summary>
/// Response model for authentication operations. Contains the authenticated user's ID, username, email, and JWT token for subsequent authenticated requests.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// The authenticated user's unique identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The authenticated user's username. This is the display name that can be shown in the UI after login.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The authenticated user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The JWT token for authenticated requests.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
