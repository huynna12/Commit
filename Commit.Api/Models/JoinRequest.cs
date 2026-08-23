namespace Commit.Api.Models
{
    public class JoinRequest
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public Status RequestStatus { get; set; } = Status.Pending;
        

    }

    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
}
