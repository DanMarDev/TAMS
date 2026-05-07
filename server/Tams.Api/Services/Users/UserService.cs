using Tams.Api.Repos;

namespace Tams.Api.Services.Users
{
    internal class UserService(IUserRepository userRepo) : IUserService
    {
        public async Task<UserProfileResponse?> GetProfileAsync(int userId)
        {
            var user = await userRepo.GetUserByIdAsync(userId);
            if (user is null) return null;

            return new UserProfileResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                DefaultSellThreshold = user.DefaultSellThreshold,
                CreatedAt = user.CreatedAt,
            };
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username is required.");

            var user = await userRepo.GetUserByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await userRepo.GetUserByEmailAsync(request.Email);
                if (existing is not null && existing.UserId != userId)
                    throw new InvalidOperationException("An account with this email already exists.");
            }

            user.Email = request.Email.Trim();
            user.UserName = request.UserName.Trim();
            await userRepo.UpdateUserAsync(user);

            return new UserProfileResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                DefaultSellThreshold = user.DefaultSellThreshold,
                CreatedAt = user.CreatedAt,
            };
        }

        public async Task<UserProfileResponse> UpdateSellThresholdAsync(int userId, UpdateSellThresholdRequest request)
        {
            if (request.DefaultSellThreshold < 0)
                throw new ArgumentException("Default sell threshold cannot be negative.");

            var user = await userRepo.GetUserByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            user.DefaultSellThreshold = request.DefaultSellThreshold;
            await userRepo.UpdateUserAsync(user);

            return new UserProfileResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                DefaultSellThreshold = user.DefaultSellThreshold,
                CreatedAt = user.CreatedAt,
            };
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.NewPassword))
                throw new ArgumentException("New password is required.");
            if (request.NewPassword.Length < 6)
                throw new ArgumentException("New password must be at least 6 characters.");
            if (request.NewPassword != request.ConfirmPassword)
                throw new ArgumentException("New passwords do not match.");

            var user = await userRepo.GetUserByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await userRepo.UpdateUserAsync(user);
        }
    }
}
