using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Inventory;

public class ItemRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    public int? BrandId { get; set; }

    [MaxLength(255)]
    public string? Model { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal MaybeSellThreshold { get; set; } = 50.00m;

    public decimal? OriginalValue { get; set; }

    public string Condition { get; set; } = "Good";

    [MaxLength(1000)]
    public string? Notes { get; set; }
}