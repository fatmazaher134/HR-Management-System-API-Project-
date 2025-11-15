namespace HRMS.ViewModels.Payroll
{
    public class PayslipViewModel
    {
        public int PayslipID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime PayDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
