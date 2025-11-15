namespace HRMS.DTOs.PayrollDTO
{
    public class PayslipDto
    {
        public int PayslipID { get; set; }
        public string EmployeeName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime PayDate { get; set; }
    }
}
