namespace Tams.Api.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HardwareModelId { get; set; }
        public string OwnerEmail { get; set; }
        public User Owner { get; set; }
        public string? CustomBrand { get; set; }
        public string? CustomName { get; set; }
        public string? CustomSpecs { get; set; }
        public string? PurchaseDate { get; set; }
        public string? PurchasePrice { get; set; }
        public string? PurchaseCondition { get; set; }
        public int? EstimatedValue { get; set; }
    }
}