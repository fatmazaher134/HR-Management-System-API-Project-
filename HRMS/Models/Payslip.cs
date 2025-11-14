using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class Payslip
{
    public int PayslipID { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    public int Year { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal GrossSalary { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal NetSalary { get; set; }

    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey("Employee")]
    public int? EmployeeID { get; set; }

    // Navigation
    public Employee? Employee { get; set; }
    public ICollection<PayslipDetail>? PayslipDetails { get; set; } = new List<PayslipDetail>();
}
