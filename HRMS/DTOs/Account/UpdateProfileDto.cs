using System.ComponentModel.DataAnnotations;

namespace HRMS.Dtos.Account
{
    /// <summary>
    /// DTO for *receiving* profile update data from a client.
    /// </summary>
    public class UpdateProfileDto
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }


        // Phone number might be optional, so '?' is often used
        public string? PhoneNumber { get; set; }
    }
}