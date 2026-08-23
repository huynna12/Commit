namespace Commit.Api.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? DurationInDays { get; set; }
        public int? MaxParticipants { get; set; }
        public DateTime StartDate { get; set; }
        public string? Description { get; set; }
        public string OwnerId { get; set; } = null!;
        public AppUser Owner { get; set; } = null!;
        public ICollection<ChallengeParticipant> Participants { get; set; } = new List<ChallengeParticipant>();
        public DayOfWeek[] ScheduleDays { get; set; } = [];
        public JoinPolicy JoinPolicy { get; set; } = JoinPolicy.Open;

    }
    public enum JoinPolicy
    {
        Open,
        InviteOnly,
        RequiresApproval,
    }
}
