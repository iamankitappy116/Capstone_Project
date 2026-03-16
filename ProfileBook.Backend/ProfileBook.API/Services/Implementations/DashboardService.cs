using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Dashboard;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;

        public DashboardService(DataContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var pendingPosts = await _context.Posts.Where(p => p.Status == "Pending").CountAsync();
            var totalReports = await _context.Reports.CountAsync();
            var activeGroups = await _context.Groups.CountAsync();

            var recentUsers = await _context.Users
                .OrderByDescending(u => u.UserId)
                .Take(5)
                .Select(u => new RecentActivityDto
                {
                    UserName = u.Username,
                    Action = "Created a new account",
                    CreatedAt = DateTime.UtcNow.AddHours(-2), // Still placeholder
                    TimeAgo = "2 hours ago"
                }).ToListAsync();

            var recentPosts = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new RecentActivityDto
                {
                    UserName = p.User != null ? p.User.Username : "Unknown",
                    Action = "Created a new post",
                    CreatedAt = p.CreatedAt,
                    TimeAgo = GetTimeAgo(p.CreatedAt)
                }).ToListAsync();

            var recentReports = await _context.Reports
                .Include(r => r.ReportingUser)
                .OrderByDescending(r => r.TimeStamp)
                .Take(5)
                .Select(r => new RecentActivityDto
                {
                    UserName = r.ReportingUser != null ? r.ReportingUser.Username : "Unknown",
                    Action = "Reported a user",
                    CreatedAt = r.TimeStamp,
                    TimeAgo = GetTimeAgo(r.TimeStamp)
                }).ToListAsync();

            var activities = recentPosts.Concat(recentReports).Concat(recentUsers)
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .ToList();

            return new DashboardStatsDto
            {
                TotalUsers = totalUsers,
                PendingPosts = pendingPosts,
                TotalReports = totalReports,
                ActiveGroups = activeGroups,
                RecentActivities = activities
            };
        }

        private static string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.UtcNow - dateTime;
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} minutes ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} hours ago";
            return $"{(int)span.TotalDays} days ago";
        }
    }
}
