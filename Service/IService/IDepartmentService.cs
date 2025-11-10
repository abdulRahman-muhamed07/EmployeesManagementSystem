using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface IDepartmentService
    {
        Task<object> GetAllAsync();
        Task<object> GetByIdAsync(int id);
        Task<object> CreateAsync(DepartmentCreateDto dto);
        Task<object> UpdateAsync(DepartmentUpdateDto dto);
        Task<object> DeleteAsync(int id);
    }
}
