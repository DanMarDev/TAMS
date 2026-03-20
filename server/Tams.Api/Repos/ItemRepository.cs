using System.Data;
using Dapper;
using Tams.Api.Models;

namespace Tams.Api.Repositories
{
    public class ItemRepository(IDbConnection db) : IItemRepository
    {
        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            const string sql = """
                SELECT item_id AS ItemId,
                       user_id AS UserId,
                       category_id AS CategoryId,
                       brand_id AS BrandId,
                       name AS Name,
                       model AS Model,
                       purchase_date AS PurchaseDate,
                       purchase_price AS PurchasePrice,
                       condition AS Condition,
                       notes AS Notes,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM Items
                WHERE item_id = @ItemId
                """;
            return await db.QueryFirstOrDefaultAsync<Item>(sql, new { ItemId = itemId });
        }

        public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId)
        {
            const string sql = """
                SELECT item_id AS ItemId,
                       user_id AS UserId,
                       category_id AS CategoryId,
                       brand_id AS BrandId,
                       name AS Name,
                       model AS Model,
                       purchase_date AS PurchaseDate,
                       purchase_price AS PurchasePrice,
                       condition AS Condition,
                       notes AS Notes,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM Items
                WHERE user_id = @UserId
                """;
            return await db.QueryAsync<Item>(sql, new { UserId = userId });
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            const string sql = """
                INSERT INTO Items (user_id, category_id, brand_id, name, model,
                                   purchase_date, purchase_price, condition, notes)
                OUTPUT INSERTED.item_id
                VALUES (@UserId, @CategoryId, @BrandId, @Name, @Model,
                        @PurchaseDate, @PurchasePrice, @Condition, @Notes)
                """;
            return await db.ExecuteScalarAsync<int>(sql, item);
        }

        public async Task UpdateItemAsync(Item item)
        {
            const string sql = """
                UPDATE Items
                SET category_id = @CategoryId,
                    brand_id = @BrandId,
                    name = @Name,
                    model = @Model,
                    purchase_date = @PurchaseDate,
                    purchase_price = @PurchasePrice,
                    condition = @Condition,
                    notes = @Notes,
                    updated_at = GETDATE()
                WHERE item_id = @ItemId
                """;
            await db.ExecuteAsync(sql, item);
        }

        public async Task DeleteItemAsync(int itemId)
        {
            const string sql = """
                DELETE FROM Items
                WHERE item_id = @ItemId
                """;
            await db.ExecuteAsync(sql, new { ItemId = itemId });
        }
    }
}