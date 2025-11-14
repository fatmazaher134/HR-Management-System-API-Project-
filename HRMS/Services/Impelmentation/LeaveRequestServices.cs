using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.Employee;

namespace HRMS.Services.Impelmentation
{
    public class LeaveRequestServices : ILeaveRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveRequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LeaveRequestViewModel>> GetAllAsync()
        {
            var requests = await _unitOfWork.LeaveRequest.FindAllAsync(
                includes: new[] { "Employee", "LeaveType" });

            return requests.Select(r => new LeaveRequestViewModel
            {
                LeaveRequestID = r.LeaveRequestID,
                EmployeeName = $"{r.Employee?.FirstName} {r.Employee?.LastName}",
                LeaveTypeName = r.LeaveType?.TypeName ?? "",
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                Comments = r.Comments
            });
        }

        // to spacific HR employee
        public async Task<IEnumerable<LeaveRequestViewModel>> GetMyRequestsAsync(string userId)
        {
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == userId);
            if (emp == null) return Enumerable.Empty<LeaveRequestViewModel>();

            var requests = await _unitOfWork.LeaveRequest.FindAllAsync(
                r => r.EmployeeID == emp.EmployeeID,
                includes: new[] { "LeaveType" });

            return requests.Select(r => new LeaveRequestViewModel
            {
                LeaveRequestID = r.LeaveRequestID,
                LeaveTypeName = r.LeaveType?.TypeName ?? "",
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                Comments = r.Comments
            });
        }

        public async Task<bool> CreateAsync(CreateLeaveRequestViewModel model, string userId)
        {
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == userId);
            if (emp == null) return false;

            var entity = new LeaveRequest
            {
                EmployeeID = emp.EmployeeID,
                LeaveTypeID = model.LeaveTypeID,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Comments = model.Comments,
                Status = "Pending"
            };

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
            var request = await _unitOfWork.LeaveRequest
                .FindAsync(lr => lr.LeaveRequestID == id && lr.Employee.ApplicationUserId == userId);

            if (request == null)
                return false;

            await _unitOfWork.LeaveRequest.DeleteAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}