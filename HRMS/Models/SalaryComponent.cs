using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class SalaryComponent
{
    [Key]
    public int ComponentID { get; set; }

    [Required, MaxLength(150)]
    public required string ComponentName { get; set; }

    [Required]
    public ComponentType ComponentType { get; set; } // Allowance or Deduction

    // Relations
    public ICollection<PayslipDetail>? PayslipDetails { get; set; } = new List<PayslipDetail>();
}
public enum ComponentType
{
    Allowance,
    Deduction
}
