
namespace Commit.Api.Models
{
    public class CheckIn
    {
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; } = null!;
        public DateTime CheckInDate { get; set; } = DateTime.UtcNow;
    }
}
