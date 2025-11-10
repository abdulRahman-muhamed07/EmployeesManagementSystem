using EmployeesManagementSystem.DataAccess;
using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagementSystem.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AttendanceService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Employee CheckIn
        public async Task<AttendanceDto> CheckInAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found");

            // منع CheckIn مرتين بنفس اليوم
            var todayCheck = await _context.Attendances
                .Where(a => a.EmployeeId == user.Id && a.CheckIn.Date == DateTime.Now.Date)
                .FirstOrDefaultAsync();

            if (todayCheck != null)
                throw new Exception("Already checked in today");

            var attendance = new Attendance
            {
                EmployeeId = user.Id,
                CheckIn = DateTime.Now
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                EmployeeId = user.Id,
                EmployeeName = user.FullName,
                CheckIn = attendance.CheckIn
            };
        }

        // Employee CheckOut
        public async Task<AttendanceDto> CheckOutAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found");

            var attendance = await _context.Attendances
                .Where(a => a.EmployeeId == user.Id && a.CheckOut == null)
                .OrderByDescending(a => a.CheckIn)
                .FirstOrDefaultAsync();

            if (attendance == null) throw new Exception("No active check-in found");

            attendance.CheckOut = DateTime.Now;
            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                EmployeeId = user.Id,
                EmployeeName = user.FullName,
                CheckIn = attendance.CheckIn,
                CheckOut = attendance.CheckOut
            };
        }

        // Employee: Get personal attendance
        public async Task<List<AttendanceDto>> GetMyAttendanceAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) throw new Exception("User not found");

            var attendances = await _context.Attendances
                .Where(a => a.EmployeeId == user.Id)
                .OrderByDescending(a => a.CheckIn)
                .ToListAsync();

            return attendances.Select(a => new AttendanceDto
            {
                EmployeeId = user.Id,
                EmployeeName = user.FullName,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut
            }).ToList();
        }

        // Admin: Get all attendance with filters
        public async Task<List<AttendanceDto>> GetAllAttendanceAsync(int? employeeId, int? departmentId, DateTime? date)
        {
            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();

            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeId == employeeId.Value.ToString());

            if (departmentId.HasValue)
                query = query.Where(a => a.Employee.DepartmentId == departmentId);

            if (date.HasValue)
                query = query.Where(a => a.CheckIn.Date == date.Value.Date);

            var attendances = await query.OrderByDescending(a => a.CheckIn).ToListAsync();

            return attendances.Select(a => new AttendanceDto
            {
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.FullName,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut
            }).ToList();
        }

        public async Task<List<AttendanceDto>> GetMyMonthlyAttendanceAsync(string username, int month, int year)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("User not found");

            var list = await _context.Attendances
                .Where(a => a.EmployeeId == user.Id &&
                            a.CheckIn.Month == month &&
                            a.CheckIn.Year == year)
                .OrderByDescending(a => a.CheckIn)
                .ToListAsync();

            return list.Select(a => new AttendanceDto
            {
                Id = a.Id,
                EmployeeName = user.FullName,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut
            }).ToList();
        }

        public async Task<List<AttendanceDto>> GetFilteredAttendanceAsync(int? employeeId, int? departmentId, DateTime? date)
        {
            return await GetAllAttendanceAsync(employeeId, departmentId, date);
        }
    }
}
