using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IPayslipDetailServices
    {
        Task<IEnumerable<PayslipDetail>> GetByPayslipIdAsync(int payslipId);
        Task<PayslipDetail?> GetByIdAsync(int id);
        Task<PayslipDetail> AddAsync(PayslipDetail detail);
        Task<bool> DeleteAsync(int id);
    }

}
