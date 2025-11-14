using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Employee
{
    public class EmployeeEditBasicInfoViewModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "First name required")]
        [Display(Name = "First name")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name required")]
        [Display(Name = "Last name")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "InValid Phone Number")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [MaxLength(500)]
        public string? Address { get; set; }

        //  (ReadOnly)
        public string Email { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }
        public DateTime HireDate { get; set; }
    }
}
