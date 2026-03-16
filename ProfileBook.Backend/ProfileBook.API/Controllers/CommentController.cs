using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Comment;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentCreateDto request)
        {
            var result = await _commentService.CreateComment(request);
            return Ok(result);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            var comments = await _commentService.GetCommentsByPost(postId);
            return Ok(comments);
        }
    }
}
