namespace ProfileBook.API.DTOs.Post
{
    public class PostCreateDto
    {
        public string Content { get; set; } = string.Empty;

        public string? MediaUrl { get; set; }

        public string? MediaType { get; set; }

        public int UserId { get; set; }
    }
}
