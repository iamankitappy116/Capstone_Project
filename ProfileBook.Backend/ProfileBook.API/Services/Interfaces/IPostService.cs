using ProfileBook.API.DTOs.Post;

namespace ProfileBook.API.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostResponseDto> CreatePost(PostCreateDto request);
        Task<List<PostResponseDto>> GetPosts();
        Task<List<PostResponseDto>> GetPostsByUser(int userId);
        Task<List<PostResponseDto>> GetPendingPosts();
        Task<PostApprovalDto> ApprovePost(int id);
        Task<PostApprovalDto> RejectPost(int id);

    }
}
