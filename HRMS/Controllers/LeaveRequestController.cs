<<<<<<< HEAD
﻿using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Controllers
{
    public class LeaveRequestController : Controller
=======
﻿using HRMS.DTOs.LeaveRequest;
using HRMS.Interfaces;
using HRMS.Interfaces.Services;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers.Api
{
    [ApiController]
    [Route("api/LeaveRequest")]
    //[Authorize]
    public class LeaveRequestController : ControllerBase
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    {
        private readonly ILeaveRequestServices _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

<<<<<<< HEAD

        public LeaveRequestController(ILeaveRequestServices service, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
=======
        public LeaveRequestController(
            ILeaveRequestServices service,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        {
            _service = service;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

<<<<<<< HEAD
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

=======
        [HttpGet("GetLeaveRequests/")]
        //[Authorize(Roles = "Admin,Employee,HR")]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequests()
        {
            // var user = await _userManager.GetUserAsync(User);
            IEnumerable<LeaveRequestDto> dtos;

            // if (User.IsInRole("HR") || User.IsInRole("Admin"))
            // {
            dtos = await _service.GetAllAsync();
            // }
            // else // Employee
            // {
            //     dtos = await _service.GetMyRequestsAsync(user.Id);
            // }

            return Ok(dtos);
        }

        [HttpGet("LeaveTypes")]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = await _unitOfWork.LeaveType.GetAllAsync();
            return Ok(leaveTypes.Select(lt => new { lt.LeaveTypeID, lt.TypeName }));
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // var user = await _userManager.GetUserAsync(User);

            var tempTestUserId = "YOUR_TEST_USER_ID_FROM_DATABASE";

            var result = await _service.CreateAsync(createDto, tempTestUserId);

            if (!result)
            {
                return BadRequest(new { message = "Could not create leave request." });
            }

            return Ok(new { message = "Leave request created successfully." });
        }

        [HttpPut("Approve/{id}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Approve(int id)
        {
            // var user = await _userManager.GetUserAsync(User);
            // var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id);
            // if (emp == null) return Unauthorized();

            var tempTestHrEmployeeId = 1; // افترض أن EmployeeID 1 هو HR

            var result = await _service.ApproveAsync(id, tempTestHrEmployeeId);
            if (!result) return NotFound(new { message = "Request not found." });

            return Ok(new { message = "Request Approved" });
        }

        [HttpPut("Reject/{id}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Reject(int id, [FromBody] string? comments)
        {
            // var user = await _userManager.GetUserAsync(User);
            // var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id);
            // if (emp == null) return Unauthorized();

            var tempTestHrEmployeeId = 1; // افترض أن EmployeeID 1 هو HR

            var result = await _service.RejectAsync(id, tempTestHrEmployeeId, comments ?? "Rejected");
            if (!result) return NotFound(new { message = "Request not found." });

            return Ok(new { message = "Request Rejected" });
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            // var userId = _userManager.GetUserId(User);

            var tempTestUserId = "YOUR_TEST_USER_ID_FROM_DATABASE";

            var result = await _service.DeleteAsync(id, tempTestUserId);

            if (!result)
            {
                return NotFound(new { message = "Request not found or user not authorized." });
            }

            return NoContent();
        }
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    }
}