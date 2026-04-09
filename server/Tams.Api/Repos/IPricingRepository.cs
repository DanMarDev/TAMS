using Tams.Api.Models;

namespace Tams.Api.Repos
{
    public interface IPricingRepository
    {
        Task<int> InsertValuationAsync(Valuation valuation);
        Task<IEnumerable<Valuation>> GetValuationsByItemIdAsync(int itemId);
        Task<Valuation?> GetLatestValuationByItemIdAsync(int itemId);
    }
}