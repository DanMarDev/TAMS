namespace Tams.Api.Services.Auth
{
    /// <summary>
    /// Service interface for handling user authentication, registration, and password reset functionality. Provides methods for registering new users, 
    /// authenticating existing users, initiating the forgot password process, and resetting passwords.
    /// </summary>

    public interface IAuthService
    {

        /// <summary>
        /// Registers a new user with the provided email, username, and password.
        /// </summary>
        /// <param name="request">The registration request containing email, username, password, and confirm password.</param>
        /// <returns>AuthResponse with user details and JWT token</returns>
        /// <exception cref="Exception">Thrown if a user with the provided email already exists.</exception>
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Authenticates a user with the provided email and password. If successful, returns an AuthResponse containing user details and a JWT token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>AuthResponse with user details and JWT token.</returns>
        /// <exception cref="Exception">Thrown if the email does not exist or the password is incorrect.</exception>
        Task<AuthResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Initiates the forgot password process for a user with the provided email. If the email exists, a password reset token is generated and stored, and an email with reset instructions is sent to the user.
        /// </summary>
        /// <param name="request">The forgot password request containing the user's email.</param>
        /// <returns>A string representing the password reset token.</returns>
        Task<string> ForgotPasswordAsync(ForgotPasswordRequest request);
        
        /// <summary>
        /// Resets the user's password with the provided token and new password.
        /// </summary>
        /// <param name="request">The reset password request containing the token and new password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ResetPasswordAsync(ResetPasswordRequest request);
    }
}