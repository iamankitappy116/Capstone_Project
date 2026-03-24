using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations;

public class FollowService : IFollowService
{
    private readonly DataContext _context;

    public FollowService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Follow(int followerId, int followingId)
    {
        if (followerId == followingId) return false;

        var existing = await _context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        if (existing != null) return false; // already following

        _context.UserFollows.Add(new UserFollow
        {
            FollowerId = followerId,
            FollowingId = followingId
        });

        // Increment counters
        var follower = await _context.Users.FindAsync(followerId);
        var following = await _context.Users.FindAsync(followingId);

        if (follower != null) follower.FollowingCount++;
        if (following != null) following.FollowerCount++;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Unfollow(int followerId, int followingId)
    {
        var follow = await _context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        if (follow == null) return false;

        _context.UserFollows.Remove(follow);

        // Decrement counters (floor at 0)
        var follower = await _context.Users.FindAsync(followerId);
        var following = await _context.Users.FindAsync(followingId);

        if (follower != null && follower.FollowingCount > 0) follower.FollowingCount--;
        if (following != null && following.FollowerCount > 0) following.FollowerCount--;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsFollowing(int followerId, int followingId)
    {
        return await _context.UserFollows
            .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
    }
}
