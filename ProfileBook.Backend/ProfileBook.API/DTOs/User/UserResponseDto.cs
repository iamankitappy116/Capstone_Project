namespace ProfileBook.API.DTOs.User
{
    public class UserResponseDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }

        public string? Role { get; set; }

        public string? Bio { get; set; }

        public string? Location { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
