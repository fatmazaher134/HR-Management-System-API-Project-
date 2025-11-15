<<<<<<< HEAD
﻿using HRMS.Models;
=======
﻿using HRMS.DTOs.LeaveRequest;
using HRMS.Models;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using HRMS.ViewModels.Employee;

namespace HRMS.Interfaces.Services
{
    public interface ILeaveRequestServices
    {
<<<<<<< HEAD
        Task<IEnumerable<LeaveRequestViewModel>> GetAllAsync();
        Task<IEnumerable<LeaveRequestViewModel>> GetMyRequestsAsync(string userId);
        Task<bool> CreateAsync(CreateLeaveRequestViewModel model, string userId);
        Task<bool> ApproveAsync(int id, int hrEmployeeId);
        Task<bool> RejectAsync(int id, int hrEmployeeId, string? comments);
        Task<bool> CancelAsync(int id, int hrEmployeeId);
=======
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();

        Task<IEnumerable<LeaveRequestDto>> GetMyRequestsAsync(string userId);

        Task<bool> CreateAsync(CreateLeaveRequestDto model, string userId);

        Task<bool> ApproveAsync(int id, int hrEmployeeId);

        Task<bool> RejectAsync(int id, int hrEmployeeId, string? comments);

        Task<bool> CancelAsync(int id, int hrEmployeeId);

>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        Task<bool> DeleteAsync(int id, string userId);



    }

}