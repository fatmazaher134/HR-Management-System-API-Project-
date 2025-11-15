using HRMS.DTOs.Employee;
using HRMS.Models;
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Interfaces.Services
{
    public interface IEmployeeServices
    {
        Task<IEnumerable<EmployeeListDTO>> GetAllAsync();
        Task<EmployeeDetailsDTO?> GetByIdAsync(int id);
        Task<IdentityResult> RegisterEmployeeAsync(CreateEmployeeDTO dto);
        Task<bool> UpdateAsync(UpdateEmployeeDTO dto);
        Task<bool> UpdateBasicInfoAsync(int employeeId, UpdateEmployeeBasicInfoDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<MyProfileDTO?> GetByUserIdAsync(string userId);
        Task<IEnumerable<EmployeeListDTO>> GetByDepartmentIdAsync(int departmentId);
        Task<decimal> GetTotalSalaryAsync(int departmentId);
        Task<bool> IsEmailExistsAsync(string email, int? excludeEmployeeId = null);
    }

}