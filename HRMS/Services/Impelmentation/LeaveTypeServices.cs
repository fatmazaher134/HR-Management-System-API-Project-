using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class LeaveTypeServices : ILeaveTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all leave types
        public async Task<IEnumerable<LeaveType>> GetAllAsync()
        {
            return await _unitOfWork.LeaveType.GetAllAsync();
        }

        // Get leave type by ID
        public async Task<LeaveType?> GetByIdAsync(int id)
        {
            return await _unitOfWork.LeaveType.GetByIdAsync(id);
        }

        // Add new leave type
        public async Task<LeaveType> AddAsync(LeaveType leaveType)
        {
            await _unitOfWork.LeaveType.AddAsync(leaveType);
            await _unitOfWork.SaveChangesAsync();
            return leaveType;
        }

        // Update leave type
        public async Task<bool> UpdateAsync(LeaveType leaveType)
        {
            try
            {
                await _unitOfWork.LeaveType.UpdateAsync(leaveType);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
        public async Task<bool> DeleteAsync(int id)
        {
            var leaveType = await _unitOfWork.LeaveType.GetByIdAsync(id);

            if (leaveType == null)
                return false;

            try
            {
                await _unitOfWork.LeaveType.DeleteAsync(leaveType);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
