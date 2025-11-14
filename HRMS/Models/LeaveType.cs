using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class LeaveType
{
    public int LeaveTypeID { get; set; }

    [Required, MaxLength(100)]
    public required string TypeName { get; set; }

    public int DefaultBalance { get; set; }

    // Relations
    public ICollection<LeaveRequest>? LeaveRequests { get; set; } = new List<LeaveRequest>();
}
