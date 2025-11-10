using System.ComponentModel.DataAnnotations;

namespace EmployeesManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int DepartmentId { get; set; }
        // تأكد أن نوع الخاصية دي هو اسم الكلاس (Department) وليس object
        public  Department Department { get; set; }
    }
}
