using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class LeaveRequest
{
    public int LeaveRequestID { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public DateTime RequestDate { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(50)]
    public required string Status { get; set; } = "Pending";

    [MaxLength(500)]
    public string? Comments { get; set; }

    [ForeignKey(nameof(EmployeeID))]
    [InverseProperty("LeaveRequests")]

    public int? EmployeeID { get; set; }

    [ForeignKey(nameof(LeaveTypeID))]
    public int? LeaveTypeID { get; set; }


    [ForeignKey(nameof(ApprovedByHRID))]
    [InverseProperty("ApprovedLeaveRequests")]

    public int? ApprovedByHRID { get; set; }

    // Navigation
    public Employee? Employee { get; set; }
    public LeaveType? LeaveType { get; set; }
    public Employee? ApprovedByHR { get; set; }
} 

