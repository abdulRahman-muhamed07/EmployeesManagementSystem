namespace EmployeesManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public ApplicationUser? Employee { get; set; }

    }
}
