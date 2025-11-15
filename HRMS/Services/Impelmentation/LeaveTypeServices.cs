using AutoMapper;
using HRMS.DTOs.LeaveType;
using HRMS.Interfaces;
using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Implementation
{
    public class LeaveTypeServices : ILeaveTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveTypeServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all leave types
        public async Task<IEnumerable<LeaveTypeDTO>> GetAllAsync()
        {
            var leaveTypes = await _unitOfWork.LeaveType.GetAllAsync();

            // Map Model to DTO
            return _mapper.Map<IEnumerable<LeaveTypeDTO>>(leaveTypes);
        }

        // Get leave type by ID
        public async Task<LeaveTypeDTO?> GetByIdAsync(int id)
        {
            var leaveType = await _unitOfWork.LeaveType.GetByIdAsync(id);

            if (leaveType == null)
                return null;

            // Map Model to DTO
            return _mapper.Map<LeaveTypeDTO>(leaveType);
        }

        // Add new leave type
        public async Task<LeaveTypeDTO> AddAsync(CreateLeaveTypeDTO dto)
        {
            // Map DTO to Model
            var leaveType = _mapper.Map<LeaveType>(dto);

            await _unitOfWork.LeaveType.AddAsync(leaveType);
            await _unitOfWork.SaveChangesAsync();

            // Map Model back to DTO
            return _mapper.Map<LeaveTypeDTO>(leaveType);
        }

        // Update leave type
        public async Task<bool> UpdateAsync(UpdateLeaveTypeDTO dto)
        {
            try
            {
                // Map DTO to Model
                var leaveType = _mapper.Map<LeaveType>(dto);

                await _unitOfWork.LeaveType.UpdateAsync(leaveType);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete leave type
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