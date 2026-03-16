using ProfileBook.API.DTOs.Report;

namespace ProfileBook.API.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportResponseDto> CreateReport(ReportCreateDto request);

        Task<List<ReportResponseDto>> GetAllReports();

        Task<List<ReportResponseDto>> GetReportsForUser(int userId);
    }
}
