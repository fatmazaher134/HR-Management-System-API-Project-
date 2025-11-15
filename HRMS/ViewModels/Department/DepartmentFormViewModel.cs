using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Department
{
    public class DepartmentFormViewModel
    {
        public int DepartmentID { get; set; } 

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(150)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Display(Name = "Department Manager")]
        public int? ManagerID { get; set; } 

        public IEnumerable<SelectListItem>? ManagerList { get; set; }
    }
}
