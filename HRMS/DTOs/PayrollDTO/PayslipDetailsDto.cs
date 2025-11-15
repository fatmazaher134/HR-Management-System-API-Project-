namespace HRMS.DTOs.PayrollDTO
{
    public class PayslipDetailsDto
    {
        public int PayslipID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedDate { get; set; } 
        public string EmployeeFullName { get; set; }
        public string EmployeeEmail { get; set; }
        public List<PayslipDetailItemDto> Details { get; set; } = new List<PayslipDetailItemDto>();
        
    }
}
