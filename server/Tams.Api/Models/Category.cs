namespace Tams.Api.Models
{
    internal sealed class Category
    {
        public int CategoryId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOfficial { get; set; }
    }
}