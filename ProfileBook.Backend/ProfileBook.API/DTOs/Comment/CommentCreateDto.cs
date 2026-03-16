namespace ProfileBook.API.DTOs.Comment
{
    public class CommentCreateDto
    {
        public string CommentText { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
