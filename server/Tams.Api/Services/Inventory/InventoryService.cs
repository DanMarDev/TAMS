using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Inventory
{
    internal class InventoryService(
        IItemRepository itemRepo
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
    }
}