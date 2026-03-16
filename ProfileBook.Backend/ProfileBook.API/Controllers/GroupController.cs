using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Group;
using ProfileBook.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProfileBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateGroup(GroupCreateDto request)
    {
        var result = await _groupService.CreateGroup(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _groupService.GetGroups();

        return Ok(groups);
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinGroup(GroupJoinDto request)
    {
        var joined = await _groupService.JoinGroup(request);

        if (!joined)
            return BadRequest("User already in group");

        return Ok("Joined group successfully");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserGroups(int userId)
    {
        var groups = await _groupService.GetUserGroups(userId);

        return Ok(groups);
    }
}
