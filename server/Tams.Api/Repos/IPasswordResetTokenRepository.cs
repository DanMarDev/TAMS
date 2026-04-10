using Tams.Api.Models;

namespace Tams.Api.Repos
{
    internal interface IPasswordResetTokenRepository
    {
        Task<int> CreateTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash);
        Task MarkUsedAsync(int resetTokenId);
    }
}