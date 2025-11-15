using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

public class EmployeeDocument
{
    [Key]
    public int DocumentID { get; set; }

    [Required, MaxLength(200)]
    public required string DocumentName { get; set; }

    [Required, MaxLength(500)]
    public required string FilePath { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }

    // Navigation
    public Employee? Employee { get; set; }
}
