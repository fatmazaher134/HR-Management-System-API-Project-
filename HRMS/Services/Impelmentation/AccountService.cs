// 1. Using statements now reference the DTOs
using HRMS.Dtos.Account;
using HRMS.Interfaces.Services; // Assuming this is where IAccountService is

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRMS.Services.Impelmentation
{
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

        // 7. Signature changed to use RegisterDto
        public async Task<IdentityResult> RegisterUserAsync(RegisterDto model)
            {
            // DTO property names match, so no logic changes needed
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
    List<IdentityRole> Roles = _roleManager.Roles.ToList();
    List<string> stringRoles = new();
    foreach (var role in Roles)
    {
        stringRoles.Add(role.Name);
    }
    return stringRoles;
}
    }
}