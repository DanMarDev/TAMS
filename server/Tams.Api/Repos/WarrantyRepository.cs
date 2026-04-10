using Tams.Api.Models;
using Dapper;
using System.Data;

namespace Tams.Api.Repos
{
    internal class WarrantyRepository(IDbConnection db) : IWarrantyRepository
    {
        public async Task<ItemWarranty?> GetWarrantyByItemIdAsync(int itemId)
        {
            const string sql = """
                SELECT item_warranty_id AS ItemWarrantyId,
                       item_id AS ItemId,
                       warranty_policy_id AS WarrantyPolicyId,
                       warranty_start_date AS WarrantyStartDate,
                       warranty_end_date AS WarrantyEndDate,
                       is_manual_entry AS IsManualEntry,
                       notes AS Notes
                FROM ItemWarranties
                WHERE item_id = @ItemId
                """;
            return await db.QuerySingleOrDefaultAsync<ItemWarranty>(sql, new { ItemId = itemId });
        }

        public async Task<IEnumerable<ItemWarranty>> GetExpiringWarrantiesAsync(int userId, int daysAhead)
        {
            const string sql = """
                SELECT iw.item_warranty_id AS ItemWarrantyId,
                       iw.item_id AS ItemId,
                       iw.warranty_policy_id AS WarrantyPolicyId,
                       iw.warranty_start_date AS WarrantyStartDate,
                       iw.warranty_end_date AS WarrantyEndDate,
                       iw.is_manual_entry AS IsManualEntry,
                       iw.notes AS Notes
                FROM ItemWarranties iw
                JOIN Items i ON iw.item_id = i.item_id
                WHERE i.user_id = @UserId
                  AND iw.warranty_end_date BETWEEN CAST(GETDATE() AS DATE) AND CAST(DATEADD(DAY, @DaysAhead, GETDATE()) AS DATE)
                """;
            return await db.QueryAsync<ItemWarranty>(sql, new { UserId = userId, DaysAhead = daysAhead });
        }

        public async Task<IEnumerable<WarrantyAlert>> GetAlertsByUserIdAsync(int userId)
        {
            const string sql = """
                SELECT alert_id AS AlertId,
                       user_id AS UserId,
                       item_id AS ItemId,
                       alert_type AS AlertType,
                       created_at AS CreatedAt,
                       dismissed_at AS DismissedAt
                FROM WarrantyAlerts
                WHERE user_id = @UserId
                  AND dismissed_at IS NULL
                ORDER BY created_at DESC
                """;
            return await db.QueryAsync<WarrantyAlert>(sql, new { UserId = userId });
        }

        public async Task<WarrantyPolicy?> GetPolicyByBrandAndCategoryAsync(int brandId, int categoryId)
        {
            const string sql = """
                SELECT warranty_policy_id AS WarrantyPolicyId,
                       brand_id AS BrandId,
                       category_id AS CategoryId,
                       warranty_term_months AS WarrantyTermMonths,
                       source AS Source,
                       created_at AS CreatedAt
                FROM WarrantyPolicies
                WHERE brand_id = @BrandId AND category_id = @CategoryId
                """;
            return await db.QuerySingleOrDefaultAsync<WarrantyPolicy>(sql, new { BrandId = brandId, CategoryId = categoryId });
        }

        public async Task<int> UpsertWarrantyAsync(ItemWarranty warranty)
        {
            const string sql = """
                IF EXISTS (SELECT 1 FROM ItemWarranties WHERE item_id = @ItemId)
                BEGIN
                    UPDATE ItemWarranties
                    SET warranty_policy_id  = @WarrantyPolicyId,
                        warranty_start_date = @WarrantyStartDate,
                        warranty_end_date   = @WarrantyEndDate,
                        is_manual_entry     = @IsManualEntry,
                        notes               = @Notes
                    WHERE item_id = @ItemId;
                    SELECT item_warranty_id FROM ItemWarranties WHERE item_id = @ItemId
                END
                ELSE
                BEGIN
                    INSERT INTO ItemWarranties (item_id, warranty_policy_id, warranty_start_date, warranty_end_date, is_manual_entry, notes)
                    OUTPUT INSERTED.item_warranty_id
                    VALUES (@ItemId, @WarrantyPolicyId, @WarrantyStartDate, @WarrantyEndDate, @IsManualEntry, @Notes)
                END
                """;
            return await db.ExecuteScalarAsync<int>(sql, warranty);
        }
    }
}
