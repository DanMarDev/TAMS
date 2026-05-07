using Tams.Api.Models;

namespace Tams.Api.Services.Inventory
{
    public interface IInventoryService
    {
        // ============== Item Management ==============

        /// <summary>
        /// Gets an item by its ID for a specific user. Returns null if the item does not 
        /// exist or does not belong to the user.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userId"></param>
        /// <returns>The item if found, otherwise null.</returns>
        Task<Item?> GetItemByIdAsync(int itemId, int userId);

        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId);

        Task<int> CreateItemAsync(Item item);

        /// <summary>
        /// Updates an existing item. The item must belong to the user specified by userId. Returns true 
        /// if the update was successful, false if the item does not exist or does not belong to the user.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UpdateItemAsync(Item item, int userId);
        Task<bool> DeleteItemAsync(int itemId, int userId);

        // ============= Brand Management ==============
        /// <summary>
        /// Gets all brands belonging to a specific user and brands built-in to the system.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Brand>> GetBrandsAsync(int userId);
        Task<Brand?> GetBrandByIdAsync(int brandId, int userId);
        Task<int> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand, int userId);
        Task<bool> DeleteBrandAsync(int brandId, int userId);

        // =========== Category Management ==============
        /// <summary>
        /// Gets all categories belonging to a specific user and categories built-in to the system.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetCategoriesAsync(int userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
        Task<int> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category, int userId);
        Task<bool> DeleteCategoryAsync(int categoryId, int userId);

        // ============ Dashboard Analytics ==============
        Task<int> GetTotalItemCountAsync(int userId);

        /// <summary>
        /// Aggregates dashboard data for the user: summary counts/totals, the five oldest items
        /// by purchase date, and items flagged as resale candidates ("Maybe Sell").
        /// </summary>
        Task<DashboardResponse> GetDashboardAsync(int userId);
        // Task<decimal> GetTotalEstimatedValueAsync(int userId);
        // TODO: Move to PricingService

        // ======== Maybe Sell Candidate Analysis ===========
        // Task<bool> IsMaybeSellCandidateAsync(int itemId, int userId, decimal? latestEsimatedValue);
        // TODO: Move to PricingService
    }
}