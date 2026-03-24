namespace ProfileBook.API.Services.Interfaces;

public interface IFollowService
{
    Task<bool> Follow(int followerId, int followingId);
    Task<bool> Unfollow(int followerId, int followingId);
    Task<bool> IsFollowing(int followerId, int followingId);
}
