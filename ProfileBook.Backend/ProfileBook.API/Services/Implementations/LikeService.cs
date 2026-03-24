using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Like;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly DataContext _context;

        public LikeService(DataContext context)
        {
            _context = context;
        }

        public async Task<LikeResponseDto> AddLike(LikeCreateDto request)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.PostId == request.PostId);

            if (existingLike != null)
                throw new Exception("User already liked this post.");

            var like = new Like
            {
                UserId = request.UserId,
                PostId = request.PostId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return new LikeResponseDto
            {
                LikeId = like.LikeId,
                UserId = like.UserId,
                PostId = like.PostId
            };
        }

        public async Task<bool> RemoveLike(int userId, int postId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            if (like == null)
                return false;

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetLikesCount(int postId)
        {
            return await _context.Likes
                .CountAsync(l => l.PostId == postId);
        }

        public async Task<bool> IsPostLikedByUser(int userId, int postId)
        {
            return await _context.Likes
                .AnyAsync(l => l.UserId == userId && l.PostId == postId);
        }
    }
}
