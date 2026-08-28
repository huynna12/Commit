namespace Commit.Api.Models.DTOs
{
    public class CreateChallengeDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public JoinPolicy JoinPolicy { get; set; } = JoinPolicy.Open;

        public int? DurationInDays { get; set; }
        public int? MaxParticipants { get; set; }
        public DayOfWeek[] ScheduleDays { get; set; } = [];
    }
}
