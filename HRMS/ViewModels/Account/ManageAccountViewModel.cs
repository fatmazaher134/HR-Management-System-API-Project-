using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Account
{
    public class ManageAccountViewModel
    {
        // For displaying user info
        public string UserId { get; set; }
        public string FullName { get; set; }
        [BindNever]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // For knowing which links to show
        public bool IsAdmin { get; set; }
        public bool IsHR { get; set; }

        // For status messages (e.g., "Profile updated successfully")
        [TempData]
        public string? StatusMessage { get; set; }

    }

}
