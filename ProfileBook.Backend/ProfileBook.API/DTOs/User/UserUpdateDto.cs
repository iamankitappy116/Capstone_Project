namespace ProfileBook.API.DTOs.User
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }
        
        public string? Bio { get; set; }

        public string? Location { get; set; }
    }
}