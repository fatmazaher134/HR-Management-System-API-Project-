
﻿using HRMS.ViewModels.Account;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRMS.Services.Impelmentation
{
    public class AccountServic(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager) : IAccountService
    {
        public async Task<IdentityResult> UpdateProfileAsync(ManageAccountViewModel model)
        {
           
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                model.StatusMessage = "Profile update Failed";

                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            model.StatusMessage = "Profile updated successfully";
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            var result = await _userManager.UpdateAsync(user);
            
            return result;

        }
        public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            }
            if (user == null)
            {
                return SignInResult.Failed;
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            bool result = await _userManager.CheckPasswordAsync(
                user,
                model.Password
            );

            if (result)
            {
                await _userManager.ResetAccessFailedCountAsync(user);

                List<Claim> claims = new List<Claim>();
                var roles = await _userManager.GetRolesAsync(user);

                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim("Address", user.Address));
                if (user.Email != null)
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }


                //create cookie idd,username [email | role]
                await _signInManager
                    .SignInWithClaimsAsync(user, model.RememberMe, claims);
                return SignInResult.Success;
            }
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }
            return SignInResult.Failed;

        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address
            };


            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && !string.IsNullOrEmpty(model.SelectedRole))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }
            }
            return result;
        }
        public List<string> GetAllRoles()
        {
            List<IdentityRole> Roles =  _roleManager.Roles.ToList();
            List<string> stringRoles= new();
            foreach (var role in Roles)
            {
                stringRoles.Add(role.Name);
            }
            return stringRoles;
        }
    }
}