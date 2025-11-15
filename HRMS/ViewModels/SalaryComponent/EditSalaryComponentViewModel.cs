using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.SalaryComponent
{
    public class EditSalaryComponentViewModel
    {
        [Required]
        public int ComponentID { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Component Type")]
        public ComponentType ComponentType { get; set; }
    }
}
