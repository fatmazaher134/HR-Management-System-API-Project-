using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Payroll
{
    public class GeneratePayrollViewModel
    {
        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1, 12")]
        public int Month { get; set; } = DateTime.Now.Month;

        [Required]
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
