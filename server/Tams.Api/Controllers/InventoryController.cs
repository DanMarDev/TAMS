using Microsoft.AspNetCore.Mvc;
using Tams.Api.Services.Inventory;

namespace Tams.Api.Controllers;

public sealed class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    // TOOD: Implement InventoryController with endpoints for managing items, brands, categories, and valuations. 
    // Use the IInventoryService to perform the necessary operations for each endpoint. Ensure that all endpoints are secured and only accessible to authenticated users. Consider implementing additional endpoints for inventory analytics and maybe sell candidate analysis in the future.
}