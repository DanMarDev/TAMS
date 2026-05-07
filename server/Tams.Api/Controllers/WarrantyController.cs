using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Warranty;

namespace Tams.Api.Controllers;

[ApiController]
[Route("api/warranty")]
[Authorize]
public sealed class WarrantyController(IWarrantyService warrantyService) : AppControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWarranties()
    {
        try
        {
            var warranties = await warrantyService.GetWarrantiesByUserIdAsync(GetUserIdFromClaims());
            return Ok(warranties);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetWarrantyByItemId([FromRoute] int itemId)
    {
        try
        {
            var warranty = await warrantyService.GetWarrantyByItemIdAsync(itemId, GetUserIdFromClaims());
            if (warranty == null)
            {
                return NotFound("Warranty not found for the given item and user.");
            }
            return Ok(warranty);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveWarranty([FromBody] WarrantyRequest request)
    {
        try
        {
            if (request.WarrantyEndDate == null && (request.TermMonths == null || request.WarrantyStartDate == null))
                return BadRequest("Either WarrantyEndDate or both TermMonths and WarrantyStartDate are required.");

            int newWarrantyId = await warrantyService.SaveWarrantyAsync(request, null, GetUserIdFromClaims());
            return CreatedAtAction(nameof(GetWarrantyByItemId), new { itemId = request.ItemId }, new { WarrantyId = newWarrantyId });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("{warrantyId}")]
    public async Task<IActionResult> UpdateWarranty([FromBody] WarrantyRequest request, [FromRoute] int warrantyId)
    {
        try
        {
            if (request.WarrantyEndDate == null && (request.TermMonths == null || request.WarrantyStartDate == null))
                return BadRequest("Either WarrantyEndDate or both TermMonths and WarrantyStartDate are required.");

            await warrantyService.SaveWarrantyAsync(request, warrantyId, GetUserIdFromClaims());
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("{warrantyId}")]
    public async Task<IActionResult> DeleteWarranty([FromRoute] int warrantyId, [FromQuery] int itemId)
    {
        try
        {
            await warrantyService.DeleteWarrantyAsync(warrantyId, GetUserIdFromClaims(), itemId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiringWarranties()
    {
        try
        {
            var expiringWarranties = await warrantyService.GetExpiringWarrantiesAsync(GetUserIdFromClaims());
            return Ok(expiringWarranties);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetActiveAlerts()
    {
        try
        {
            var activeAlerts = await warrantyService.GetActiveAlertsAsync(GetUserIdFromClaims());
            return Ok(activeAlerts);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("alerts/{alertId}")]
    public async Task<IActionResult> DismissAlert([FromRoute] int alertId)
    {
        try
        {
            await warrantyService.DismissAlertAsync(alertId, GetUserIdFromClaims());
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
