using HRMS.Interfaces.Services;
using HRMS.Models;

namespace HRMS.Services.Impelmentation
{
    public class PayslipDetailServices : IPayslipDetailServices
    {
        public Task<PayslipDetail> AddAsync(PayslipDetail detail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PayslipDetail?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayslipDetail>> GetByPayslipIdAsync(int payslipId)
        {
            throw new NotImplementedException();
        }
    }
}
