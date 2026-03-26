using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.User;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    ProfileImage = u.ProfileImage,
                    Role = u.Role,
                    Bio = u.Bio,
                    Location = u.Location,
                    FollowerCount = u.FollowerCount,
                    FollowingCount = u.FollowingCount,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<UserResponseDto?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return null;

            return new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Role = user.Role,
                Bio = user.Bio,
                Location = user.Location,
                FollowerCount = user.FollowerCount,
                FollowingCount = user.FollowingCount,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto?> UpdateUser(int id, UserUpdateDto request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return null;

            user.Username = request.Username;
            user.Email = request.Email;
            user.ProfileImage = request.ProfileImage;
            user.Bio = request.Bio;
            user.Location = request.Location;
            user.Role = request.Role ?? user.Role;

            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Role = user.Role,
                Bio = user.Bio,
                Location = user.Location,
                FollowerCount = user.FollowerCount,
                FollowingCount = user.FollowingCount,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto> CreateUser(UserCreateDto request)
        {
            CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            var user = new Models.User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return false;

            // 1. Handle Follows & Sync Counts
            var follows = await _context.UserFollows
                .Where(f => f.FollowerId == id || f.FollowingId == id)
                .ToListAsync();

            foreach (var follow in follows)
            {
                if (follow.FollowerId == id)
                {
                    // User was following someone, decrement their follower count
                    var following = await _context.Users.FindAsync(follow.FollowingId);
                    if (following != null && following.FollowerCount > 0) following.FollowerCount--;
                }
                else
                {
                    // Someone was following the user, decrement their following count
                    var follower = await _context.Users.FindAsync(follow.FollowerId);
                    if (follower != null && follower.FollowingCount > 0) follower.FollowingCount--;
                }
            }
            _context.UserFollows.RemoveRange(follows);

            // 2. Delete Reports
            var reports = await _context.Reports
                .Where(r => r.ReportedUserId == id || r.ReportingUserId == id)
                .ToListAsync();
            _context.Reports.RemoveRange(reports);

            // 3. Delete Messages
            var messages = await _context.Messages
                .Where(m => m.SenderId == id || m.ReceiverId == id)
                .ToListAsync();
            _context.Messages.RemoveRange(messages);

            // 4. Handle Group Memberships (where user is just a member)
            var personalMemberships = await _context.GroupMembers
                .Where(gm => gm.UserId == id)
                .ToListAsync();
            _context.GroupMembers.RemoveRange(personalMemberships);

            // 5. Handle Groups Created by User
            var createdGroups = await _context.Groups
                .Where(g => g.CreatedByUserId == id)
                .ToListAsync();

            if (createdGroups.Any())
            {
                var groupIds = createdGroups.Select(g => g.GroupId).ToList();

                // Delete all memberships in these groups
                var allGroupMembers = await _context.GroupMembers
                    .Where(gm => groupIds.Contains(gm.GroupId))
                    .ToListAsync();
                _context.GroupMembers.RemoveRange(allGroupMembers);

                // Delete all posts in these groups
                var groupPosts = await _context.Posts
                    .Include(p => p.Comments)
                    .Include(p => p.Likes)
                    .Where(p => p.GroupId != null && groupIds.Contains(p.GroupId.Value))
                    .ToListAsync();

                foreach (var p in groupPosts)
                {
                    _context.Comments.RemoveRange(p.Comments);
                    _context.Likes.RemoveRange(p.Likes);
                }
                _context.Posts.RemoveRange(groupPosts);

                // Finally delete the groups
                _context.Groups.RemoveRange(createdGroups);
            }

            // 6. Delete User's Content (not already deleted via group cleanup)
            // Need to ensure comments/likes on these posts are also cleared if not handled by group cleanup
            foreach (var p in user.Posts)
            {
                var postComments = await _context.Comments.Where(c => c.PostId == p.PostId).ToListAsync();
                var postLikes = await _context.Likes.Where(l => l.PostId == p.PostId).ToListAsync();
                _context.Comments.RemoveRange(postComments);
                _context.Likes.RemoveRange(postLikes);
            }
            _context.Posts.RemoveRange(user.Posts);
            _context.Comments.RemoveRange(user.Comments);
            _context.Likes.RemoveRange(user.Likes);

            // 7. Delete the User
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}