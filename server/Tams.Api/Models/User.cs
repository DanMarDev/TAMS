namespace Tams.Api.Models
{
    internal class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal DefaultSellThreshold { get; set; } = 50.00m;
        public DateTime CreatedAt { get; set; }
    }
}
