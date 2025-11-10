using EmployeesManagementSystem.DTOs.Request;

namespace EmployeesManagementSystem.Service.IService
{
    public interface ILeaveService
    {
        // Employee
        Task<LeaveRequestDto> CreateLeaveAsync(string userId, CreateLeaveRequestDto dto);
        Task<List<LeaveRequestDto>> GetMyLeavesAsync(string userId);

        // Admin
        Task<List<LeaveRequestDto>> GetAllLeavesAsync();
        Task<LeaveRequestDto> ApproveLeaveAsync(int leaveId, ApproveRejectLeaveDto dto, string adminId);
        Task<LeaveRequestDto> RejectLeaveAsync(int leaveId, ApproveRejectLeaveDto dto, string adminId);
    }
}
