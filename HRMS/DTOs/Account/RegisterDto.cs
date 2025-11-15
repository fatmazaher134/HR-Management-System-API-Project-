// We only need this 'using' for the validation attributes
using System.ComponentModel.DataAnnotations;

namespace HRMS.Dtos.Account
{
    /// <summary>
    /// DTO for receiving new user registration data.
    /// </summary>
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        // [Compare] is a VALIDATION attribute, not a UI hint,
        // so it absolutely stays in the DTO.
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public string SelectedRole { get; set; }

        // The 'List<SelectListItem> Options' property is completely removed.
    }
}