namespace Commit.Api.Models
{

    public enum MilestoneBadge 
    { 
        OneThird,
        Halfway,
        AlmostThere
    }
    public class Milestone
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int DateTarget { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; } = null!;
        public MilestoneBadge Badge { get; set; }

    }
}
