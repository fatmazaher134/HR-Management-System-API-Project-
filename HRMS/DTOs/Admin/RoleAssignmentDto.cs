namespace HRMS.Dtos
{
    /// <summary>
    /// Represents a role and whether it is assigned to the user.
    /// </summary>
    public class RoleAssignmentDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
    }
}