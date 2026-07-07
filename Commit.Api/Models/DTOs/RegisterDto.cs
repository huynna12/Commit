namespace Commit.Api.Models.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? DisplayName { get; set; }
    }
}
