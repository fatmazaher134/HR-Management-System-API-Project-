using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestServices _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public LeaveRequestController(ILeaveRequestServices service, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _service = service;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin,Employee,HR")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("HR") || User.IsInRole("Admin"))
            {
                var model = await _service.GetAllAsync();
                return View("HRIndex", model);
            }
            else // Employee
            {
                var user = await _userManager.GetUserAsync(User);
                var model = await _service.GetMyRequestsAsync(user.Id);
                return View("EmployeeIndex", model);
            }
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await _service.GetMyRequestsAsync(user.Id);
            return View(model);
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var leaveTypes = await _unitOfWork.LeaveType.GetAllAsync();

            var viewModel = new CreateLeaveRequestViewModel
            {
                LeaveTypes = leaveTypes.Select(x => new SelectListItem
                {
                    Value = x.LeaveTypeID.ToString(),
                    Text = x.TypeName
                })
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(model, user.Id);
                return RedirectToAction(nameof(MyRequests));
            }
            Console.WriteLine("Create POST called");

            return View(model);
        }


        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Approve(int id)
        {
            // hear can the hr approve the request
            var user = await _userManager.GetUserAsync(User);
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id);
            await _service.ApproveAsync(id, emp.EmployeeID);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Reject(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id);
            await _service.RejectAsync(id, emp.EmployeeID, "Not eligible");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id);
            await _service.CancelAsync(id, emp.EmployeeID);
            return RedirectToAction(nameof(MyRequests));
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _service.DeleteAsync(id, userId);

            if (!result)
                return NotFound();

            TempData["Success"] = "Leave request deleted successfully.";
            return RedirectToAction(nameof(MyRequests));
        }

    }
}