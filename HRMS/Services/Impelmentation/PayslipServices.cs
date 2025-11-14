using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class PayslipServices : IPayslipServices
    {
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Payslip> GeneratePayslipAsync(int employeeId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payslip>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<Payslip?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
