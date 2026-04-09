using System.Data;
using Dapper;
using Tams.Api.Models;

namespace Tams.Api.Repos
{
    public class PasswordResetTokenRepository(IDbConnection db) : IPasswordResetTokenRepository
    {
        public async Task<int> CreateTokenAsync(PasswordResetToken token)
        {
            const string sql = """
                INSERT INTO PasswordResetTokens (user_id, token_hash, expires_at)
                OUTPUT INSERTED.reset_token_id
                VALUES (@UserId, @TokenHash, @ExpiresAt)
                """;

            return await db.ExecuteScalarAsync<int>(sql, token);
        }

        public async Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash)
        {
            const string sql = """
                SELECT reset_token_id AS ResetTokenId,
                       user_id AS UserId,
                       token_hash AS TokenHash,
                       expires_at AS ExpiresAt,
                       used_at AS UsedAt,
                       created_at AS CreatedAt
                FROM PasswordResetTokens
                WHERE token_hash = @TokenHash
                """;
            return await db.QuerySingleOrDefaultAsync<PasswordResetToken>(sql, new { TokenHash = tokenHash });
        }

        public async Task MarkUsedAsync(int resetTokenId)
        {
            const string sql = """
                UPDATE PasswordResetTokens
                SET used_at = GETDATE()
                WHERE reset_token_id = @ResetTokenId
                """;
            await db.ExecuteAsync(sql, new { ResetTokenId = resetTokenId });
        }
    }
}