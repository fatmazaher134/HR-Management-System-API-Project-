using HRMS.Models;
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Interfaces.Services
{
    public interface IEmployeeServices
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> UpdateAsync(Employee employee);

        Task<Employee?> GetByUserIdAsync(string userId);

        Task<bool> UpdateBasicInfoAsync(int employeeId, EmployeeEditBasicInfoViewModel model);

        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId);
        Task<decimal> GetTotalSalaryAsync(int departmentId);
        Task<bool> IsEmailExistsAsync(string email, int? excludeEmployeeId = null);
        public Task<IdentityResult> RegisterEmployeeAsync(EmployeeFormViewModel employee);
    }

}