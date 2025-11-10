using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface IProfileService
    {

        Task<ProfileDto?> GetMyProfileAsync(string userId);

    }
}
