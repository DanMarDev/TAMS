using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Pricing;

internal class PricingService(
    IPricingRepository pricingRepository,
    IItemRepository itemRepository
) : IPricingService
{
    public async Task<Valuation> GenerateAutomatedEstimateAsync(int itemId, int userId)
    {
        var existingItem = await itemRepository.GetItemAsync(itemId, userId);
        if (existingItem == null)
        {
            throw new KeyNotFoundException("Item not found.");
        }
        // For demonstration, we'll return a dummy valuation. In a real implementation, this would call an external API or use a pricing algorithm.
        int randomEstimatedValue = new Random().Next(20, 500); // Random value between $20 and $500
        var valuation = new Valuation
        {
            ItemId = itemId,
            EstimatedValue = randomEstimatedValue, // Dummy value
            Source = ValuationSources.Ebay,
            RetrievedAt = DateTime.UtcNow
        };
        var insertedValuationId = await pricingRepository.InsertValuationAsync(valuation);
        valuation.ValuationId = insertedValuationId;
        return valuation;
    }

    public async Task<Valuation> SubmitManualValuationAsync(int itemId, decimal value, int userId)
    {
        var existingItem = await itemRepository.GetItemAsync(itemId, userId);
        if (existingItem == null)
        {
            throw new KeyNotFoundException("Item not found.");
        }
        var valuation = new Valuation
        {
            ItemId = itemId,
            EstimatedValue = value,
            Source = ValuationSources.Manual,
            RetrievedAt = DateTime.UtcNow
        };
        var insertedValuationId = await pricingRepository.InsertValuationAsync(valuation);
        valuation.ValuationId = insertedValuationId;
        return valuation;
    }

    public async Task<Valuation?> GetLatestValuationAsync(int itemId, int userId)
    {
        var existingItem = await itemRepository.GetItemAsync(itemId, userId);
        if (existingItem == null)
        {
            throw new KeyNotFoundException("Item not found.");
        }
        var existingValuation = await pricingRepository.GetLatestValuationByItemIdAsync(itemId);
        return existingValuation;
    }

    public async Task<IEnumerable<Valuation>> GetPriceHistoryAsync(int itemId, int userId)
    {
        var existingItem = await itemRepository.GetItemAsync(itemId, userId);
        if (existingItem == null)
        {
            throw new KeyNotFoundException("Item not found.");
        }
        return await pricingRepository.GetValuationsByItemIdAsync(itemId);
    }
}