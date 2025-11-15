namespace HRMS.DTOs.Employee
{
    public class MyProfileDTO
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BasicSalary { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }
        public string? UserName { get; set; }
        public int Age { get; set; }
        public int YearsOfService { get; set; }
        public int TotalLeaveRequests { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingLeaves { get; set; }
    }
}
