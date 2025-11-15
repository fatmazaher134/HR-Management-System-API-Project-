using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IDepartmentServices
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto?> GetByEmpIdAsync(int Empid);
        Task<DepartmentDto> AddAsync(DepartmentFormDto dto);

        Task<bool> UpdateAsync(int id, DepartmentFormDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
