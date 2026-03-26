namespace ProfileBook.API.DTOs.Post
{
    public class PostResponseDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
        public string Status { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
