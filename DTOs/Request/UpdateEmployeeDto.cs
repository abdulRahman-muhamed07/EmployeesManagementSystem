namespace EmployeesManagementSystem.DTOs.Request
{
    public class UpdateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int DepartmentId { get; set; }      
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
