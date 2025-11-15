namespace HRMS.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BasicSalary { get; set; }
        public int? DepartmentID { get; set; }
        public int? JobTitleID { get; set; }
        public string? UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
