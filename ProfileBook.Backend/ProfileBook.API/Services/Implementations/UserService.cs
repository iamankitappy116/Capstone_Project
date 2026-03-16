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

            // 1. Delete Reports where user is reporter or reported
            var reports = await _context.Reports
                .Where(r => r.ReportedUserId == id || r.ReportingUserId == id)
                .ToListAsync();
            _context.Reports.RemoveRange(reports);

            // 2. Delete Messages where user is sender or receiver
            var messages = await _context.Messages
                .Where(m => m.SenderId == id || m.ReceiverId == id)
                .ToListAsync();
            _context.Messages.RemoveRange(messages);

            // 3. Delete Group Memberships
            var groupMembers = await _context.GroupMembers
                .Where(gm => gm.UserId == id)
                .ToListAsync();
            _context.GroupMembers.RemoveRange(groupMembers);

            // 4. Handle Groups created by user (Delete them)
            var createdGroups = await _context.Groups
                .Where(g => g.CreatedByUserId == id)
                .ToListAsync();
            _context.Groups.RemoveRange(createdGroups);

            // 5. Delete user's content (Posts, Comments, Likes)
            // Note: Cascade deletes on Post should handle its comments/likes if configured in DB,
            // but we'll be explicit for safety.
            _context.Comments.RemoveRange(user.Comments);
            _context.Likes.RemoveRange(user.Likes);
            _context.Posts.RemoveRange(user.Posts);

            // 6. Delete the user
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}