using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDashboard()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}
