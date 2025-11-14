using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class AttendanceRecord
{
    [Key]
    public int AttendanceID { get; set; }

    [Required]
    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }

    // Navigation
    public Employee? Employee { get; set; }
}
