namespace EmployeesManagementSystem.DTOs.Request
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int DepartmentId { get; set; } // اختياري لو حاب تضيف الأقسام

    }
}
