using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class Department
{
    public int DepartmentID { get; set; }

    [Required, MaxLength(150)]
    public required string DepartmentName { get; set; }

    public int? ManagerID { get; set; }

    [ForeignKey(nameof(ManagerID))]
    public Employee? Manager { get; set; }

    // Relations
    public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
}
