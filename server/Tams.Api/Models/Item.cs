namespace Tams.Api.Models
{
    internal class Item
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Model { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal MaybeSellThreshold { get; set; } = 50.00m;
        public decimal? OriginalValue { get; set; }
        public string Condition { get; set; } = ItemConditions.Good;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}