using System.Collections.Generic;

namespace HRMS.Dtos
{
    /// <summary>
    /// Represents user details along with their assigned role names.
    /// </summary>
    public class UserWithRolesDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// A list of role names assigned to the user.
        /// Changed from a single string (ViewModel) to a collection (DTO)
        /// for cleaner API data transfer.
        /// </summary>
        public List<string> Roles { get; set; }

        public UserWithRolesDto()
        {
            Roles = new List<string>();
        }
    }
}