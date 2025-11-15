using HRMS.DTOs.SalaryComponent;
using HRMS.Models;
using HRMS.ViewModels.SalaryComponent;

namespace HRMS.Interfaces.Services
{
    public interface ISalaryComponentServices
    {
        Task<IEnumerable<SalaryComponentDto>> GetAllAsync();
        Task<SalaryComponentDto?> GetByIdAsync(int id);
        Task<SalaryComponentDto> CreateAsync(CreateSalaryComponentDto model); 
        Task<bool> UpdateAsync(int id, UpdateSalaryComponentDto model); 
        Task<bool> DeleteAsync(int id);
    }

}
