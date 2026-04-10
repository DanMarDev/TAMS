using System.Data;
using Dapper;
using Tams.Api.Models;

namespace Tams.Api.Repos
{
    internal class ItemRepository(IDbConnection db) : IItemRepository
    {
        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            const string sql = """
                SELECT item_id                AS ItemId,
                       user_id                AS UserId,
                       category_id            AS CategoryId,
                       brand_id               AS BrandId,
                       name                   AS Name,
                       model                  AS Model,
                       purchase_date          AS PurchaseDate,
                       purchase_price         AS PurchasePrice,
                       maybe_sell_threshold   AS MaybeSellThreshold,
                       original_value         AS OriginalValue,
                       condition              AS Condition,
                       notes                  AS Notes,
                       created_at             AS CreatedAt,
                       updated_at             AS UpdatedAt
                FROM Items
                WHERE item_id = @ItemId
                """;
            return await db.QueryFirstOrDefaultAsync<Item>(sql, new { ItemId = itemId });
        }

        public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId)
        {
            const string sql = """
                SELECT item_id                AS ItemId,
                       user_id                AS UserId,
                       category_id            AS CategoryId,
                       brand_id               AS BrandId,
                       name                   AS Name,
                       model                  AS Model,
                       purchase_date          AS PurchaseDate,
                       purchase_price         AS PurchasePrice,
                       maybe_sell_threshold   AS MaybeSellThreshold,
                       original_value         AS OriginalValue,
                       condition              AS Condition,
                       notes                  AS Notes,
                       created_at             AS CreatedAt,
                       updated_at             AS UpdatedAt
                FROM Items
                WHERE user_id = @UserId
                """;
            return await db.QueryAsync<Item>(sql, new { UserId = userId });
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            const string sql = """
                INSERT INTO Items (user_id, category_id, brand_id, name, model,
                                   purchase_date, purchase_price, maybe_sell_threshold, original_value, condition, notes)
                OUTPUT INSERTED.item_id
                VALUES (@UserId, @CategoryId, @BrandId, @Name, @Model,
                        @PurchaseDate, @PurchasePrice, @MaybeSellThreshold, @OriginalValue, @Condition, @Notes)
                """;
            return await db.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            const string sql = """
                UPDATE Items
                SET category_id          = @CategoryId,
                    brand_id             = @BrandId,
                    name                 = @Name,
                    model                = @Model,
                    purchase_date        = @PurchaseDate,
                    purchase_price       = @PurchasePrice,
                    maybe_sell_threshold = @MaybeSellThreshold,
                    original_value       = @OriginalValue,
                    condition            = @Condition,
                    notes                = @Notes,
                    updated_at           = GETDATE()
                WHERE item_id = @ItemId
                """;
            int rowsAffected = await db.ExecuteAsync(sql, item);
            return rowsAffected > 0;
        }

        public async Task DeleteItemAsync(int itemId)
        {
            const string sql = """
                DELETE FROM Items
                WHERE item_id = @ItemId
                """;
            await db.ExecuteAsync(sql, new { ItemId = itemId });
        }

        /** 
        ==========================
        Brands 
        ==========================
        **/
        public async Task<int> CreateBrandAsync(Brand brand)
        {
            const string sql = """
                INSERT INTO Brands (user_id, name)
                OUTPUT INSERTED.brand_id
                VALUES (@UserId, @Name)
                """;
            return await db.ExecuteScalarAsync<int>(sql, brand);
        }

        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            const string sql = """
                UPDATE Brands
                OUTPUT UPDATED.brand_id
                SET name = @Name
                WHERE brand_id = @BrandId AND user_id = @UserId
                """;
            int rowsAffected = await db.ExecuteAsync(sql, brand);
            return rowsAffected > 0;
        }

        public async Task DeleteBrandAsync(int brandId, int userId)
        {
            const string sql = """
                DELETE FROM Brands
                WHERE brand_id = @BrandId AND user_id = @UserId
                """;
            await db.ExecuteAsync(sql, new { BrandId = brandId, UserId = userId });
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync(int userId)
        {
            const string sql = """
                SELECT brand_id AS BrandID,
                        name as Name,
                        is_official AS IsOfficial
                FROM Brands
                WHERE user_id = @UserId OR is_official = 1
                ORDER BY is_official DESC, name ASC
                """;
            return await db.QueryAsync<Brand>(sql, new { UserId = userId });
        }

        public async Task<Brand?> GetBrandByIdAsync(int brandId, int userId)
        {
            const string sql = """
                SELECT brand_id AS BrandID,
                        name as Name,
                        is_official AS IsOfficial
                FROM Brands
                WHERE (user_id = @UserId OR is_official = 1) AND brand_id = @BrandId
                """;
            return await db.QueryFirstOrDefaultAsync<Brand>(sql, new { UserId = userId, BrandId = brandId });
        }

        /** 
        ==========================
        Categories
        ==========================
        */

        public async Task<int> CreateCategoryAsync(Category category)
        {
            const string sql = """
                INSERT INTO Categories (user_id, name)
                OUTPUT INSERTED.category_id
                VALUES (@UserId, @Name)
                """;
            return await db.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            const string sql = """
                UPDATE Categories
                OUTPUT UPDATED.category_id
                SET name = @Name
                WHERE category_id = @CategoryId AND user_id = @UserId
                """;
            int rowsAffected = await db.ExecuteAsync(sql, category);
            return rowsAffected > 0;
        }

        public async Task DeleteCategoryAsync(int categoryId, int userId)
        {
            const string sql = """
                DELETE FROM Categories
                WHERE category_id = @CategoryId AND user_id = @UserId
                """;
            await db.ExecuteAsync(sql, new { CategoryId = categoryId, UserId = userId });
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int userId)
        {
            const string sql = """
                SELECT category_id AS CategoryId,
                        name as Name,
                        is_official AS IsOfficial
                FROM Categories
                WHERE user_id = @UserId OR is_official = 1
                ORDER BY is_official DESC, name ASC
                """;
            return await db.QueryAsync<Category>(sql, new { UserId = userId });
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, int userId)
        {
            const string sql = """
                SELECT category_id AS CategoryId,
                        name as Name,
                        is_official AS IsOfficial
                FROM Categories
                WHERE (user_id = @UserId OR is_official = 1) AND category_id = @CategoryId
                """;
            return await db.QueryFirstOrDefaultAsync<Category>(sql, new { UserId = userId, CategoryId = categoryId });
        }
    }
}