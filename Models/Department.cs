using System.ComponentModel.DataAnnotations;

namespace EmployeesManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public ICollection<Employee>? Employees { get; set; }
    }
}
