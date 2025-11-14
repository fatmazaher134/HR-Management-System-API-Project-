using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Employee
{
    public class EmployeeFormViewModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "First name required")]
        [Display(Name = "First name")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name required")]
        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Eamil")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "phone Numbe")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [MaxLength(500)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Hiring Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "*")]
        [Display(Name = "Bsic Salary")]
        [Range(0, 9999999.99, ErrorMessage = "Invalid Salary")]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }

        [Display(Name = "Job Title")]
        public int? JobTitleID { get; set; }

        [Display(Name = "Linked Account")]
        public string? UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //  Dropdowns
        public IEnumerable<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> JobTitleList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> UserList { get; set; } = new List<SelectListItem>();
    }
}