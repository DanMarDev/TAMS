namespace Tams.Api.Models
{
    internal class WarrantyPolicy
    {
        public int WarrantyPolicyId { get; set; }
        public int BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int WarrantyTermMonths { get; set; }
        public string? Source { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}