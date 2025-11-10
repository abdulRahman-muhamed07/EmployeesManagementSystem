namespace EmployeesManagementSystem.DTOs.Request
{
    public class DashboardDtoSimple
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int LateEmployeesToday { get; set; }
        public int NewLeaveRequests { get; set; }
    }
}
