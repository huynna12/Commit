using Microsoft.AspNetCore.Identity;
namespace Commit.Api.Models;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public PlanTier PlanTier { get; set; } = PlanTier.Free;
    public bool IsDeleted { get; set; } = false;
}

public enum PlanTier
{
    Free,
    Plus
}

