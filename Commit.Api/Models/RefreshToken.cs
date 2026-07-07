namespace Commit.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }
}