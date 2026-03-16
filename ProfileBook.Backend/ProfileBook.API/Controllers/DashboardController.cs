using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileBook.API.Services.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace ProfileBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _dashboardService.GetDashboardStats();
            return Ok(stats);
        }

        [Authorize]
        [HttpGet("test-role")]
        public IActionResult TestRole()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var isAdmin = User.IsInRole("Admin");
            return Ok(new { IsAdmin = isAdmin, Claims = claims });
        }
    }
}
