using System.ComponentModel.DataAnnotations;

// DTOs are often placed in their own namespace, like .Dtos or .Models.Api
namespace HRMS.Dtos.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        // This property is optional data, so it doesn't need validation
        public bool RememberMe { get; set; }
    }
}