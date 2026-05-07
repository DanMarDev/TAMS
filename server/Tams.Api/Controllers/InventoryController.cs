using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tams.Api.Services.Inventory;
using Tams.Api.Models;

namespace Tams.Api.Controllers;



/// <summary>
/// Controller for handling inventory-related endpoints such as managing items, brands, categories, and inventory analytics.
/// This controller uses the IInventoryService to perform the necessary operations for each endpoint. All endpoints should be
/// secured and only accessible to authenticated users.
/// </summary>
/// <param name="inventoryService"></param>
[ApiController]
[Route("api/inventory")]
[Authorize]
public sealed class InventoryController(IInventoryService inventoryService) : AppControllerBase
{
    // ====== Dashboard ======

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var dashboard = await inventoryService.GetDashboardAsync(GetUserIdFromClaims());
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    // ====== Item CRUD ======

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        try
        {
            var items = await inventoryService.GetItemsByUserIdAsync(GetUserIdFromClaims());
            return Ok(items);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetItem([FromRoute] int itemId)
    {
        try
        {
            var item = await inventoryService.GetItemByIdAsync(itemId, GetUserIdFromClaims());
            if (item is null) return NotFound("Item not found.");
            return Ok(item);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] ItemRequest request)
    {
        try
        {
            var newItem = new Item
            {
                UserId = GetUserIdFromClaims(),
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                Name = request.Name,
                Model = request.Model,
                PurchaseDate = request.PurchaseDate,
                PurchasePrice = request.PurchasePrice,
                MaybeSellThreshold = request.MaybeSellThreshold,
                OriginalValue = request.OriginalValue,
                Condition = request.Condition,
                Notes = request.Notes
            };

            var itemId = await inventoryService.CreateItemAsync(newItem);
            return CreatedAtAction(nameof(GetItem), new { itemId }, new { ItemId = itemId });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> UpdateItem([FromBody] ItemRequest request, [FromRoute] int itemId)
    {
        try
        {
            var updatedItem = new Item
            {
                ItemId = itemId,
                UserId = GetUserIdFromClaims(),
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                Name = request.Name,
                Model = request.Model,
                PurchaseDate = request.PurchaseDate,
                PurchasePrice = request.PurchasePrice,
                MaybeSellThreshold = request.MaybeSellThreshold,
                OriginalValue = request.OriginalValue,
                Condition = request.Condition,
                Notes = request.Notes
            };
            var ok = await inventoryService.UpdateItemAsync(updatedItem, GetUserIdFromClaims());
            if (!ok) return NotFound("Item not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
    {
        try
        {
            var ok = await inventoryService.DeleteItemAsync(itemId, GetUserIdFromClaims());
            if (!ok) return NotFound("Item not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    // ====== Category CRUD ======

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var categories = await inventoryService.GetCategoriesAsync(GetUserIdFromClaims());
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetCategory([FromRoute] int id)
    {
        try
        {
            var category = await inventoryService.GetCategoryByIdAsync(id, GetUserIdFromClaims());
            if (category is null) return NotFound("Category not found.");
            return Ok(category);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        try
        {
            var newCategory = new Category
            {
                UserId = GetUserIdFromClaims(),
                Name = request.Name,
                Description = request.Description
            };

            var categoryId = await inventoryService.CreateCategoryAsync(newCategory);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryId }, new { CategoryId = categoryId });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("categories/{id}")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request, [FromRoute] int id)
    {
        try
        {
            var updatedCategory = new Category
            {
                CategoryId = id,
                UserId = GetUserIdFromClaims(),
                Name = request.Name,
                Description = request.Description
            };
            var ok = await inventoryService.UpdateCategoryAsync(updatedCategory, GetUserIdFromClaims());
            if (!ok) return NotFound("Category not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        try
        {
            var ok = await inventoryService.DeleteCategoryAsync(id, GetUserIdFromClaims());
            if (!ok) return NotFound("Category not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    // ====== Brand CRUD ======

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        try
        {
            var brands = await inventoryService.GetBrandsAsync(GetUserIdFromClaims());
            return Ok(brands);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("brands/{id}")]
    public async Task<IActionResult> GetBrand([FromRoute] int id)
    {
        try
        {
            var brand = await inventoryService.GetBrandByIdAsync(id, GetUserIdFromClaims());
            if (brand is null) return NotFound("Brand not found.");
            return Ok(brand);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("brands")]
    public async Task<IActionResult> CreateBrand([FromBody] BrandRequest request)
    {
        try
        {
            var newBrand = new Brand
            {
                UserId = GetUserIdFromClaims(),
                Name = request.Name
            };
            var brandId = await inventoryService.CreateBrandAsync(newBrand);
            return CreatedAtAction(nameof(GetBrand), new { id = brandId }, new { BrandId = brandId });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPut("brands/{id}")]
    public async Task<IActionResult> UpdateBrand([FromBody] BrandRequest request, [FromRoute] int id)
    {
        try
        {
            var updatedBrand = new Brand
            {
                BrandId = id,
                UserId = GetUserIdFromClaims(),
                Name = request.Name
            };
            var ok = await inventoryService.UpdateBrandAsync(updatedBrand, GetUserIdFromClaims());
            if (!ok) return NotFound("Brand not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpDelete("brands/{id}")]
    public async Task<IActionResult> DeleteBrand([FromRoute] int id)
    {
        try
        {
            var ok = await inventoryService.DeleteBrandAsync(id, GetUserIdFromClaims());
            if (!ok) return NotFound("Brand not found.");
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
