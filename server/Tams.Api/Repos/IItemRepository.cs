using Tams.Api.Models;

namespace Tams.Api.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetItemByIdAsync(int itemId);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId);
        Task<int> CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int itemId);
    }
}