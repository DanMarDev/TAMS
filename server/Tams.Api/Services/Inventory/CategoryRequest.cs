using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Inventory;

public class CategoryRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}