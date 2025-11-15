using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.Dtos.Dashboard; // Updated namespace
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers.Api
{
    [Authorize] // 1. Authorization is handled at the controller level
    [ApiController] // 2. Identifies this as an API controller
    [Route("api/[controller]")] // 3. Sets the route to /api/dashboard
    public class HomeController : ControllerBase // 4. Inherits from ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeServices _employeeServices;
        private readonly ILeaveRequestServices _leaveRequestServices;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IEmployeeServices employeeServices,
            ILeaveRequestServices leaveRequestServices
            )
        {
            _userManager = userManager;
            _employeeServices = employeeServices;
            _leaveRequestServices = leaveRequestServices;
        }

        [HttpGet] // 5. This action responds to HTTP GET /api/dashboard
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            // 6. We no longer check User.Identity.IsAuthenticated.
            //    The [Authorize] attribute handles it. If not authenticated,
            //    the API will automatically return a 401 Unauthorized.

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // This should rarely happen if [Authorize] is active, but it's good practice
                return Unauthorized();
            }

            var dto = new DashboardDto();

            bool isHR = await _userManager.IsInRoleAsync(user, "HR") ||
                        await _userManager.IsInRoleAsync(user, "Admin");

            if (isHR)
            {
                var allEmployees = await _employeeServices.GetAllAsync();
                dto.TotalEmployees = allEmployees.Count();

                var allRequests = await _leaveRequestServices.GetAllAsync();
                dto.PendingLeaveRequests = allRequests.Count(r => r.Status == "Pending");

                // You could add logic for NewHiresThisMonth here
                // dto.NewHiresThisMonth = ...
            }
            else
            {
                const int annualLeaveBalance = 21;
                int currentYear = DateTime.Now.Year;

                var employee = await _employeeServices.GetByUserIdAsync(user.Id);

                if (employee == null)
                {
                    // 7. Return a 404 Not Found if the user is authenticated 
                    //    but has no corresponding employee record.
                    return NotFound("Employee record not found for this user.");
                }

                var myRequests = await _leaveRequestServices.GetMyRequestsAsync(user.Id);

                var approvedRequestsThisYear = myRequests
                    .Where(r => r.Status == "Approved" && r.StartDate.Year == currentYear)
                    .ToList();

                int totalDaysUsed = 0;
                foreach (var req in approvedRequestsThisYear)
                {
                    // 8. Fixed calculation: (EndDate - StartDate).Days + 1
                    //    This correctly counts inclusive days. (e.g., Nov 15 to Nov 15 is 1 day)
                    totalDaysUsed += (int)(req.EndDate.Date - req.StartDate.Date).TotalDays + 1;
                }

                dto.LeaveBalance = annualLeaveBalance - totalDaysUsed;
                dto.ApprovedLeavesThisYear = approvedRequestsThisYear.Count();
            }

            // 9. Return an HTTP 200 OK with the DTO as JSON
            return Ok(dto);
        }

        // 10. The Privacy() and Error() actions are removed as they are 
        //     specific to MVC views, not a data API.
    }
}