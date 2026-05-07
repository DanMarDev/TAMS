using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Inventory
{
    internal class InventoryService(
        IItemRepository itemRepo,
        IWarrantyRepository warrantyRepo
    ) : IInventoryService
    {
        // =========== Item CRUD ============
        #region Item CRUD
        public async Task<Item?> GetItemByIdAsync(int itemId, int userId)
        {
            var item = await itemRepo.GetItemByIdAsync(itemId);
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId)
        {
            return await itemRepo.GetItemsByUserIdAsync(userId);
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            return await itemRepo.CreateItemAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Item item, int userId)
        {
            var existingItem = await itemRepo.GetItemByIdAsync(item.ItemId);

            if (existingItem is null || existingItem.UserId != userId)
            {
                return false;
            }

            return await itemRepo.UpdateItemAsync(item);
        }

        public async Task<bool> DeleteItemAsync(int itemId, int userId)
        {
            var existingItem = await itemRepo.GetItemByIdAsync(itemId);

            if (existingItem is null || existingItem.UserId != userId)
            {
                return false;
            }

            return await itemRepo.DeleteItemAsync(itemId);
        }

        #endregion

        // ====== Brand CRUD ======
        public async Task<IEnumerable<Brand>> GetBrandsAsync(int userId)
        {
            return await itemRepo.GetBrandsAsync(userId);
        }

        public async Task<Brand?> GetBrandByIdAsync(int brandId, int userId)
        {
            return await itemRepo.GetBrandByIdAsync(brandId, userId);
        }

        public async Task<int> CreateBrandAsync(Brand brand)
        {
            if (string.IsNullOrWhiteSpace(brand.Name))
                throw new ArgumentException("Brand name is required.");

            brand.Name = brand.Name.Trim();

            var existing = (await itemRepo.GetBrandsAsync(brand.UserId ?? 0))
                .FirstOrDefault(b => string.Equals(b.Name, brand.Name, StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
                return existing.BrandId;

            return await itemRepo.CreateBrandAsync(brand);
        }

        public async Task<bool> UpdateBrandAsync(Brand brand, int userId)
        {
            var existingBrand = await itemRepo.GetBrandByIdAsync(brand.BrandId, userId);

            if (existingBrand is null || existingBrand.UserId != userId)
            {
                return false;
            }

            return await itemRepo.UpdateBrandAsync(brand);
        }

        public async Task<bool> DeleteBrandAsync(int brandId, int userId)
        {
            var existingBrand = await itemRepo.GetBrandByIdAsync(brandId, userId);

            if (existingBrand is null || existingBrand.UserId != userId)
            {
                return false;
            }

            return await itemRepo.DeleteBrandAsync(brandId, userId);
        }

        // ====== Category CRUD ======
        public async Task<IEnumerable<Category>> GetCategoriesAsync(int userId)
        {
            return await itemRepo.GetCategoriesAsync(userId);
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, int userId)
        {
            return await itemRepo.GetCategoryByIdAsync(categoryId, userId);
        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name is required.");

            category.Name = category.Name.Trim();

            var existing = (await itemRepo.GetCategoriesAsync(category.UserId ?? 0))
                .FirstOrDefault(c => string.Equals(c.Name, category.Name, StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
                return existing.CategoryId;

            return await itemRepo.CreateCategoryAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(Category category, int userId)
        {
            var existingCategory = await itemRepo.GetCategoryByIdAsync(category.CategoryId, userId);

            if (existingCategory is null || existingCategory.UserId != userId)
            {
                return false;
            }

            return await itemRepo.UpdateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, int userId)
        {
            var existingCategory = await itemRepo.GetCategoryByIdAsync(categoryId, userId);

            if (existingCategory is null || existingCategory.UserId != userId)
            {
                return false;
            }

            return await itemRepo.DeleteCategoryAsync(categoryId, userId);
        }

        // ====== Dashboard Analytics ======
        public async Task<int> GetTotalItemCountAsync(int userId)
        {
            var items = await itemRepo.GetItemsByUserIdAsync(userId);
            return items.Count();
        }

        public async Task<DashboardResponse> GetDashboardAsync(int userId)
        {
            var items = (await itemRepo.GetItemsWithLatestValuationAsync(userId)).ToList();
            var expiring = await warrantyRepo.GetExpiringWarrantiesAsync(userId, 30);

            static DashboardItem ToDto(ItemWithLatestValuation x) => new()
            {
                ItemId = x.ItemId,
                CategoryId = x.CategoryId,
                BrandId = x.BrandId,
                Name = x.Name,
                Model = x.Model,
                PurchaseDate = x.PurchaseDate,
                PurchasePrice = x.PurchasePrice,
                MaybeSellThreshold = x.MaybeSellThreshold,
                Condition = x.Condition,
                LatestEstimatedValue = x.LatestEstimatedValue,
                LatestValuationAt = x.LatestValuationAt,
            };

            // "Maybe Sell": surface items whose latest estimated value meets or exceeds the user's
            // sell threshold (i.e. resale is potentially worthwhile). Items priced below the threshold
            // are not shown.
            var maybeSell = items
                .Where(i => i.LatestEstimatedValue.HasValue && i.LatestEstimatedValue.Value >= i.MaybeSellThreshold)
                .OrderByDescending(i => i.LatestEstimatedValue)
                .Select(ToDto)
                .ToList();

            // Oldest items: prefer purchase_date asc; nulls go last.
            var oldest = items
                .OrderBy(i => i.PurchaseDate.HasValue ? 0 : 1)
                .ThenBy(i => i.PurchaseDate ?? DateOnly.MaxValue)
                .Take(5)
                .Select(ToDto)
                .ToList();

            // Total estimated value: sum latest valuation when available, fall back to purchase price.
            var totalEstimated = items.Sum(i => i.LatestEstimatedValue ?? i.PurchasePrice ?? 0m);

            return new DashboardResponse
            {
                Summary = new DashboardSummary
                {
                    TotalItems = items.Count,
                    TotalEstimatedValue = totalEstimated,
                    MaybeSellCount = maybeSell.Count,
                    ExpiringWarrantyCount = expiring.Count(),
                },
                OldestItems = oldest,
                MaybeSellItems = maybeSell,
            };
        }
    }
}