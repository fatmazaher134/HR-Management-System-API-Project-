using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class PayslipDetail
{
    public int PayslipDetailID { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [ForeignKey("Payslip")]
    public int PayslipID { get; set; }

    [ForeignKey("SalaryComponent")]
    public int ComponentID { get; set; }

    // Navigation
    public Payslip? Payslip { get; set; }
    public SalaryComponent? SalaryComponent { get; set; }
}
