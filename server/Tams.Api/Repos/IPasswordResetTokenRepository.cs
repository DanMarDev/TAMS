using Tams.Api.Models;

namespace Tams.Api.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task<int> CreateTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash);
        Task MarkUsedAsync(int resetTokenId);
    }
}