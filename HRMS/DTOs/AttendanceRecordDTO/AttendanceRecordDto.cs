namespace HRMS.DTOs.AttendanceRecordDTO
{
    public class AttendanceRecordDto
    {
        public int AttendanceID { get; set; }
        public string? EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
