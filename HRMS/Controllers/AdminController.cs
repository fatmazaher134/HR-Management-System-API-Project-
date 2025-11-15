using HRMS.Models;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;

namespace HRMS.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) : Controller
    {

        [HttpGet("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModelList = new List<UserWithRolesViewModel>();
            foreach (var user in users)
            {
                var rolesList = await _userManager.GetRolesAsync(user);

                var viewModel = new UserWithRolesViewModel
                {
                    UserId = user.Id,
                    Name = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    // Join all roles into a single string
                    Roles = string.Join(", ", rolesList)
                };
                userRolesViewModelList.Add(viewModel);
            }
            return View(userRolesViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
            {
                return NotFound();
            }

            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            // Get all roles
            var allRoles = await _roleManager.Roles.ToListAsync();
            // Get roles this user already has
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in allRoles)
            {
                model.Roles.Add(new UserRoleCheckboxViewModel
                {
                    RoleName = role.Name,
                    // Check if the user has this role
                    IsSelected = userRoles.Contains(role.Name)
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Get the roles the user currently has
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Get the roles selected in the form
            var selectedRoleNames = model.Roles
                .Where(r => r.IsSelected)
                .Select(r => r.RoleName);

            // Roles to ADD: selected roles that are NOT in current roles
            var rolesToAdd = selectedRoleNames.Except(currentRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            // Roles to REMOVE: current roles that are NOT in selected roles
            var rolesToRemove = currentRoles.Except(selectedRoleNames);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToAction(nameof(Index));
        }
    }
}
