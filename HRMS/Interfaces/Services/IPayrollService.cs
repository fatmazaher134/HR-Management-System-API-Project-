using HRMS.ViewModels.Payroll;

namespace HRMS.Interfaces.Services
{
    public interface IPayrollService
    {
<<<<<<< HEAD
        Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(int month, int year);

        Task<PayslipDetailsViewModel?> GetPayslipDetailsAsync(int payslipId);

        Task<PayslipDetailsViewModel?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId);
        Task<IEnumerable<PayslipViewModel>> GetMyPayslipsAsync(string applicationUserId);
        Task DeletePayslip(int id);

        Task<IEnumerable<PayslipSummaryViewModel>> GetAllAsync();

        
=======
        Task<IEnumerable<PayslipSummaryDto>> GetAllAsync();

        Task<IEnumerable<PayslipDto>> GetMyPayslipsAsync(string applicationUserId);

        Task<PayslipDetailsDto?> GetPayslipDetailsAsync(int payslipId);

        Task<PayslipDetailsDto?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId);

        Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(GeneratePayrollDto dto);
        Task DeletePayslip(int id);


>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    }
}
