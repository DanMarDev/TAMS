namespace Tams.Api.Models
{
    public class WarrantyPolicy
    {
        public int WarrantyID { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int DefaultMonths { get; set; }
        public string Region { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}