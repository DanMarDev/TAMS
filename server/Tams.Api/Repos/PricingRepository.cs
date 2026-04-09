using Tams.Api.Models;
using Dapper;
using System.Data;

namespace Tams.Api.Repos
{
    public class PricingRepository(IDbConnection db) : IPricingRepository
    {
        public async Task<int> InsertValuationAsync(Valuation valuation)
        {
            const string sql = """
                INSERT INTO Valuations (item_id, estimated_value, source)
                OUTPUT INSERTED.valuation_id
                VALUES (@ItemId, @EstimatedValue, @Source)
                """;
            return await db.ExecuteScalarAsync<int>(sql, valuation);
        }

        public async Task<IEnumerable<Valuation>> GetValuationsByItemIdAsync(int itemId)
        {
            const string sql = """
                SELECT valuation_id AS ValuationId,
                       item_id AS ItemId,
                       estimated_value AS EstimatedValue,
                       source AS Source,
                       retrieved_at AS RetrievedAt
                FROM Valuations
                WHERE item_id = @ItemId
                ORDER BY retrieved_at DESC
                """;
            return await db.QueryAsync<Valuation>(sql, new { ItemId = itemId });
        }

        public async Task<Valuation?> GetLatestValuationByItemIdAsync(int itemId)
        {
            const string sql = """
                SELECT TOP 1
                       valuation_id AS ValuationId,
                       item_id AS ItemId,
                       estimated_value AS EstimatedValue,
                       source AS Source,
                       retrieved_at AS RetrievedAt
                FROM Valuations
                WHERE item_id = @ItemId
                ORDER BY retrieved_at DESC
                """;
            return await db.QueryFirstOrDefaultAsync<Valuation>(sql, new { ItemId = itemId });
        }
    }
}
