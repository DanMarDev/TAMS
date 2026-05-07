using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Pricing;
using Microsoft.AspNetCore.Authorization;

namespace Tams.Api.Controllers;

[ApiController]
[Route("api/pricing")]
[Authorize]
public sealed class PricingController(IPricingService pricingService) : AppControllerBase
{
    [HttpGet("estimate/{itemId}")]
    public async Task<IActionResult> GenerateAutomatedEstimateAsync([FromRoute] int itemId)
    {
        try
        {
            var valuation = await pricingService.GenerateAutomatedEstimateAsync(itemId, GetUserIdFromClaims());
            return CreatedAtAction(nameof(GetLatestValuationAsync), new { itemId }, valuation);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("latest/{itemId}")]
    public async Task<IActionResult> GetLatestValuationAsync([FromRoute] int itemId)
    {
        try
        {
            var valuation = await pricingService.GetLatestValuationAsync(itemId, GetUserIdFromClaims());
            if (valuation == null)
            {
                return NotFound("No valuation found for the given item and user.");
            }
            return Ok(valuation);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("history/{itemId}")]
    public async Task<IActionResult> GetPriceHistoryAsync([FromRoute] int itemId)
    {
        try
        {
            var valuationHistory = await pricingService.GetPriceHistoryAsync(itemId, GetUserIdFromClaims());
            return Ok(valuationHistory);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("manual/{itemId}")]
    public async Task<IActionResult> SubmitManualValuationAsync([FromRoute] int itemId, [FromBody] ManualValuationRequest request)
    {
        try
        {
            var valuation = await pricingService.SubmitManualValuationAsync(itemId, request.Value, GetUserIdFromClaims());
            return CreatedAtAction(nameof(GetLatestValuationAsync), new { itemId }, valuation);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
