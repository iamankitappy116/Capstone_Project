namespace ProfileBook.API.DTOs.Message
{
    public class MessageResponseDto
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderProfileImage { get; set; }

        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverProfileImage { get; set; }

        public string MessageContent { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }
    }
}
