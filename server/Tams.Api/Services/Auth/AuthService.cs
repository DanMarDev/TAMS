using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Auth
{
    internal class AuthService(
        IUserRepository userRepo,
        IPasswordResetTokenRepository tokenRepo,
        IConfiguration config
    ) : IAuthService
    {
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if user with email already exists
            var existingUser = await userRepo.GetUserByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                throw new Exception("An account with this email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            user.UserId = await userRepo.CreateUserAsync(user);

            return new AuthResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                Token = GenerateJwt(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await userRepo.GetUserByEmailAsync(request.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            return new AuthResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                Token = GenerateJwt(user)
            };
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await userRepo.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                return "If that email exists, a password reset link has been sent.";
            }

            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var hashedToken = HashToken(rawToken);

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                TokenHash = hashedToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await tokenRepo.CreateTokenAsync(resetToken);

            // TODO: Send email with reset link containing rawToken
            return $"Password reset token (for testing): {rawToken}";
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var hashedToken = HashToken(request.Token);
            var resetToken = await tokenRepo.GetByTokenHashAsync(hashedToken);
            if (resetToken is null || resetToken.UsedAt != null || resetToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired password reset token.");
            }

            // Update user's password
            var user = await userRepo.GetUserByIdAsync(resetToken.UserId);
            if (user is null)
            {
                throw new Exception("User not found.");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await userRepo.UpdateUserAsync(user);

            // Mark the token as used
            await tokenRepo.MarkUsedAsync(resetToken.ResetTokenId);
        }

        // =========================
        // Private helper methods
        // =========================
        private string GenerateJwt(User user)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
              new Claim(JwtRegisteredClaimNames.Email, user.Email),
              new Claim("username", user.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    double.Parse(jwtSettings["ExpiresInHours"] ?? "1")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashToken(string rawToken)
        {
            var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToBase64String(bytes);
        }
    }
}