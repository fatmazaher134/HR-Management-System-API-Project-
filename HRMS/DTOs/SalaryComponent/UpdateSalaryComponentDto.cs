using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs.SalaryComponent
{
    public class UpdateSalaryComponentDto
    {
        [Required, MaxLength(150)]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        public ComponentType ComponentType { get; set; }
    }
}
