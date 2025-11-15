using HRMS.Models;
using HRMS.DTOs.LeaveType;


namespace HRMS.Interfaces.Services
{
    public interface ILeaveTypeServices
    {
        Task<IEnumerable<LeaveTypeDTO>> GetAllAsync();
        Task<LeaveTypeDTO?> GetByIdAsync(int id);
        Task<LeaveTypeDTO> AddAsync(CreateLeaveTypeDTO dto);
        Task<bool> UpdateAsync(UpdateLeaveTypeDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
