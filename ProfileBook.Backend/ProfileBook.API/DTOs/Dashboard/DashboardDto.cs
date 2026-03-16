using System;
using System.Collections.Generic;

namespace ProfileBook.API.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int PendingPosts { get; set; }
        public int TotalReports { get; set; }
        public int ActiveGroups { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; } = new List<RecentActivityDto>();
    }

    public class RecentActivityDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // e.g., "Created a new post", "Reported a user"
        public string TimeAgo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
