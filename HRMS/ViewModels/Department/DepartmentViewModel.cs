using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Department
{
    public class DepartmentViewModel
    {
        [Display(Name = "ID")]
        public int DepartmentID { get; set; }

        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Display(Name = "Manager Name")]
        public string? ManagerName { get; set; }
    }
}
