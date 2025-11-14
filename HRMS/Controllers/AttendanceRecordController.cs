using HRMS.Interfaces.Services;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Authorize]
    public class AttendanceRecordController : Controller
    {
        private readonly IAttendanceRecordServices _attendanceService;
        private readonly IEmployeeServices _employeeServices;

        public AttendanceRecordController(
            IAttendanceRecordServices attendanceService,
            IEmployeeServices employeeServices)
        {
            _attendanceService = attendanceService;
            _employeeServices = employeeServices;
        }

        // Helper to get the EmployeeID (resolve from logged-in ApplicationUser.Id claim)
        // Returns null if no matching Employee is found for the current user.
        private async Task<int?> GetCurrentEmployeeIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return null;

            var employee = await _employeeServices.GetByUserIdAsync(userId);
            return employee?.EmployeeID;
        }

        // GET: Employee's Attendance Dashboard
        
        public async Task<IActionResult> Index()
        {
            var empId = await GetCurrentEmployeeIdAsync();
            if (!empId.HasValue)
            {
                TempData["ErrorMessage"] = "Employee record not found for the current user.";
                return RedirectToAction("AccessDenied", "Account"); // or return Forbid()
            }

            var records = await _attendanceService.GetByEmployeeIdAsync(empId.Value);
            return View(records);
        }

        // Admin/HR: full attendance list (includes Employee navigation)
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> List()
        {
            var records = await _attendanceService.GetAllAsync();
            return View(records);
        }

        // DETAILS action — returns view for a single attendance record
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> Details(int id)
        {
            var record = await _attendanceService.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            // If current user is an Employee, ensure they only view their own records
            if (User.IsInRole("Employee"))
            {
                var empId = await GetCurrentEmployeeIdAsync();
                if (!empId.HasValue || record.EmployeeID != empId.Value)
                {
                    return Forbid();
                }
            }

            return View(record);
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _attendanceService.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            if (User.IsInRole("Employee"))
            {
                var empId = await GetCurrentEmployeeIdAsync();
                if (!empId.HasValue || record.EmployeeID != empId.Value)
                    return Forbid();
            }

            return View(record);
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendanceRecord model)
        {
            if (id != model.AttendanceID)
                return BadRequest();

            var existing = await _attendanceService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (User.IsInRole("Employee"))
            {
                var empId = await GetCurrentEmployeeIdAsync();
                if (!empId.HasValue || existing.EmployeeID != empId.Value)
                    return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            existing.CheckInTime = DateTime.SpecifyKind(model.CheckInTime, DateTimeKind.Local).ToUniversalTime();

            if (model.CheckOutTime.HasValue)
                existing.CheckOutTime = DateTime.SpecifyKind(model.CheckOutTime.Value, DateTimeKind.Local).ToUniversalTime();
            else
                existing.CheckOutTime = null;

            existing.Date = existing.CheckInTime.Date;

            var updated = await _attendanceService.UpdateAsync(existing);
            if (!updated)
            {
                ModelState.AddModelError(string.Empty, "Failed to update attendance record.");
                return View(existing);
            }

            TempData["SuccessMessage"] = "Attendance record updated successfully.";

            if (User.IsInRole("Employee"))
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(List));
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _attendanceService.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            if (User.IsInRole("Employee"))
            {
                var empId = await GetCurrentEmployeeIdAsync();
                if (!empId.HasValue || record.EmployeeID != empId.Value)
                    return Forbid();
            }

            return View(record);
        }

        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _attendanceService.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            if (User.IsInRole("Employee"))
            {
                var empId = await GetCurrentEmployeeIdAsync();
                if (!empId.HasValue || record.EmployeeID != empId.Value)
                    return Forbid();
            }

            var deleted = await _attendanceService.DeleteAsync(id);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Failed to delete the attendance record.";
                if (User.IsInRole("Employee"))
                    return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(List));
            }

            TempData["SuccessMessage"] = "Attendance record deleted successfully.";
            if (User.IsInRole("Employee"))
                return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockIn()
        {
            var empId = await GetCurrentEmployeeIdAsync();
            if (!empId.HasValue)
            {
                TempData["ErrorMessage"] = "Employee record not found for the current user.";
                return RedirectToAction(nameof(Index));
            }

            bool success = await _attendanceService.CheckInAsync(empId.Value);

            if (success)
            {
                TempData["SuccessMessage"] = "You have successfully clocked in.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to clock in. You may already have an active shift.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockOut()
        {
            var empId = await GetCurrentEmployeeIdAsync();
            if (!empId.HasValue)
            {
                TempData["ErrorMessage"] = "Employee record not found for the current user.";
                return RedirectToAction(nameof(Index));
            }

            bool success = await _attendanceService.CheckOutAsync(empId.Value);

            if (success)
            {
                TempData["SuccessMessage"] = "You have successfully clocked out.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to clock out. You must clock in first.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}