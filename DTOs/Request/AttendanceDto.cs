namespace EmployeesManagementSystem.DTOs.Request
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double? WorkHours
        {
            get
            {
                if (CheckOut.HasValue)
                    return Math.Round((CheckOut.Value - CheckIn).TotalHours, 2); // ساعتين عشريتين
                return null;
            }
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
