using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Comment;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;

        public CommentService(DataContext context)
        {
            _context = context;
        }

        public async Task<CommentResponseDto> CreateComment(CommentCreateDto request)
        {
            var comment = new Comment
            {
                CommentText = request.CommentText,
                UserId = request.UserId,
                PostId = request.PostId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                PostId = comment.PostId,
                UserName = await _context.Users.Where(u => u.UserId == comment.UserId).Select(u => u.Username).FirstOrDefaultAsync(),
                ProfileImage = await _context.Users.Where(u => u.UserId == comment.UserId).Select(u => u.ProfileImage).FirstOrDefaultAsync()
            };
        }

        public async Task<List<CommentResponseDto>> GetCommentsByPost(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Select(c => new CommentResponseDto
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    UserName = c.User.Username,
                    ProfileImage = c.User.ProfileImage
                })
                .ToListAsync();
        }
    }
}
