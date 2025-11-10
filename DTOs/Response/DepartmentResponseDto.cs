namespace EmployeesManagementSystem.DTOs.Response
{
    public class DepartmentResponseDto

    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}
