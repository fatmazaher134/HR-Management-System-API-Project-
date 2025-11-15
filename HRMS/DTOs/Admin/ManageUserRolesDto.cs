using System.Collections.Generic;

namespace HRMS.Dtos
{
    /// <summary>
    /// DTO for managing a user's roles. Contains user details
    /// and a list of all roles with their assignment status.
    /// </summary>
    public class ManageUserRolesDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        // Use the new DTO
        public List<RoleAssignmentDto> Roles { get; set; }

        public ManageUserRolesDto()
        {
            Roles = new List<RoleAssignmentDto>();
        }
    }
}