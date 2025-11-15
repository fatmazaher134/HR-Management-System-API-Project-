using HRMS.ViewModels.Payroll;

namespace HRMS.Interfaces.Services
{
    public interface IPayrollService
    {
        Task<IEnumerable<PayslipSummaryDto>> GetAllAsync();

        Task<IEnumerable<PayslipDto>> GetMyPayslipsAsync(string applicationUserId);

        Task<PayslipDetailsDto?> GetPayslipDetailsAsync(int payslipId);

        Task<PayslipDetailsDto?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId);

        Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(GeneratePayrollDto dto);
        Task DeletePayslip(int id);


    }
}
