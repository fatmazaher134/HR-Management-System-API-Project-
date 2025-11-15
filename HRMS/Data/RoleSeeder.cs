using HRMS.Models;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "HR", "Employee" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                if (await _userManager.FindByEmailAsync("admin@hrms.com") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@hrms.com",
                        FullName = "Admin User",
                        Address = "Cairo",
                        EmailConfirmed = true // Skip email confirmation for the admin
                    };
                    var result = await _userManager.CreateAsync(adminUser, "AdminPassword123!");
                    if (result.Succeeded)
                    {
                        // Assign the "Admin" role to this new user
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

            }
        }
    }
}
