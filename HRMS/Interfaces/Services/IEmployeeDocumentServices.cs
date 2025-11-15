using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IEmployeeDocumentServices
    {
        Task<IEnumerable<EmployeeDocument>> GetAllByEmployeeIdAsync(int employeeId);
        Task<EmployeeDocument?> GetByIdAsync(int id);
        Task<EmployeeDocument> AddAsync(EmployeeDocument document);
        Task<bool> DeleteAsync(int id);
    }

}
