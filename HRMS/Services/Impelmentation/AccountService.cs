<<<<<<< HEAD
// 1. Using statements now reference the DTOs
using HRMS.Dtos.Account;
using HRMS.Interfaces.Services; // Assuming this is where IAccountService is
=======

﻿using HRMS.ViewModels.Account;

>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRMS.Services.Impelmentation
{
<<<<<<< HEAD
    // Note: Your class had a typo "AccountServic(" - I've corrected
    // this to a standard constructor.
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // 2. Signature changed to use UpdateProfileDto

        public async Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileDto model)
        {
            // 3. User is found by the 'userId' parameter, not from the model
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // 4. Removed 'model.StatusMessage'. The service layer
                //    should not set UI properties. It just returns the result.
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // 5. Removed 'model.StatusMessage'
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;

            // The ViewModel had 'PhoneNumber' but your original code
            // wasn't updating it, so I've matched that logic.
            // If you *do* want to update it, add it to the DTO and uncomment below:
            // user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            return result;
        }
        // 6. Signature changed to use LoginDto
        public async Task<SignInResult> LoginUserAsync(LoginDto model)
        {
            // The DTO property names match the ViewModel,
            // so no other logic changes are needed here.
=======
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
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
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

<<<<<<< HEAD
=======

                //create cookie idd,username [email | role]
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
                await _signInManager
                    .SignInWithClaimsAsync(user, model.RememberMe, claims);
                return SignInResult.Success;
            }
<<<<<<< HEAD

=======
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }
            return SignInResult.Failed;
<<<<<<< HEAD
=======

>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

<<<<<<< HEAD
        // 7. Signature changed to use RegisterDto
        public async Task<IdentityResult> RegisterUserAsync(RegisterDto model)
            {
            // DTO property names match, so no logic changes needed
=======
        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address
            };

<<<<<<< HEAD
        var result = await _userManager.CreateAsync(user, model.Password);
=======

            var result = await _userManager.CreateAsync(user, model.Password);
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e

            if (result.Succeeded && !string.IsNullOrEmpty(model.SelectedRole))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }
<<<<<<< HEAD
}
return result;
        }

        public List<string> GetAllRoles()
{
    List<IdentityRole> Roles = _roleManager.Roles.ToList();
    List<string> stringRoles = new();
    foreach (var role in Roles)
    {
        stringRoles.Add(role.Name);
    }
    return stringRoles;
}
=======
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
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    }
}