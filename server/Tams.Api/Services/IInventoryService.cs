using Tams.Api.Models;

namespace Tams.Api.Services
{
    internal interface IInventoryService
    {
        // Item CRUD
        Task<Item?> GetItemByIdAsync(int itemId, int userId);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId);
        Task<int> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item, int userId);
        Task<bool> DeleteItemAsync(int itemId, int userId);

        // Brand Management
        Task<IEnumerable<Brand>> GetBrandsAsync(int userId);
        Task<Brand?> GetBrandByIdAsync(int brandId, int userId);
        Task<int> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand, int userId);
        Task<bool> DeleteBrandAsync(int brandId, int userId);

        // Category Management
        Task<IEnumerable<Category>> GetCategoriesAsync(int userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
        Task<int> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category, int userId);
        Task<bool> DeleteCategoryAsync(int categoryId, int userId);

        // Dashboard Aggregations
        Task<int> GetTotalItemCountAsync(int userId);
        Task<decimal> GetTotalValueAsync(int userId);

        // Maybe Sell Scoring
        Task<bool> IsMaybeSellCandidateAsync(int itemId, int userId, decimal? latestEsimatedValue);
    }
}