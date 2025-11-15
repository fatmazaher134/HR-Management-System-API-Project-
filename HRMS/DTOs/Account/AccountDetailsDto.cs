// DTOs don't need UI-specific 'using' statements
// like Microsoft.AspNetCore.Mvc

namespace HRMS.Dtos.Account
{
    /// <summary>
    /// DTO for sending account details to a client.
    /// </summary>
    public class AccountDetailsDto
    {
        // These are the core data properties
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // These properties are data that a client (like a 
        // JavaScript app) would use to make UI decisions.
        public bool IsAdmin { get; set; }
        public bool IsHR { get; set; }

        // StatusMessage is removed.
    }
}