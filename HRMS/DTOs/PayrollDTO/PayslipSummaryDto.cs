namespace HRMS.DTOs.PayrollDTO
{
    public class PayslipSummaryDto
    {
        public int PayslipID { get; set; }
        public string EmployeeFullName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
