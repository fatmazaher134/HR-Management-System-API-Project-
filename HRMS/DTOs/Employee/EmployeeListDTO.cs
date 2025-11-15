namespace HRMS.DTOs.Employee
{
    public class EmployeeListDTO
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }
}
