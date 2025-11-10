using EmployeesManagementSystem.DataAccess;
using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagementSystem.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public LeaveService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Employee: طلب إجازة
        public async Task<LeaveRequestDto> CreateLeaveAsync(string username, CreateLeaveRequestDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found");

            var leave = new LeaveRequest
            {
                EmployeeId = user.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = LeaveStatus.Pending
            };

            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();

            return new LeaveRequestDto
            {
                Id = leave.Id,
                EmployeeId = user.Id,
                EmployeeName = user.FullName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                Status = leave.Status.ToString()
            };
        }

        // Employee: عرض سجل الإجازات الشخصية
        public async Task<List<LeaveRequestDto>> GetMyLeavesAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found");

            var leaves = await _context.LeaveRequests
                .Where(l => l.EmployeeId == user.Id)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return leaves.Select(l => new LeaveRequestDto
            {
                Id = l.Id,
                EmployeeId = user.Id,
                EmployeeName = user.FullName,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status.ToString(),
                AdminNote = l.AdminNote
            }).ToList();
        }

        // Admin: عرض كل الطلبات
        public async Task<List<LeaveRequestDto>> GetAllLeavesAsync()
        {
            var leaves = await _context.LeaveRequests.Include(l => l.Employee)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return leaves.Select(l => new LeaveRequestDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                EmployeeName = l.Employee?.FullName ?? "",
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status.ToString(),
                AdminNote = l.AdminNote
            }).ToList();
        }

        // Admin: الموافقة على إجازة
        public async Task<LeaveRequestDto> ApproveLeaveAsync(int leaveId, ApproveRejectLeaveDto dto, string adminId)
        {
            // لو انت مش محتاج adminId دلوقتي، ممكن تتجاهله مؤقتًا
            var leave = await _context.LeaveRequests.Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == leaveId);

            if (leave == null) throw new Exception("Leave not found");

            leave.Status = LeaveStatus.Approved;
            leave.AdminNote = dto.Note;

            await _context.SaveChangesAsync();

            return new LeaveRequestDto
            {
                Id = leave.Id,
                EmployeeId = leave.EmployeeId,
                EmployeeName = leave.Employee?.FullName ?? "",
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                Status = leave.Status.ToString(),
                AdminNote = leave.AdminNote
            };
        }

        public async Task<LeaveRequestDto> RejectLeaveAsync(int leaveId, ApproveRejectLeaveDto dto, string adminId)
        {
            var leave = await _context.LeaveRequests.Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == leaveId);

            if (leave == null) throw new Exception("Leave not found");

            leave.Status = LeaveStatus.Rejected;
            leave.AdminNote = dto.Note;

            await _context.SaveChangesAsync();

            return new LeaveRequestDto
            {
                Id = leave.Id,
                EmployeeId = leave.EmployeeId,
                EmployeeName = leave.Employee?.FullName ?? "",
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                Status = leave.Status.ToString(),
                AdminNote = leave.AdminNote
            };
        }

















    }
}
