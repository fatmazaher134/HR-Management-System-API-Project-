using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs.AttendanceRecordDTO
{
    public class AttendanceRecordForCreationDto
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
