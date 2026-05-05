using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Inventory;

public class BrandRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
}