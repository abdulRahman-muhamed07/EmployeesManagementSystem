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
    [Authorize]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeavesController(ILeaveService leaveService, UserManager<ApplicationUser> userManager)
        {
            _leaveService = leaveService;
            _userManager = userManager;
        }

         // POST /api/leaves
        // ===============================
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeave([FromBody] CreateLeaveRequestDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _leaveService.CreateLeaveAsync(user.Id, dto);
            return Ok(new { success = true, data = result });
        }

         // GET /api/leaves/my
        // ===============================
        [HttpGet("my")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = await _leaveService.GetMyLeavesAsync(user.Id);
            return Ok(new { success = true, data = result });
        }

         // GET /api/leaves/all
        // ===============================
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var result = await _leaveService.GetAllLeavesAsync();
            return Ok(new { success = true, data = result });
        }

         // PUT /api/leaves/{id}/approve
        // ===============================
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveLeave(int id, [FromBody] ApproveRejectLeaveDto dto)
        {
            var admin = await _userManager.GetUserAsync(User);
            if (admin == null)
                return Unauthorized(new { success = false, message = "Admin not found" });

            var result = await _leaveService.ApproveLeaveAsync(id, dto, admin.Id);
            return Ok(new { success = true, message = "Leave approved", data = result });
        }

        // PUT /api/leaves/{id}/reject
        // ===============================
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectLeave(int id, [FromBody] ApproveRejectLeaveDto dto)
        {
            var admin = await _userManager.GetUserAsync(User);
            if (admin == null)
                return Unauthorized(new { success = false, message = "Admin not found" });

            var result = await _leaveService.RejectLeaveAsync(id, dto, admin.Id);
            return Ok(new { success = true, message = "Leave rejected", data = result });
        }
    }
}
