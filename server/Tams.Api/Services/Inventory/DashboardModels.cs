namespace Tams.Api.Services.Inventory
{
    public class DashboardItem
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Model { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal MaybeSellThreshold { get; set; }
        public string? Condition { get; set; }
        public decimal? LatestEstimatedValue { get; set; }
        public DateTime? LatestValuationAt { get; set; }
    }

    public class DashboardSummary
    {
        public int TotalItems { get; set; }
        public decimal TotalEstimatedValue { get; set; }
        public int MaybeSellCount { get; set; }
        public int ExpiringWarrantyCount { get; set; }
    }

    public class DashboardResponse
    {
        public DashboardSummary Summary { get; set; } = new();
        public List<DashboardItem> OldestItems { get; set; } = new();
        public List<DashboardItem> MaybeSellItems { get; set; } = new();
    }
}
