using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.DTOs.Response;
using EmployeesManagementSystem.Models;
using EmployeesManagementSystem.Repositories.IRepositories;
using EmployeesManagementSystem.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeesManagementSystem.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<ApplicationUser> _userRepository;

        public ProfileService(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ProfileDto?> GetMyProfileAsync(string userId)
        {
            var user = await _userRepository.GetOneAsync(
                u => u.Id == userId,
                new Expression<Func<ApplicationUser, object>>[] { u => u.Department }
            );

            if (user == null)
                return null;

            return new ProfileDto
            {
                FullName = user.FullName,
                Email = user.Email ?? "",
                DepartmentName = user.Department.Name ?? "No Department",
                JoinDate = user.JoinDate
            };
        }
    }
}
