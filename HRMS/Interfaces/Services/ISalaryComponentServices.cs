using HRMS.Models;
using HRMS.ViewModels.SalaryComponent;

namespace HRMS.Interfaces.Services
{
    public interface ISalaryComponentServices
    {
        Task<IEnumerable<SalaryComponentViewModel>> GetAllAsync();
        Task<SalaryComponentViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSalaryComponentViewModel model);
        Task<bool> UpdateAsync(int id, EditSalaryComponentViewModel model);
        Task<bool> DeleteAsync(int id);
    }

}
