using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs.AttendanceRecordDTO
{
    public class AttendanceRecordForUpdateDto
    {
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}