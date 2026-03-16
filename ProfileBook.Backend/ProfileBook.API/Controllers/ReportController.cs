using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.DTOs.Report;
using ProfileBook.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProfileBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize] // Any authenticated user can submit a report
        public async Task<IActionResult> CreateReport(ReportCreateDto request)
        {
            var result = await _reportService.CreateReport(request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only admins can see all reports
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReports();
            return Ok(reports);
        }
        
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")] // Only admins can see reports for a specific user
        public async Task<IActionResult> GetReportsForUser(int userId)
        {
            var reports = await _reportService.GetReportsForUser(userId);
            return Ok(reports);
        }
    }
}
