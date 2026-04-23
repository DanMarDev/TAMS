namespace Tams.Api.Models
{
    internal class ItemWarranty
    {
        public int ItemWarrantyId { get; set; }
        public int ItemId { get; set; }
        public int? WarrantyPolicyId { get; set; }
        public DateOnly? WarrantyStartDate { get; set; }
        public DateOnly? WarrantyEndDate { get; set; }
        public bool IsManualEntry { get; set; } = true;
        public string? Notes { get; set; }
    }
}