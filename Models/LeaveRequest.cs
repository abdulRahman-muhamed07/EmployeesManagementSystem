namespace EmployeesManagementSystem.Models
{
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class LeaveRequest
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser? Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public string? AdminNote { get; set; } = string.Empty;
    }
}
