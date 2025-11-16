using HRMS.DTOs.AttendanceRecordDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/attendance")]
[ApiController]
public class AttendanceRecordsController : ControllerBase
{
    private readonly IAttendanceRecordServices _attendanceService;
    private readonly IEmployeeServices _employeeServices;
    private readonly IMapper _mapper;

    public AttendanceRecordsController(
        IAttendanceRecordServices attendanceService,
        IEmployeeServices employeeServices,
        IMapper mapper)
    {
        _attendanceService = attendanceService;
        _employeeServices = employeeServices;
        _mapper = mapper;
    }

    private async Task<int?> GetCurrentEmployeeIdAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return null;

        var employee = await _employeeServices.GetByUserIdAsync(userId);
        return employee?.EmployeeID;
    }

    // ----------------------------------------------------
    // 1. Employee Clock In/Out (POST)
    // ----------------------------------------------------

    [HttpPost("clock-in")]
    [Authorize(Roles = "Employee")] // Only employees can clock in
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClockIn()
    {
        var empId = await GetCurrentEmployeeIdAsync();
        if (!empId.HasValue) return Unauthorized("Employee record not found for the current user.");

        bool success = await _attendanceService.CheckInAsync(empId.Value);

        if (success)
        {
            return Ok(new { message = "You have successfully clocked in." });
        }

        return BadRequest(new { message = "Failed to clock in. You may already have an active shift." });
    }

    [HttpPost("clock-out")]
    [Authorize(Roles = "Employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClockOut()
    {
        var empId = await GetCurrentEmployeeIdAsync();
        if (!empId.HasValue) return Unauthorized("Employee record not found for the current user.");

        bool success = await _attendanceService.CheckOutAsync(empId.Value);

        if (success)
        {
            return Ok(new { message = "You have successfully clocked out." });
        }

        return BadRequest(new { message = "Failed to clock out. You must clock in first." });
    }

    // ----------------------------------------------------
    // 2. Employee History (GET)
    // ----------------------------------------------------

    [HttpGet("my-history")]
    [Authorize(Roles = "Employee, Admin, HRManager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetMyHistory()
    {
        var empId = await GetCurrentEmployeeIdAsync();
        if (!empId.HasValue) return Unauthorized("Employee record not found for the current user.");

        var records = await _attendanceService.GetByEmployeeIdAsync(empId.Value);

        // Map the entities to DTOs for the response
        var recordsDto = _mapper.Map<IEnumerable<AttendanceRecordDto>>(records);

        return Ok(recordsDto);
    }

    // ----------------------------------------------------
    // 3. Admin/HR CRUD Endpoints
    // ----------------------------------------------------

    [HttpGet] // GET api/attendance (Admin list of all records)
    [Authorize(Roles = "Admin, HRManager")]
    public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetAllAttendance()
    {
        var records = await _attendanceService.GetAllAsync();
        var recordsDto = _mapper.Map<IEnumerable<AttendanceRecordDto>>(records);
        return Ok(recordsDto.OrderByDescending(r => r.CheckInTime));
    }

    [HttpGet("{id:int}")] // GET api/attendance/{id}
    [Authorize(Roles = "Admin, HRManager")]
    public async Task<ActionResult<AttendanceRecordDto>> GetAttendanceById(int id)
    {
        var record = await _attendanceService.GetByIdAsync(id);
        if (record == null) return NotFound();

        var recordDto = _mapper.Map<AttendanceRecordDto>(record);
        return Ok(recordDto);
    }

    [HttpPut("{id:int}")] // PUT api/attendance/{id} (Admin edit)
    [Authorize(Roles = "Admin, HRManager")]
    public async Task<IActionResult> UpdateAttendance(int id, AttendanceRecordForUpdateDto updateDto)
    {
        var existingRecord = await _attendanceService.GetByIdAsync(id);
        if (existingRecord == null) return NotFound();

        _mapper.Map(updateDto, existingRecord);

        if (existingRecord.CheckOutTime.HasValue && existingRecord.CheckInTime >= existingRecord.CheckOutTime.Value)
        {
            return BadRequest(new { message = "Check-Out time must be later than Check-In time." });
        }

        var success = await _attendanceService.UpdateAsync(existingRecord);
        if (!success) return StatusCode(500, new { message = "Failed to update attendance record." });

        return NoContent();
    }

    [HttpDelete("{id:int}")] // DELETE api/attendance/{id}
    [Authorize(Roles = "Admin, HRManager")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        bool success = await _attendanceService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}