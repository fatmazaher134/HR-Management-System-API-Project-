using HRMS.DTOs.LeaveRequest;
using HRMS.Models;
using HRMS.ViewModels.Employee;

namespace HRMS.Interfaces.Services
{
    public interface ILeaveRequestServices
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();

        Task<IEnumerable<LeaveRequestDto>> GetMyRequestsAsync(string userId);

        Task<bool> CreateAsync(CreateLeaveRequestDto model, string userId);

        Task<bool> ApproveAsync(int id, int hrEmployeeId);

        Task<bool> RejectAsync(int id, int hrEmployeeId, string? comments);

        Task<bool> CancelAsync(int id, int hrEmployeeId);

        Task<bool> DeleteAsync(int id, string userId);



    }

}