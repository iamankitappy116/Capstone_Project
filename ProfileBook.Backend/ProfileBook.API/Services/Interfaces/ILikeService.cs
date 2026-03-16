using ProfileBook.API.DTOs.Like;

namespace ProfileBook.API.Services.Interfaces
{
    public interface ILikeService
    {
        Task<LikeResponseDto> AddLike(LikeCreateDto request);

        Task<bool> RemoveLike(int userId, int postId);

        Task<int> GetLikesCount(int postId);
    }
}
