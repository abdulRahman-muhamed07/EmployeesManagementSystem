using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.DTOs.Response;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeesManagementSystem.Service
{
    public class DepartmentService
    {
        private readonly IRepository<Department> _departmentRepo;

        public DepartmentService(IRepository<Department> departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public async Task<object> GetAllAsync()
        {
            var departments = await _departmentRepo.Query()
                .Include(d => d.Employees)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    EmployeeCount = d.Employees.Count
                }).ToListAsync();

            return new { success = true, message = "Departments retrieved successfully", data = departments };
        }

        public async Task<object> GetByIdAsync(int id)
        {
            var dep = await _departmentRepo.Query()
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dep == null)
                return new { success = false, message = "Department not found", data = (object?)null };

            var dto = new DepartmentResponseDto
            {
                Id = dep.Id,
                Name = dep.Name,
                EmployeeCount = dep.Employees.Count
            };

            return new { success = true, message = "Department retrieved", data = dto };
        }

        public async Task<object> CreateAsync(DepartmentCreateDto dto)
        {
            var exists = await _departmentRepo.GetOneAsync(d => d.Name == dto.Name);
            if (exists != null)
                return new { success = false, message = "Department name already exists", data = (object?)null };

            var dep = new Department { Name = dto.Name };
            await _departmentRepo.AddAsync(dep);
            await _departmentRepo.CommitAsync();

            return new { success = true, message = "Department created successfully", data = dep };
        }

        public async Task<object> UpdateAsync(DepartmentUpdateDto dto)
        {
            var dep = await _departmentRepo.GetOneAsync(d => d.Id == dto.Id);
            if (dep == null)
                return new { success = false, message = "Department not found", data = (object?)null };

            dep.Name = dto.Name;
            _departmentRepo.Update(dep);
            await _departmentRepo.CommitAsync();

            return new { success = true, message = "Department updated successfully", data = dep };
        }

        public async Task<object> DeleteAsync(int id)
        {
            var dep = await _departmentRepo.GetOneAsync(d => d.Id == id, new Expression<Func<Department, object>>[] { d => d.Employees });
            if (dep == null)
                return new { success = false, message = "Department not found", data = (object?)null };

            if (dep.Employees != null && dep.Employees.Any())
                return new { success = false, message = "Cannot delete department with assigned employees", data = (object?)null };

            _departmentRepo.Delete(dep);
            await _departmentRepo.CommitAsync();

            return new { success = true, message = "Department deleted successfully", data = (object?)null };
        }
    }
}

