namespace Tams.Api.Models
{
    public class Item
    {
        /// <summary>
        /// Note: ItemId is the only property that is not user-generated. It is an identity column
        /// in the database and is assigned by the system upon item creation. All other properties are 
        /// expected to be provided by the user when creating or updating an item.
        /// </summary>
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