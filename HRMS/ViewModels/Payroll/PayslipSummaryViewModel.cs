namespace HRMS.ViewModels.Payroll
{
    public class PayslipSummaryViewModel
    {
        public int PayslipID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string EmployeeFullName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
    }
}
