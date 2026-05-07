using System.Data;
using Dapper;
using Tams.Api.Models;

namespace Tams.Api.Repos
{
    internal class UserRepository(IDbConnection db) : IUserRepository
    {
        private const string SelectColumns = """
            SELECT user_id AS UserId,
                   email AS Email,
                   password_hash AS PasswordHash,
                   user_name AS UserName,
                   default_sell_threshold AS DefaultSellThreshold,
                   created_at AS CreatedAt
            """;

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var sql = $"{SelectColumns} FROM Users WHERE user_id = @UserId";
            return await db.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = $"{SelectColumns} FROM Users WHERE email = @Email";
            return await db.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User user)
        {
            const string sql = """
                INSERT INTO Users (email, password_hash, user_name, default_sell_threshold)
                OUTPUT INSERTED.user_id
                VALUES (@Email, @PasswordHash, @UserName, @DefaultSellThreshold)
                """;

            return await db.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            const string sql = """
                UPDATE Users
                SET email = @Email,
                    password_hash = @PasswordHash,
                    user_name = @UserName,
                    default_sell_threshold = @DefaultSellThreshold
                WHERE user_id = @UserId
                """;
            await db.ExecuteAsync(sql, user);
        }
    }
}
