using ProfileBook.API.DTOs.Dashboard;

namespace ProfileBook.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStats();
    }
}
