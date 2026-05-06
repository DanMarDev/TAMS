using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Warranty;

namespace Tams.Api.Controllers;

[ApiController]
[Route("api/warranty")]
[Authorize]
public sealed class WarrantyController(IWarrantyService warrantyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWarranties()
    {
        try
        {
            // Change 1 to GetUserIdFromClaims() from InventoryController
            var warranties = await warrantyService.GetWarrantiesByUserIdAsync(1);
            return Ok(warranties);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
     }
}