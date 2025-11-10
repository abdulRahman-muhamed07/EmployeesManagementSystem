using Microsoft.AspNetCore.Identity;

namespace EmployeesManagementSystem.Models
{
    public class ApplicationUser  : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public Department Department { get; internal set; }
        public int DepartmentId { get; internal set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

    }
}
