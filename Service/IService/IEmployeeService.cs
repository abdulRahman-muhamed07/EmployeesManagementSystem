using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<object> CreateAsync(CreateEmployeeDto dto);
        Task<object> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<object> DeleteAsync(int id);
    }
}
