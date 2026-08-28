namespace Commit.Api.Configuration
{
    public class PlanLimitsOptions
    {
        public PlanLimit Free { get; set; } = new();
        public PlanLimit Plus { get; set; } = new();

    }
    public class PlanLimit
    {
        public int? MaxActiveChallengesJoined { get; set; }
        public int? MaxParticipantsPerChallenge { get; set; }
        public int? MaxActiveChallengesCreated { get; set; }
        public bool AllowOpenEnded { get; set; }
    }
}
