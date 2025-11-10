using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Repositories.IRepositories;
using EmployeesManagementSystem.Service.IService;

namespace EmployeesManagementSystem.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<LeaveRequest> _leaveRepository;

        public DashboardService(
            IRepository<ApplicationUser> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<LeaveRequest> leaveRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
        }

        public async Task<DashboardDtoSimple> GetDashboardStatsAsync()
        {
            var today = DateTime.Today;

            // عدد الموظفين
            var totalEmployees = await Task.FromResult(_userRepository.Query().Count());

            // عدد الأقسام
            var totalDepartments = await Task.FromResult(_departmentRepository.Query().Count());

            // الموظفين المتأخرين اليوم (بعد الساعة 9 صباحًا)
            var lateEmployeesToday = await Task.FromResult(
                _attendanceRepository.Query()
                    .Where(a => a.CheckIn.Date == today && a.CheckIn.TimeOfDay > TimeSpan.FromHours(9))
                    .Select(a => a.EmployeeId)
                    .Distinct()
                    .Count()
            );

            // الطلبات الجديدة (Pending)
            var newLeaveRequests = await Task.FromResult(
                _leaveRepository.Query()
                    .Where(l => l.Status == LeaveStatus.Pending)
                    .Count()
            );

            return new DashboardDtoSimple
            {
                TotalEmployees = totalEmployees,
                TotalDepartments = totalDepartments,
                LateEmployeesToday = lateEmployeesToday,
                NewLeaveRequests = newLeaveRequests
            };
        }
    }
}
