using Tams.Api.Models;

namespace Tams.Api.Services.Pricing
{
    public interface IPricingService
    {
        Task<Valuation> GenerateAutomatedEstimateAsync(int itemId, int userId);
        Task<Valuation> SubmitManualValuationAsync(int itemId, decimal value, int userId);
        Task<Valuation?> GetLatestValuationAsync(int itemId, int userId);
        Task<IEnumerable<Valuation>> GetPriceHistoryAsync(int itemId, int userId);
    }
}