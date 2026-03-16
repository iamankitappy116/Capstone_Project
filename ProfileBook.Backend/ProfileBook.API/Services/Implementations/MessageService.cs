using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Message;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;

        public MessageService(DataContext context)
        {
            _context = context;
        }

        public async Task<MessageResponseDto> SendMessage(MessageCreateDto request)
        {
            var senderExists = await _context.Users
                .AnyAsync(u => u.UserId == request.SenderId);

            var receiverExists = await _context.Users
                .AnyAsync(u => u.UserId == request.ReceiverId);

            if (!senderExists || !receiverExists)
                throw new Exception("Sender or Receiver not found");

            var message = new Message
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                MessageContent = request.MessageContent,
                TimeStamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Fetch with user details for response
            var savedMessage = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.MessageId == message.MessageId);

            return new MessageResponseDto
            {
                MessageId = savedMessage.MessageId,
                SenderId = savedMessage.SenderId,
                SenderName = savedMessage.Sender?.Username,
                SenderProfileImage = savedMessage.Sender?.ProfileImage,
                ReceiverId = savedMessage.ReceiverId,
                ReceiverName = savedMessage.Receiver?.Username,
                ReceiverProfileImage = savedMessage.Receiver?.ProfileImage,
                MessageContent = savedMessage.MessageContent,
                TimeStamp = savedMessage.TimeStamp
            };
        }
        public async Task<List<MessageResponseDto>> GetConversation(int user1Id, int user2Id)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m =>
                    (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                    (m.SenderId == user2Id && m.ReceiverId == user1Id)
                )
                .OrderBy(m => m.TimeStamp)
                .Select(m => new MessageResponseDto
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.Username,
                    SenderProfileImage = m.Sender.ProfileImage,
                    ReceiverId = m.ReceiverId,
                    ReceiverName = m.Receiver.Username,
                    ReceiverProfileImage = m.Receiver.ProfileImage,
                    MessageContent = m.MessageContent,
                    TimeStamp = m.TimeStamp
                })
                .ToListAsync();
        }

        public async Task<List<InboxResponseDto>> GetInbox(int userId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.TimeStamp)
                .ToListAsync();

            var inbox = messages
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => 
                {
                    var lastMessage = g.First();
                    var contact = lastMessage.SenderId == userId ? lastMessage.Receiver : lastMessage.Sender;
                    
                    return new InboxResponseDto
                    {
                        ContactId = contact?.UserId ?? 0,
                        ContactName = contact?.Username,
                        ContactProfileImage = contact?.ProfileImage,
                        LastMessage = lastMessage.MessageContent,
                        LastMessageTime = lastMessage.TimeStamp,
                        IsActive = true
                    };
                })
                .ToList();

            return inbox;
        }
    }
}
