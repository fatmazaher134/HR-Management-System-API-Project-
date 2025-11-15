using HRMS.Interfaces.Services; 
using HRMS.Models;
using HRMS.ViewModels;
using HRMS.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS.Controllers
{
    public class HomeController : Controller
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

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            var model = new DashboardViewModel(); 

            bool isHR = await _userManager.IsInRoleAsync(user, "HR") ||
                        await _userManager.IsInRoleAsync(user, "Admin");

            if (isHR)
            {

                var allEmployees = await _employeeServices.GetAllAsync();
                model.TotalEmployees = allEmployees.Count();

                var allRequests = await _leaveRequestServices.GetAllAsync();
                model.PendingLeaveRequests = allRequests.Count(r => r.Status == "Pending");
            }
            else
            {


                const int annualLeaveBalance = 21;
                int currentYear = DateTime.Now.Year;

                var employee = await _employeeServices.GetByUserIdAsync(user.Id);

                if (employee != null)
                {
                    var myRequests = await _leaveRequestServices.GetMyRequestsAsync(user.Id);

                    var approvedRequestsThisYear = myRequests
                        .Where(r => r.Status == "Approved" && r.StartDate.Year == currentYear)
                        .ToList();

                    int totalDaysUsed = 0;
                    foreach (var req in approvedRequestsThisYear)
                    {
                        totalDaysUsed += (req.EndDate - req.StartDate).Days ;
                    }


                    model.LeaveBalance = annualLeaveBalance - totalDaysUsed;

                    model.ApprovedLeavesThisYear = approvedRequestsThisYear.Count();
                }
                else
                {
                    model.LeaveBalance = annualLeaveBalance;
                    model.ApprovedLeavesThisYear = 0;
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}