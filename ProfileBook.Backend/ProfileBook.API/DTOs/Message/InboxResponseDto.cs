namespace ProfileBook.API.DTOs.Message
{
    public class InboxResponseDto
    {
        public int ContactId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactProfileImage { get; set; }
        public string? LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int UnreadCount { get; set; } = 0; // We can implement unread later if needed
        public bool IsActive { get; set; } // For UI "Active now"
    }
}
