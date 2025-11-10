using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface IAttendanceService


    {

        Task<AttendanceDto> CheckInAsync(string username);
        Task<AttendanceDto> CheckOutAsync(string username);
        Task<List<AttendanceDto>> GetMyAttendanceAsync(string username);
        Task<List<AttendanceDto>> GetAllAttendanceAsync(int? employeeId, int? departmentId, DateTime? date);

        Task<List<AttendanceDto>> GetMyMonthlyAttendanceAsync(string username, int month, int year);
        Task<List<AttendanceDto>> GetFilteredAttendanceAsync(int? employeeId, int? departmentId, DateTime? date);

    }
}
