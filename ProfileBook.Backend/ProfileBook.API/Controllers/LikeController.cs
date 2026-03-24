using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Like;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(LikeCreateDto request)
        {
            try
            {
                var result = await _likeService.AddLike(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveLike([FromQuery] int userId, [FromQuery] int postId)
        {
            var removed = await _likeService.RemoveLike(userId, postId);

            if (!removed)
                return NotFound("Like not found");

            return Ok("Like removed");
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetLikesCount(int postId)
        {
            var count = await _likeService.GetLikesCount(postId);

            return Ok(new { likes = count });
        }

        [HttpGet]
        public async Task<IActionResult> IsLiked([FromQuery] int userId, [FromQuery] int postId)
        {
            var liked = await _likeService.IsPostLikedByUser(userId, postId);
            return Ok(new { isLiked = liked });
        }
    }
}
