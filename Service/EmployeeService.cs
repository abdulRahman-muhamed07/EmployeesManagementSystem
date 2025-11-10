using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Repositories.IRepositories;
using EmployeesManagementSystem.Service.IService;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IRepository<Department> _departmentRepo;

        public EmployeeService(IRepository<Employee> employeeRepo, IRepository<Department> departmentRepo)
        {
            _employeeRepo = employeeRepo;
            _departmentRepo = departmentRepo;
        }

        // Get all employees
        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepo.GetAsync(null, new System.Linq.Expressions.Expression<Func<Employee, object>>[]
            {
                e => e.Department
            });

            var list = new List<EmployeeDto>();
            foreach (var e in employees)
            {
                list.Add(new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Position = e.Position,
                    DepartmentName = e.Department?.Name ?? "",
                    Phone = e.Phone,
                    Email = e.Email
                });
            }
            return list;
        }

        // Get employee by Id
        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var e = await _employeeRepo.GetOneAsync(x => x.Id == id, new System.Linq.Expressions.Expression<Func<Employee, object>>[]
            {
                x => x.Department
            });
            if (e == null) return null;

            return new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Position = e.Position,
                DepartmentName = e.Department?.Name ?? "",
                Phone = e.Phone,
                Email = e.Email
            };
        }

        // Create employee
        public async Task<object> CreateAsync(CreateEmployeeDto dto)
        {
            var dept = await _departmentRepo.GetOneAsync(d => d.Id == dto.DepartmentId);
            if (dept == null)
                return new { success = false, message = "Department not found" };

            var employee = new Employee
            {
                FullName = dto.FullName,
                Position = dto.Position,
                Department = dept,
                Phone = dto.Phone,
                Email = dto.Email
            };

            await _employeeRepo.AddAsync(employee);
            await _employeeRepo.CommitAsync();

            return new { success = true, message = "Employee created", data = new { employee.Id } };
        }

        // Update employee
        public async Task<object> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepo.GetOneAsync(e => e.Id == id);
            if (employee == null)
                return new { success = false, message = "Employee not found" };

            var dept = await _departmentRepo.GetOneAsync(d => d.Id == dto.DepartmentId);
            if (dept == null)
                return new { success = false, message = "Department not found" };

            employee.FullName = dto.FullName;
            employee.Position = dto.Position;
            employee.Department = dept;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;

            _employeeRepo.Update(employee);
            await _employeeRepo.CommitAsync();

            return new { success = true, message = "Employee updated", data = new { employee.Id } };
        }

        // Delete employee
        public async Task<object> DeleteAsync(int id)
        {
            var employee = await _employeeRepo.GetOneAsync(e => e.Id == id);
            if (employee == null)
                return new { success = false, message = "Employee not found" };

            _employeeRepo.Delete(employee);
            await _employeeRepo.CommitAsync();

            return new { success = true, message = "Employee deleted", data = new { employee.Id } };
        }
    }
}
