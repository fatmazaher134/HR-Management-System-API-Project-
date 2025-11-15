using HRMS.DTOs.LeaveRequest;
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
    [Authorize] 
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestServices _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveRequestController(
            ILeaveRequestServices service,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _service = service;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetLeaveRequests/")]
        [Authorize(Roles = "Admin,Employee,HR")] 
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequests()
        {
            var user = await _userManager.GetUserAsync(User); 
            if (user == null) return Unauthorized(); 

            IEnumerable<LeaveRequestDto> dtos;

            if (User.IsInRole("HR") || User.IsInRole("Admin")) 
            {
                dtos = await _service.GetAllAsync();
            }
            else 
            {
                dtos = await _service.GetMyRequestsAsync(user.Id); 
            }

            return Ok(dtos);
        }

        [HttpGet("LeaveTypes")]
        [Authorize(Roles = "Admin,HR,Employee")] 
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = await _unitOfWork.LeaveType.GetAllAsync();
            return Ok(leaveTypes.Select(lt => new { lt.LeaveTypeID, lt.TypeName }));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Employee")] 
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(); 

          
            var result = await _service.CreateAsync(createDto, user.Id);

            if (!result)
            {
                return BadRequest(new { message = "Could not create leave request." });
            }

            return Ok(new { message = "Leave request created successfully." });
        }

        [HttpPut("Approve/{id}")]
        [Authorize(Roles = "Admin,HR")] 
        public async Task<IActionResult> Approve(int id)
        {
            var user = await _userManager.GetUserAsync(User); 
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id); 
            if (emp == null) return Unauthorized(); 


            var result = await _service.ApproveAsync(id, emp.EmployeeID); 
            if (!result) return NotFound(new { message = "Request not found." });

            return Ok(new { message = "Request Approved" });
        }

        [HttpPut("Reject/{id}")]
        [Authorize(Roles = "Admin,HR")] 
        public async Task<IActionResult> Reject(int id, [FromBody] string? comments)
        {
            var user = await _userManager.GetUserAsync(User); 
            var emp = await _unitOfWork.Employee.FindAsync(e => e.ApplicationUserId == user.Id); 
            if (emp == null) return Unauthorized(); 


            var result = await _service.RejectAsync(id, emp.EmployeeID, comments ?? "Rejected"); 
            if (!result) return NotFound(new { message = "Request not found." });

            return Ok(new { message = "Request Rejected" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR,Employee")] 
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User); 
            if (string.IsNullOrEmpty(userId)) return Unauthorized(); 


            var result = await _service.DeleteAsync(id, userId); 

            if (!result)
            {
                return NotFound(new { message = "Request not found or user not authorized." });
            }

            return NoContent();
        }
    }
}