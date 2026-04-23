namespace Tams.Api.Models
{
    internal class Brand
    {
        public int BrandId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsOfficial { get; set; }
    }
}