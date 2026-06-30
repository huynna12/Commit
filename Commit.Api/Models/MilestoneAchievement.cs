namespace Commit.Api.Models
{
    public class MilestoneAchievement
    {
        public string AppUserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public DateTime AchievedAt { get; set; }
        public int MilestoneId { get; set; }
        public Milestone Milestone { get; set; } = null!;
    }
}
