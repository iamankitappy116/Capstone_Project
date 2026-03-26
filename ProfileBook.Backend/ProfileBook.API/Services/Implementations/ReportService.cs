using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;
using ProfileBook.API.DTOs.Report;
using ProfileBook.API.Models;
using ProfileBook.API.Services.Interfaces;

namespace ProfileBook.API.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<ReportResponseDto> CreateReport(ReportCreateDto request)
        {
            var report = new Report
            {
                ReportingUserId = request.ReportingUserId,
                ReportedUserId = request.ReportedUserId,
                Reason = request.Reason,
                TimeStamp = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReportingUserId = report.ReportingUserId,
                ReportedUserId = report.ReportedUserId,
                Reason = report.Reason,
                TimeStamp = report.TimeStamp
            };
        }

        public async Task<List<ReportResponseDto>> GetAllReports()
        {
            return await _context.Reports
                .Include(r => r.ReportingUser)
                .Include(r => r.ReportedUser)
                .Select(r => new ReportResponseDto
                {
                    ReportId = r.ReportId,
                    ReportingUserId = r.ReportingUserId,
                    ReportingUserName = r.ReportingUser != null ? r.ReportingUser.Username : "Unknown",
                    ReportedUserId = r.ReportedUserId,
                    ReportedUserName = r.ReportedUser != null ? r.ReportedUser.Username : "Unknown",
                    Reason = r.Reason,
                    TimeStamp = r.TimeStamp,
                    Severity = r.Reason.ToLower().Contains("inappropriate") ? "High" : (r.Reason.ToLower().Contains("spam") ? "Low" : "Medium")
                })
                .ToListAsync();
        }

        public async Task<List<ReportResponseDto>> GetReportsForUser(int userId)
        {
            return await _context.Reports
                .Where(r => r.ReportedUserId == userId)
                .Include(r => r.ReportingUser)
                .Include(r => r.ReportedUser)
                .Select(r => new ReportResponseDto
                {
                    ReportId = r.ReportId,
                    ReportingUserId = r.ReportingUserId,
                    ReportingUserName = r.ReportingUser != null ? r.ReportingUser.Username : "Unknown",
                    ReportedUserId = r.ReportedUserId,
                    ReportedUserName = r.ReportedUser != null ? r.ReportedUser.Username : "Unknown",
                    Reason = r.Reason,
                    TimeStamp = r.TimeStamp,
                    Severity = r.Reason.ToLower().Contains("inappropriate") ? "High" : (r.Reason.ToLower().Contains("spam") ? "Low" : "Medium")
                })
                .ToListAsync();
        }
    }
}
