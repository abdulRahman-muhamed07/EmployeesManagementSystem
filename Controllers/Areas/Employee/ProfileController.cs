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
    [Authorize] // لازم يكون المستخدم مسجل دخول
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileService _profileService;

        public ProfileController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
        }

        // ✅ GET /api/profile/me
        // ===============================
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var profile = await _profileService.GetMyProfileAsync(user.Id);
            if (profile == null)
                return NotFound(new { success = false, message = "Profile not found" });

            return Ok(new
            {
                success = true,
                data = profile
            });
        }

        // ✅ PUT /api/profile/update
        // ===============================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            user.FullName = model.FullName ?? user.FullName;
            if (model.DepartmentId.HasValue)
                user.DepartmentId = model.DepartmentId.Value;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                success = true,
                message = "Profile updated successfully"
            });
        }
    }
}
