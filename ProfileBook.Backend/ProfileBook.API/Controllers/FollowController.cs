using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Follow;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpPost]
    public async Task<IActionResult> Follow([FromBody] FollowCreateDto request)
    {
        if (request.FollowerId == request.FollowingId)
            return BadRequest("Cannot follow yourself.");

        var result = await _followService.Follow(request.FollowerId, request.FollowingId);

        if (!result)
            return BadRequest("Already following this user.");

        return Ok(new { message = "Followed successfully." });
    }

    [HttpDelete]
    public async Task<IActionResult> Unfollow([FromQuery] int followerId, [FromQuery] int followingId)
    {
        var result = await _followService.Unfollow(followerId, followingId);

        if (!result)
            return NotFound("Follow relationship not found.");

        return Ok(new { message = "Unfollowed successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> IsFollowing([FromQuery] int followerId, [FromQuery] int followingId)
    {
        var isFollowing = await _followService.IsFollowing(followerId, followingId);
        return Ok(new { isFollowing });
    }
}
