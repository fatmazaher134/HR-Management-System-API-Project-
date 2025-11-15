using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required, MaxLength(100)]
    public required string FirstName { get; set; }

    [Required, MaxLength(100)]
    public required string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required, MaxLength(255)]
    public required string Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(10,2)")]
    public decimal BasicSalary { get; set; }

    public bool IsActive { get; set; } = true; // Soft Delete


    [ForeignKey("JobTitle")]
    public int? JobTitleID { get; set; }

    public JobTitle? JobTitle { get; set; }

    public int? DepartmentID { get; set; }

    [ForeignKey(nameof(DepartmentID))]
    public Department? Department { get; set; }



    [Required]
    public string ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<EmployeeDocument>? Documents { get; set; } = new List<EmployeeDocument>();
    public ICollection<LeaveRequest>? LeaveRequests { get; set; } = new List<LeaveRequest>();
    public ICollection<AttendanceRecord>? AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<Payslip>? Payslips { get; set; } = new List<Payslip>();


}
