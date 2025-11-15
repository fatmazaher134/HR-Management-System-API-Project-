using System.ComponentModel.DataAnnotations;

namespace HRMS.DTOs.SalaryComponent
{
    public class CreateSalaryComponentDto
    {
        [Required, MaxLength(150)]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        public ComponentType ComponentType { get; set; }
    }
}
