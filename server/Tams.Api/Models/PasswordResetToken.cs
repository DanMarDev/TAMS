namespace Tams.Api.Models
{
    internal sealed class PasswordResetToken
    {
        public int ResetTokenId { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}