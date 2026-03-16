using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Group;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;
using System.Linq;

namespace ProfileBook.API.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly DataContext _context;

    public GroupService(DataContext context)
    {
        _context = context;
    }

    public async Task<GroupResponseDto> CreateGroup(GroupCreateDto request)
    {
        var group = new Group
        {
            GroupName = request.GroupName,
            Description = request.Description,
            Category = request.Category,
            CreatedByUserId = request.CreatedByUserId
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        // Add creator as member
        _context.GroupMembers.Add(new GroupMember { GroupId = group.GroupId, UserId = request.CreatedByUserId });

        // Add selected members
        if (request.MemberIds != null && request.MemberIds.Any())
        {
            foreach (var userId in request.MemberIds.Where(id => id != request.CreatedByUserId))
            {
                _context.GroupMembers.Add(new GroupMember { GroupId = group.GroupId, UserId = userId });
            }
        }

        await _context.SaveChangesAsync();

        return new GroupResponseDto
        {
            GroupId = group.GroupId,
            GroupName = group.GroupName,
            Description = group.Description,
            Category = group.Category,
            CreatedByUserId = group.CreatedByUserId,
            MemberCount = 0,
            PostCount = 0
        };
    }

    public async Task<List<GroupResponseDto>> GetGroups()
    {
        return await _context.Groups
            .Select(g => new GroupResponseDto
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName,
                Description = g.Description,
                Category = g.Category,
                MemberCount = g.Members.Count,
                PostCount = _context.Posts.Count(p => p.GroupId == g.GroupId),
                CreatedByUserId = g.CreatedByUserId
            })
            .ToListAsync();
    }

    public async Task<bool> JoinGroup(GroupJoinDto request)
    {
        var exists = await _context.GroupMembers
            .AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId == request.UserId);

        if (exists)
            return false;

        var member = new GroupMember
        {
            GroupId = request.GroupId,
            UserId = request.UserId
        };

        _context.GroupMembers.Add(member);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<GroupResponseDto>> GetUserGroups(int userId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.UserId == userId)
            .Select(gm => new GroupResponseDto
            {
                GroupId = gm.Group!.GroupId,
                GroupName = gm.Group.GroupName,
                Description = gm.Group.Description,
                Category = gm.Group.Category,
                MemberCount = gm.Group.Members.Count,
                PostCount = _context.Posts.Count(p => p.GroupId == gm.GroupId),
                CreatedByUserId = gm.Group.CreatedByUserId
            })
            .ToListAsync();
    }
}