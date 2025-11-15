using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IDepartmentServices
    {
<<<<<<< HEAD
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<Department> AddAsync(Department department);
        Task<bool> UpdateAsync(Department department);
=======
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto?> GetByEmpIdAsync(int Empid);
        Task<DepartmentDto> AddAsync(DepartmentFormDto dto);

        Task<bool> UpdateAsync(int id, DepartmentFormDto dto);
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        Task<bool> DeleteAsync(int id);
    }

}
