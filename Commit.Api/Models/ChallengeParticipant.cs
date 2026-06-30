namespace Commit.Api.Models
{
    public class ChallengeParticipant
    {
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
