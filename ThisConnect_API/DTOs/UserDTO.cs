namespace ThisConnect_API.DTOs
{
    public class UserDTO
    {

        public string? UserId { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string LastSeenAt { get; set; } = null!;
    }
}
