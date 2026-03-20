namespace Tams.Api.Models
{
    public class WarrantyAlert
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string AlertType { get; set; } = WarrantyAlertTypes.Expiring30d;
        public DateTime CreatedAt { get; set; }
        public DateTime? DismissedAt { get; set; }
    }

    public static class WarrantyAlertTypes
    {
        public const string Expiring30d = "warranty_expiring_30d";
        public const string Expiring7d = "warranty_expiring_7d";
        public const string Expired = "warranty_expired";
    }
}