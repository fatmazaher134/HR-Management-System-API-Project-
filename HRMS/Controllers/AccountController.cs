using HRMS.Interfaces.Services;
using HRMS.Models;
<<<<<<< HEAD
using HRMS.ViewModels.Account; // We can remove this
using HRMS.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
// We no longer need Mvc.Rendering
// using Microsoft.AspNetCore.Mvc.Rendering; 
=======
using HRMS.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using System.Data;

namespace HRMS.Controllers
{
<<<<<<< HEAD
    // --- API CONTROLLER ATTRIBUTES ---
    [ApiController]
    [Route("api/[controller]")]
    // --- DEFAULT AUTHORIZATION ---
    // All endpoints will require auth unless [AllowAnonymous] is added.
    [Authorize]
    public class AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager) : ControllerBase // --- Changed to ControllerBase
    {

        // --- NEW [HttpGet("profile")] ---
        // This replaces the old Index() GET action.
        // It returns the current user's profile data as JSON.
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
=======
    public class AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

<<<<<<< HEAD
            // --- Use the DTO we created earlier ---
            var model = new AccountDetailsDto
=======
            // Populate the ViewModel with user data
            var model = new ManageAccountViewModel
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
<<<<<<< HEAD
=======
                // Check the user's roles
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
                IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                IsHR = await _userManager.IsInRoleAsync(user, "HR")
            };

<<<<<<< HEAD
            return Ok(model);
        }

        // --- MODIFIED [HttpPost("login")] ---
        [HttpPost("login")]
        [AllowAnonymous] // Allow unauthenticated users to call this
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // [ApiController] handles this, but it's good to double-check
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

=======
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
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            var result = await _accountService.LoginUserAsync(model);

            if (result.Succeeded)
            {
<<<<<<< HEAD
                // The service sets the auth cookie. We just return success.
                return Ok(new { Message = "Login successful." });
            }
            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "Account locked out." });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid login attempt." });
            }
        }

        // --- NEW [HttpGet("roles")] ---
        // Replaces the logic from the old Register[HttpGet]
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoles()
        {
            List<string> Roles = _accountService.GetAllRoles();
            // Just return the list of strings. The client (UI)
            // will build its own dropdown options.
            return Ok(Roles);
        }


        // --- MODIFIED [HttpPost("register")] ---
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "User registered successfully." });
                }

                // --- Return errors as a list for the client ---
                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        // --- MODIFIED [HttpPost("logout")] ---
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            // The service clears the cookie. We just return success.
            return Ok(new { Message = "Logged out successfully." });
        }

        // --- MODIFIED [HttpPut("profile")] ---
        // This replaces the old UpdateProfile() POST action.
        // PUT is more semantically correct for an update.
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // We only need to check model validity
            if (ModelState.IsValid)
            {
                // Call the service using the *logged-in user's ID*
                // This is MORE SECURE and prevents IDOR attacks.
                var result = await _accountService.UpdateProfileAsync(user.Id, model);

                if (result.Succeeded)
                {
                    // No TempData. Just return success.
                    return Ok(new { Message = "Your profile has been updated successfully." });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        // --- The AddErrors() helper method is no longer needed. ---
    }
}
=======
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
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
