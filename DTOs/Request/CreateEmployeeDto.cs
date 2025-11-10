namespace EmployeesManagementSystem.DTOs.Request
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int DepartmentId { get; internal set; }
    }

}
