namespace HRMS.ViewModels.Employee
{
    public class EmployeeDetailsViewModel
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BasicSalary { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties
        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }
        public string? UserName { get; set; }

        // Calculated
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public int YearsOfService => DateTime.Now.Year - HireDate.Year;

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}