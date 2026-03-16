namespace ProfileBook.API.DTOs.Message
{
    public class MessageCreateDto
    {
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string MessageContent { get; set; } = string.Empty;
    }
}
