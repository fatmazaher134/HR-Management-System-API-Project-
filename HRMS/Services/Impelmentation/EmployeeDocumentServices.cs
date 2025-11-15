using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class EmployeeDocumentServices : IEmployeeDocumentServices
    {
        public Task<EmployeeDocument> AddAsync(EmployeeDocument document)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeDocument>> GetAllByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDocument?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
