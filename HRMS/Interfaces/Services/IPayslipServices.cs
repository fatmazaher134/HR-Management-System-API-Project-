using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IPayslipServices
    {
        Task<IEnumerable<Payslip>> GetAllAsync();
        Task<Payslip?> GetByIdAsync(int id);
        Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId);
        Task<Payslip> GeneratePayslipAsync(int employeeId, int month, int year);
        Task<bool> DeleteAsync(int id);
    }

}
