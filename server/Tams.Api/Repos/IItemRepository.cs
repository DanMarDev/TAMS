using Tams.Api.Models;

namespace Tams.Api.Repos
{
    public interface IItemRepository
    {
        Task<Item?> GetItemByIdAsync(int itemId);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId);
        Task<int> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task DeleteItemAsync(int itemId);
        // Brands
        Task<int> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(int brandId, int userId);
        Task<IEnumerable<Brand>> GetBrandsAsync(int userId);
        Task<Brand?> GetBrandByIdAsync(int brandId, int userId);
        // Categories
        Task<int> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId, int userId);
        Task<IEnumerable<Category>> GetCategoriesAsync(int userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
    }
}