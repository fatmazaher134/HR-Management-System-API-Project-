using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface ILeaveTypeServices
    {
        Task<IEnumerable<LeaveType>> GetAllAsync();
        Task<LeaveType?> GetByIdAsync(int id);
        Task<LeaveType> AddAsync(LeaveType leaveType);
        Task<bool> UpdateAsync(LeaveType leaveType);
        Task<bool> DeleteAsync(int id);
    }

}
