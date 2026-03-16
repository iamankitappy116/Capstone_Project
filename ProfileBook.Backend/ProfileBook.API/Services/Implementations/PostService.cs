using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Post;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;

        public PostService(DataContext context)
        {
            _context = context;
        }

        public async Task<PostResponseDto> CreatePost(PostCreateDto request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == request.UserId);

            if (!userExists)
                throw new Exception("Invalid userId.");

            var post = new Post
            {
                Content = request.Content,
                MediaUrl = request.MediaUrl,
                MediaType = request.MediaType,
                UserId = request.UserId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return new PostResponseDto
            {
                PostId = post.PostId,
                Content = post.Content,
                MediaUrl = post.MediaUrl,
                MediaType = post.MediaType,
                Status = post.Status,
                CreatedAt = post.CreatedAt,
                // These might be null directly after create unless fetched, but that's okay for create response
            };
        }

        public async Task<List<PostResponseDto>> GetPosts()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Where(p => p.Status == "Approved")
                .Select(p => new PostResponseDto
                {
                    PostId = p.PostId,
                    Content = p.Content,
                    MediaUrl = p.MediaUrl,
                    MediaType = p.MediaType,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UserName = p.User.Username,
                    ProfileImage = p.User.ProfileImage,
                    LikeCount = p.Likes.Count(),
                    CommentCount = p.Comments.Count()
                })
                .ToListAsync();
        }

        public async Task<List<PostResponseDto>> GetPostsByUser(int userId)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Where(p => p.UserId == userId && p.Status == "Approved")
                .Select(p => new PostResponseDto
                {
                    PostId = p.PostId,
                    Content = p.Content,
                    MediaUrl = p.MediaUrl,
                    MediaType = p.MediaType,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UserName = p.User.Username,
                    ProfileImage = p.User.ProfileImage,
                    LikeCount = p.Likes.Count(),
                    CommentCount = p.Comments.Count()
                })
                .ToListAsync();
        }

        public async Task<List<PostResponseDto>> GetPendingPosts()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Where(p => p.Status == "Pending")
                .Select(p => new PostResponseDto
                {
                    PostId = p.PostId,
                    Content = p.Content,
                    MediaUrl = p.MediaUrl,
                    MediaType = p.MediaType,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UserName = p.User.Username,
                    ProfileImage = p.User.ProfileImage,
                    LikeCount = p.Likes.Count(),
                    CommentCount = p.Comments.Count()
                })
                .ToListAsync();
        }

        public async Task<PostApprovalDto> ApprovePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                throw new Exception("Post not found");

            post.Status = "Approved";

            await _context.SaveChangesAsync();

            return new PostApprovalDto
            {
                PostId = post.PostId,
                Status = post.Status
            };
        }

        public async Task<PostApprovalDto> RejectPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                throw new Exception("Post not found");

            post.Status = "Rejected";

            await _context.SaveChangesAsync();

            return new PostApprovalDto
            {
                PostId = post.PostId,
                Status = post.Status
            };
        }
    }
}
