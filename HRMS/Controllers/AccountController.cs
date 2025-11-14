using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace HRMS.Controllers
{
    public class AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Populate the ViewModel with user data
            var model = new ManageAccountViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Check the user's roles
                IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                IsHR = await _userManager.IsInRoleAsync(user, "HR")
            };

            return View(model);
        }

        //Register Action
        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            await Logout();
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _accountService.LoginUserAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "This account has been locked out. Please try again later.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            List<string> Roles = _accountService.GetAllRoles();

            List<SelectListItem> roleOptions = Roles
                .Select(role => new SelectListItem { Text = role, Value = role })
                .ToList();

            RegisterViewModel registerViewModel = new()
            {
                Options = roleOptions
            };
            return View("Register", registerViewModel);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ModelState.Remove(nameof(model.Options));
            if (ModelState.IsValid)
            {

                var result = await _accountService.RegisterUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                AddErrors(result);
            }

            List<string> Roles = _accountService.GetAllRoles();

            List<SelectListItem> roleOptions = Roles
                .Select(role => new SelectListItem { Text = role, Value = role })
                .ToList();

            RegisterViewModel registerViewModel = new()
            {
                Options = roleOptions
            };
            return View("Register", registerViewModel);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ManageAccountViewModel manageAccount)
        {
            // 1. Get Current Logged-in User for Security Checks
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return NotFound();

            // 2. SECURITY GUARDRAIL: IDOR Protection
            // If the ID in the form does not match the logged-in user, AND they are not an Admin...
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (manageAccount.UserId != currentUser.Id && !isAdmin)
            {
                return Forbid(); // Stop the request immediately.
            }
            ModelState.Remove(nameof(manageAccount.UserName));
            var userToPush = await _userManager.FindByIdAsync(manageAccount.UserId);
            if (userToPush != null)
            {
                manageAccount.UserName = userToPush.UserName; // <--- Re-populate this!
            }
            if (ModelState.IsValid)
            {
                // 3. Call your Service
                var result = await _accountService.UpdateProfileAsync(manageAccount);

                if (result.Succeeded)
                {
                    // 4. Use TempData so the message survives the Redirect
                    TempData["StatusMessage"] = "Your profile has been updated successfully.";
                    
                    return RedirectToAction("Index");
                }

                AddErrors(result);
            }
            var userToReload = await _userManager.FindByIdAsync(manageAccount.UserId);
            if (userToReload != null)
            {
                manageAccount.UserName = userToReload.UserName; // <--- Re-populate this!
            }
            // 5. If we got here, something failed. 
            // We MUST re-populate the Roles for the sidebar, or the layout will break.
            manageAccount.IsAdmin = isAdmin;
            manageAccount.IsHR = await _userManager.IsInRoleAsync(currentUser, "HR");

            return View("Index", manageAccount);
        }
    }
}
