using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(IAttendanceService attendanceService, UserManager<ApplicationUser> userManager)
        {
            _attendanceService = attendanceService;
            _userManager = userManager;
        }

        // ------------------------- Employee Endpoints -------------------------

        [HttpPost("checkin")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CheckIn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _attendanceService.CheckInAsync(user.UserName!);
            return Ok(result);
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CheckOut()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _attendanceService.CheckOutAsync(user.UserName!);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyAttendance()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _attendanceService.GetMyAttendanceAsync(user.UserName!);
            return Ok(result);
        }

        [HttpGet("my/monthly")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyMonthlyAttendance([FromQuery] int month, [FromQuery] int year)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _attendanceService.GetMyMonthlyAttendanceAsync(user.UserName!, month, year);
            return Ok(result);
        }

        // ------------------------- Admin Endpoints -------------------------

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int? employeeId, [FromQuery] int? departmentId, [FromQuery] DateTime? date)
        {
            var result = await _attendanceService.GetAllAttendanceAsync(employeeId, departmentId, date);
            return Ok(result);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFiltered([FromQuery] int? employeeId, [FromQuery] int? departmentId, [FromQuery] DateTime? date)
        {
            var result = await _attendanceService.GetFilteredAttendanceAsync(employeeId, departmentId, date);
            return Ok(result);
        }
    }
}
