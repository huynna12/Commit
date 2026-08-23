using Microsoft.EntityFrameworkCore;
using Commit.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Commit.Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ChallengeParticipant> ChallengeParticipants { get; set; }
        public DbSet<MilestoneAchievement> MilestoneAchievements { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }

        // Override the OnModelCreating method to configure
        // composite keys and relationships
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChallengeParticipant>(entity =>
            {
                entity.HasKey(cp => new { cp.ChallengeId, cp.AppUserId });


                entity.HasOne(cp => cp.Challenge)
                    .WithMany(c => c.Participants)
                    .OnDelete(DeleteBehavior.Cascade);

              // Try to delete AppUser
              //→ SQL Server checks: does this user have ChallengeParticipant rows?
              //  → Yes → ERROR, deletion blocked, nothing happens
              //  → No  → user gets deleted
                entity.HasOne(cp => cp.User)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MilestoneAchievement>(entity =>
            {
                entity.HasKey(ma => new { ma.MilestoneId, ma.AppUserId });

                entity.HasOne(ma => ma.Milestone)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ma => ma.User)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Post>()
                .HasIndex(p => p.CheckInId)
                .IsUnique();

            builder.Entity<CheckIn>(entity =>
            {
                entity.HasIndex(checkIn => new { checkIn.ChallengeId, checkIn.AppUserId, checkIn.CheckInDate })
                    .IsUnique();

                entity.HasOne(c => c.Challenge)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.AppUser)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<JoinRequest>(entity =>
            {
                entity.HasOne(jr => jr.Challenge)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(jr => jr.AppUser)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
