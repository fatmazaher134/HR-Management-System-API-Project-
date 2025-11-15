namespace HRMS.DTOs.LeaveRequest
{
    public class LeaveRequestDto
    {
        public int LeaveRequestID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Comments { get; set; }
    }
}
