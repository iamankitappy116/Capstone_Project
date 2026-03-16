namespace ProfileBook.API.DTOs.Comment
{
    public class CommentResponseDto
    {
        public int CommentId { get; set; }

        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
    }
}
