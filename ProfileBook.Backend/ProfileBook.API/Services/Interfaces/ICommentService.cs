using ProfileBook.API.DTOs.Comment;

namespace ProfileBook.API.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponseDto> CreateComment(CommentCreateDto request);

        Task<List<CommentResponseDto>> GetCommentsByPost(int postId);
    }
}