using Tams.Api.Models;

namespace Tams.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}