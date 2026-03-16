using ProfileBook.API.DTOs.Message;

namespace ProfileBook.API.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageResponseDto> SendMessage(MessageCreateDto request);

        Task<List<MessageResponseDto>> GetConversation(int user1Id, int user2Id);

        Task<List<InboxResponseDto>> GetInbox(int userId);
    }
}
