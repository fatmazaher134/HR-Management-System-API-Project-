using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs.LeaveRequest
{
    public class CreateLeaveRequestDto
    {
        [Required]
        public int LeaveTypeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Comments { get; set; }
    }
}
