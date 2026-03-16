using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Message;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // Flow is like this
        // User send message then GetConvo map this convo between the users
        // then Getinbox is to load the UI for the convo that is happening
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageCreateDto request)
        {
            var result = await _messageService.SendMessage(request);
            return Ok(result);
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation(int user1Id, int user2Id)
        {
            var messages = await _messageService.GetConversation(user1Id, user2Id);
            return Ok(messages);
        }

        [HttpGet("inbox/{userId}")]
        public async Task<IActionResult> GetInbox(int userId)
        {
            var inbox = await _messageService.GetInbox(userId);
            return Ok(inbox);
        }
    }
}
