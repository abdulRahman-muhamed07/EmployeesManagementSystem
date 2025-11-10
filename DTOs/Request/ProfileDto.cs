namespace EmployeesManagementSystem.DTOs.Request
{
    public class ProfileDto
    {

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
    }
}
