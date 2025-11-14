namespace HRMS.ViewModels.Payroll
{
    public class PayslipDetailsViewModel
    {
        public int PayslipID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedDate { get; set; }

        // Employee Info
        public string EmployeeFullName { get; set; }
        public string EmployeeEmail { get; set; }

        // Details
        public List<PayslipDetailItem> Details { get; set; } = new List<PayslipDetailItem>();
    }

    public class PayslipDetailItem
    {
        public string ComponentName { get; set; }
        public ComponentType ComponentType { get; set; }
        public decimal Amount { get; set; }
    }
}

