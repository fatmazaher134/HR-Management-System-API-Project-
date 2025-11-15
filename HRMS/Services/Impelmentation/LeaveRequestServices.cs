using AutoMapper;
using HRMS.DTOs.LeaveRequest;
using HRMS.Interfaces;
using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class LeaveRequestServices : ILeaveRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveRequestServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
        {
            var requests = await _unitOfWork.LeaveRequest.FindAllAsync(
                includes: new[] { "Employee", "LeaveType" });

            return _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetMyRequestsAsync(string userId)
        {
            // var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == userId);
            // if (emp == null) return Enumerable.Empty<LeaveRequestDto>();

            var tempTestEmployeeId = 1; 

            var requests = await _unitOfWork.LeaveRequest.FindAllAsync(
                // r => r.EmployeeID == emp.EmployeeID,
                r => r.EmployeeID == tempTestEmployeeId, 
                includes: new[] { "LeaveType" });

            return _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
        }

        public async Task<bool> CreateAsync(CreateLeaveRequestDto model, string userId)
        {
            // var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == userId);
            // if (emp == null) return false;

            var tempTestEmployeeId = 1; 

            var entity = _mapper.Map<LeaveRequest>(model);

            // entity.EmployeeID = emp.EmployeeID;
            entity.EmployeeID = tempTestEmployeeId; 
            entity.Status = "Pending";
            entity.RequestDate = DateTime.UtcNow;

            await _unitOfWork.LeaveRequest.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id, int hrEmployeeId)
        {
            var request = await _unitOfWork.LeaveRequest.GetByIdAsync(id);
            if (request == null) return false;

            request.Status = "Approved";
            request.ApprovedByHRID = hrEmployeeId;

            await _unitOfWork.LeaveRequest.UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id, int hrEmployeeId, string? comments)
        {
            var request = await _unitOfWork.LeaveRequest.GetByIdAsync(id);
            if (request == null) return false;

            request.Status = "Rejected";
            request.Comments = comments;
            request.ApprovedByHRID = hrEmployeeId;

            await _unitOfWork.LeaveRequest.UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(int id, int hrEmployeeId)
        {
            var request = await _unitOfWork.LeaveRequest.GetByIdAsync(id);
            if (request == null || request.Status != "Pending")
                return false;

            request.Status = "Cancelled";
            request.ApprovedByHRID = hrEmployeeId;

            await _unitOfWork.LeaveRequest.UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            // var request = await _unitOfWork.LeaveRequest
            //     .FindAsync(lr => lr.LeaveRequestID == id && lr.Employee.ApplicationUserId == userId);

            
            var request = await _unitOfWork.LeaveRequest.GetByIdAsync(id);

            if (request == null)
                return false;

            await _unitOfWork.LeaveRequest.DeleteAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}