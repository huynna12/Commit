namespace Commit.Api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Caption { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CheckInId { get; set; }
        public CheckIn CheckIn { get; set; } = null!;
    }
}
