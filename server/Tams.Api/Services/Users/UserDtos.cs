namespace Tams.Api.Services.Users
{
    public sealed class UserProfileResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal DefaultSellThreshold { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public sealed class UpdateProfileRequest
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public sealed class UpdateSellThresholdRequest
    {
        public decimal DefaultSellThreshold { get; set; }
    }

    public sealed class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
