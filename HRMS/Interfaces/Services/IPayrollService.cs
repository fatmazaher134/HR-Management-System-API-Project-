using HRMS.ViewModels.Payroll;

namespace HRMS.Interfaces.Services
{
    public interface IPayrollService
    {
        Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(int month, int year);

        Task<PayslipDetailsViewModel?> GetPayslipDetailsAsync(int payslipId);

        Task<PayslipDetailsViewModel?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId);
        Task<IEnumerable<PayslipViewModel>> GetMyPayslipsAsync(string applicationUserId);
        Task DeletePayslip(int id);

        Task<IEnumerable<PayslipSummaryViewModel>> GetAllAsync();

        
    }
}
