using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.Account; // We can remove this
using HRMS.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
// We no longer need Mvc.Rendering
// using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Data;

namespace HRMS.Controllers
{
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // --- Use the DTO we created earlier ---
            var model = new AccountDetailsDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                IsHR = await _userManager.IsInRoleAsync(user, "HR")
            };

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

            var result = await _accountService.LoginUserAsync(model);

            if (result.Succeeded)
            {
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