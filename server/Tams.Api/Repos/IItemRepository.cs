using Tams.Api.Models;

namespace Tams.Api.Repos
{
    public class ItemWithLatestValuation
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Model { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal MaybeSellThreshold { get; set; }
        public decimal? OriginalValue { get; set; }
        public string? Condition { get; set; }
        public decimal? LatestEstimatedValue { get; set; }
        public DateTime? LatestValuationAt { get; set; }
    }

    internal interface IItemRepository
    {
        Task<Item?> GetItemByIdAsync(int itemId);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId);
        Task<IEnumerable<ItemWithLatestValuation>> GetItemsWithLatestValuationAsync(int userId);
        Task<Item?> GetItemAsync(int itemId, int userId);
        Task<int> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int itemId);
        // Brands
        Task<int> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(int brandId, int userId);
        Task<IEnumerable<Brand>> GetBrandsAsync(int userId);
        Task<Brand?> GetBrandByIdAsync(int brandId, int userId);
        // Categories
        Task<int> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int categoryId, int userId);
        Task<IEnumerable<Category>> GetCategoriesAsync(int userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
    }
}