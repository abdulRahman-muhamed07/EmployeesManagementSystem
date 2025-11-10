using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface IDashboardService
    {
        Task<DashboardDtoSimple> GetDashboardStatsAsync();

    }
}
