using HRMS.Models;
using HRMS.Dtos; // <-- Import your new DTOs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // Required for List
using System.Linq; // Required for .Select(), .Except()
using System.Threading.Tasks; // Required for Task

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")] // Sets the base route to "api/admin"
    [ApiController]
    public class AdminApiController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) : ControllerBase
    {
        // GET: api/admin
        /// <summary>
        /// Gets a list of all users and their assigned roles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesDtoList = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                var rolesList = await _userManager.GetRolesAsync(user);

                var dto = new UserWithRolesDto
                {
                    UserId = user.Id,
                    Name = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    // Use the DTO's List<string> property
                    Roles = rolesList.ToList()
                };
                userRolesDtoList.Add(dto);
            }

            // Return JSON data instead of a View
            return Ok(userRolesDtoList);
        }

        // GET: api/admin/{userId}/roles
        /// <summary>
        /// Gets a specific user's role assignments for management.
        /// </summary>
        [HttpGet("{userId}/roles")]
        public async Task<ActionResult<ManageUserRolesDto>> GetRolesForUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var dto = new ManageUserRolesDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in allRoles)
            {
                dto.Roles.Add(new RoleAssignmentDto
                {
                    // Assuming RoleAssignmentDto has RoleName and IsAssigned
                    RoleName = role.Name,
                    IsAssigned = userRoles.Contains(role.Name)
                });
            }

            return Ok(dto);
        }

        // PUT: api/admin/{userId}/roles
        /// <summary>
        /// Updates the roles for a specific user.
        /// </summary>
        [HttpPut("{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] UpdateUserRolesDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Get the roles the user currently has
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Get the roles selected in the request
            var selectedRoleNames = dto.RoleNames ?? new List<string>();

            // Roles to ADD: selected roles that are NOT in current roles
            var rolesToAdd = selectedRoleNames.Except(currentRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            // Roles to REMOVE: current roles that are NOT in selected roles
            var rolesToRemove = currentRoles.Except(selectedRoleNames);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            // Return 204 No Content, the standard for a successful PUT
            return NoContent();
        }
    }

    /// <summary>
    /// DTO for updating user roles. The client just needs to send
    /// a list of role names the user should have.
    /// </summary>
    public class UpdateUserRolesDto
    {
        public List<string> RoleNames { get; set; }
    }
}