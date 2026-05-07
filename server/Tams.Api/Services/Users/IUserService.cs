namespace Tams.Api.Services.Users
{
    public interface IUserService
    {
        Task<UserProfileResponse?> GetProfileAsync(int userId);
        Task<UserProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request);
        Task<UserProfileResponse> UpdateSellThresholdAsync(int userId, UpdateSellThresholdRequest request);
        Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
    }
}
