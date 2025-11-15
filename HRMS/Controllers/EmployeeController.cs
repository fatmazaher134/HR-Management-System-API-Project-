using AutoMapper;
using HRMS.DTOs.Employee;
using HRMS.Interfaces.Services;
using HRMS.Models.Enums;
using HRMS.ViewModels;
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IJobTitleServices _jobTitleServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeServices employeeServices,
            IDepartmentServices departmentServices,
            IJobTitleServices jobTitleServices,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
            _jobTitleServices = jobTitleServices;
            _userManager = userManager;
            _mapper = mapper;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new UnauthorizedAccessException("User not authenticated");
        }

        // GET: api/Employee
        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ResponseViewModel<IEnumerable<EmployeeListDTO>>>> GetAll()
        {
            try
            {
                // Service returns DTOs directly
                var dtos = await _employeeServices.GetAllAsync();

                return Ok(ResponseViewModel<IEnumerable<EmployeeListDTO>>.Success(dtos, "Employees retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<IEnumerable<EmployeeListDTO>>.Failure(
                    "An error occurred while retrieving employees",
                    ErrorCode.ServerError));
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<ResponseViewModel<EmployeeDetailsDTO>>> GetById(int id)
        {
            try
            {
                // Service returns DTO directly
                var dto = await _employeeServices.GetByIdAsync(id);

                if (dto == null)
                {
                    return NotFound(ResponseViewModel<EmployeeDetailsDTO>.NotFound("Employee not found"));
                }

                // Employee can only view their own details
                if (User.IsInRole("Employee"))
                {
                    var currentUserId = GetCurrentUserId();
                    if (dto.ApplicationUserId != currentUserId)
                    {
                        return StatusCode(403, ResponseViewModel<EmployeeDetailsDTO>.Unauthorized("Access denied"));
                    }
                }

                return Ok(ResponseViewModel<EmployeeDetailsDTO>.Success(dto, "Employee details retrieved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ResponseViewModel<EmployeeDetailsDTO>.Unauthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<EmployeeDetailsDTO>.Failure(
                    "An error occurred while retrieving employee details",
                    ErrorCode.ServerError));
            }
        }

        // POST: api/Employee
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ResponseViewModel<string>>> Create([FromBody] EmployeeFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<string>.ValidationError("Invalid data"));
                }

                if (await _employeeServices.IsEmailExistsAsync(model.Email))
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "The Email is Already Used",
                        ErrorCode.EmailAlreadyExists));
                }

                // Map ViewModel to DTO
                var dto = _mapper.Map<CreateEmployeeDTO>(model);

                // Service handles DTO to Model mapping
                var result = await _employeeServices.RegisterEmployeeAsync(dto);

                if (result.Succeeded)
                {
                    return Ok(ResponseViewModel<string>.Success("Employee created successfully"));
                }

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(ResponseViewModel<string>.Failure(errors, ErrorCode.ValidationError));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred while creating the employee",
                    ErrorCode.ServerError));
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ResponseViewModel<string>>> Update(int id, [FromBody] EmployeeFormViewModel model)
        {
            try
            {
                // Check if employee exists
                var existingDto = await _employeeServices.GetByIdAsync(id);

                if (existingDto == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Employee not found"));
                }

                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<string>.ValidationError("Invalid data"));
                }

                if (await _employeeServices.IsEmailExistsAsync(model.Email, model.EmployeeID))
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "Email already exists",
                        ErrorCode.EmailAlreadyExists));
                }

                // Map ViewModel to DTO
                var updateDto = _mapper.Map<UpdateEmployeeDTO>(model);
                updateDto.EmployeeID = id;

                // Service handles DTO to Model mapping
                var updateResult = await _employeeServices.UpdateAsync(updateDto);

                // Update ApplicationUser
                if (existingDto.ApplicationUserId != null)
                {
                    var appUser = await _userManager.FindByIdAsync(existingDto.ApplicationUserId);
                    if (appUser != null)
                    {
                        appUser.Email = model.Email;
                        appUser.UserName = model.Email;
                        appUser.FullName = $"{model.FirstName} {model.LastName}";
                        appUser.Address = model.Address;
                        appUser.PhoneNumber = model.PhoneNumber;

                        var appUserUpdateResult = await _userManager.UpdateAsync(appUser);

                        if (!appUserUpdateResult.Succeeded)
                        {
                            var errors = string.Join(", ", appUserUpdateResult.Errors.Select(e => e.Description));
                            return BadRequest(ResponseViewModel<string>.Failure(errors, ErrorCode.OperationFailed));
                        }
                    }
                }

                if (!updateResult)
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "Failed to update employee",
                        ErrorCode.OperationFailed));
                }

                return Ok(ResponseViewModel<string>.Success("Employee updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred while updating the employee",
                    ErrorCode.ServerError));
            }
        }

        // PUT: api/Employee/UpdateBasicInfo/5
        [HttpPut("UpdateBasicInfo/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<ResponseViewModel<string>>> UpdateBasicInfo(int id, [FromBody] EmployeeEditBasicInfoViewModel model)
        {
            try
            {
                var existingDto = await _employeeServices.GetByIdAsync(id);
                if (existingDto == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Employee not found"));
                }

                var currentUserId = GetCurrentUserId();
                if (existingDto.ApplicationUserId != currentUserId)
                {
                    return StatusCode(403, ResponseViewModel<string>.Unauthorized("Access denied"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<string>.ValidationError("Invalid data"));
                }

                // Map ViewModel to DTO
                var updateDto = _mapper.Map<UpdateEmployeeBasicInfoDTO>(model);
                updateDto.EmployeeID = id;

                // Service handles DTO to Model mapping
                var updated = await _employeeServices.UpdateBasicInfoAsync(id, updateDto);

                if (!updated)
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "Failed to update employee information",
                        ErrorCode.OperationFailed));
                }

                return Ok(ResponseViewModel<string>.Success("Your data has been updated successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ResponseViewModel<string>.Unauthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred during the update",
                    ErrorCode.ServerError));
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ResponseViewModel<string>>> Delete(int id)
        {
            try
            {
                var dto = await _employeeServices.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Employee not found"));
                }

                var user = await _userManager.FindByIdAsync(dto.ApplicationUserId);
                if (user == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("User not found"));
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(ResponseViewModel<string>.Failure(errors, ErrorCode.OperationFailed));
                }

                return Ok(ResponseViewModel<string>.Success("Employee deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred while deleting the employee",
                    ErrorCode.ServerError));
            }
        }

        // GET: api/Employee/MyProfile
        [HttpGet("MyProfile")]
        [Authorize(Roles = "Employee,HR,Admin")]
        public async Task<ActionResult<ResponseViewModel<MyProfileDTO>>> GetMyProfile()
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                // Service returns DTO directly
                var dto = await _employeeServices.GetByUserIdAsync(currentUserId);

                if (dto == null)
                {
                    return NotFound(ResponseViewModel<MyProfileDTO>.NotFound("Employee data not found"));
                }

                return Ok(ResponseViewModel<MyProfileDTO>.Success(dto, "Profile retrieved successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ResponseViewModel<MyProfileDTO>.Unauthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<MyProfileDTO>.Failure(
                    "An error occurred while retrieving profile",
                    ErrorCode.ServerError));
            }
        }
    }
}