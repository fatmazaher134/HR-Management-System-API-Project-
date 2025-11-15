<<<<<<< HEAD
﻿
=======
﻿using AutoMapper;
using HRMS.DTOs.Employee;
using HRMS.Interfaces.Services;
using HRMS.Models.Enums;
using HRMS.ViewModels;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;



namespace HRMS.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
=======
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IJobTitleServices _jobTitleServices;
        private readonly UserManager<ApplicationUser> _userManager;
<<<<<<< HEAD
=======
        private readonly IMapper _mapper;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e

        public EmployeeController(
            IEmployeeServices employeeServices,
            IDepartmentServices departmentServices,
            IJobTitleServices jobTitleServices,
<<<<<<< HEAD
            UserManager<ApplicationUser> userManager)
=======
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
            _jobTitleServices = jobTitleServices;
            _userManager = userManager;
<<<<<<< HEAD
        }


        private async Task<(IEnumerable<SelectListItem> departments,
                            IEnumerable<SelectListItem> jobTitles,
                            IEnumerable<SelectListItem> users)> GetSelectListsAsync()
        {
            // Departments
            var departments = (await _departmentServices.GetAllAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentID.ToString(),
                    Text = d.DepartmentName
                });

            // Job Titles
            var jobTitles = (await _jobTitleServices.GetAllAsync())
                .Select(j => new SelectListItem
                {
                    Value = j.JobTitleID.ToString(),
                    Text = j.TitleName
                });

            // Users (اللي ملهمش Employee مرتبط بيهم)
            var allUsers = await _userManager.Users.ToListAsync();
            var allEmployees = await _employeeServices.GetAllAsync();
            var usedUserIds = allEmployees.Where(e => e.ApplicationUserId != null).Select(e => e.ApplicationUserId).ToList();

            var availableUsers = allUsers
                .Where(u => !usedUserIds.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName + " (" + u.Email + ")"
                });

            return (departments, jobTitles, availableUsers);
        }

       
=======
            _mapper = mapper;
        }

>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new UnauthorizedAccessException("User not authenticated");
        }

<<<<<<< HEAD
       
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeServices.GetAllAsync();

            var model = employees.Select(e => new EmployeeListViewModel
            {
                EmployeeID = e.EmployeeID,
                FullName = $"{e.FirstName} {e.LastName}",
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                DepartmentName = e.Department?.DepartmentName ?? "N/A",
                JobTitleName = e.JobTitle?.TitleName ?? "N/A",
                BasicSalary = e.BasicSalary,
                HireDate = e.HireDate,
                IsActive = e.IsActive
            }).ToList();

            return View(model);
        }


        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeServices.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            if (User.IsInRole("Employee"))
            {
                var currentUserId = GetCurrentUserId();
                if (employee.ApplicationUserId != currentUserId)
                {
                    return Forbid(); // أو RedirectToAction("AccessDenied", "Account")
                }
            }

            var model = new EmployeeDetailsViewModel
            {
                EmployeeID = employee.EmployeeID,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                BasicSalary = employee.BasicSalary,
                IsActive = employee.IsActive,
                DepartmentName = employee.Department?.DepartmentName ?? "N/A",
                JobTitleName = employee.JobTitle?.TitleName ?? "N/A",
                UserName = employee.ApplicationUser?.UserName ?? "N/A"
            };

            return View(model);
        }


        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var (departments, jobTitles, users) = await GetSelectListsAsync();

            var model = new EmployeeFormViewModel
            {
                DepartmentList = departments,
                JobTitleList = jobTitles,
                UserList = users,
                HireDate = DateTime.Now
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var (depts, jobs, usrs) = await GetSelectListsAsync();
                model.DepartmentList = depts;
                model.JobTitleList = jobs;
                model.UserList = usrs;
                return View(model);
            }

            if (await _employeeServices.IsEmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "The Email is Already Used");
                var (depts, jobs, usrs) = await GetSelectListsAsync();
                model.DepartmentList = depts;
                model.JobTitleList = jobs;
                model.UserList = usrs;
                return View(model);
            }

            IdentityResult result = await _employeeServices.RegisterEmployeeAsync(model);

            if (result.Succeeded)
            {
                TempData["Success"] = "Employee Added Successfully";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var (departments, jobTitles, users) = await GetSelectListsAsync();
            model.DepartmentList = departments;
            model.JobTitleList = jobTitles;
            model.UserList = users;
            return View(model);
        }

          

        [Authorize(Roles = "Admin,HR,Employee")]  //  السماح للجميع
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeServices.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            //  Check: هل الموظف يحاول يعدل بيانات غيره؟
            if (User.IsInRole("Employee"))
            {
                var currentUserId = GetCurrentUserId();
                if (employee.ApplicationUserId != currentUserId)
                {
                    return Forbid(); 
                }

                //Emp Edit Basic info
                var basicModel = new EmployeeEditBasicInfoViewModel
                {
                    EmployeeID = employee.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    Email = employee.Email,
                    DepartmentName = employee.Department?.DepartmentName,
                    JobTitleName = employee.JobTitle?.TitleName,
                    HireDate = employee.HireDate
                };

                return View("EditBasicInfo", basicModel);
            }

            // Admin & HR Can Edit on All
            var (departments, jobTitles, users) = await GetSelectListsAsync();

            var model = new EmployeeFormViewModel
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                BasicSalary = employee.BasicSalary,
                DepartmentID = employee.DepartmentID,
                JobTitleID = employee.JobTitleID,
                UserId = employee.ApplicationUserId,
                DepartmentList = departments,
                JobTitleList = jobTitles,
                UserList = users
            };

            return View(model);
        }


        [Authorize(Roles = "Admin,HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeFormViewModel model)
        {
            //if (id != model.EmployeeID)
            //    return BadRequest();

            var employee = await _employeeServices.GetByIdAsync(id);

            // 1. Add 'NotFound' check
            if (employee == null)
                return NotFound();

            ModelState.Remove(nameof(model.Password));
            ModelState.Remove(nameof(model.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                // 2. Use helper method for populating lists
                await PopulateSelectListsAsync(model);
                return View(model);
            }

            // Check: Email مش متكرر
            if (await _employeeServices.IsEmailExistsAsync(model.Email, model.EmployeeID))
            {
                ModelState.AddModelError("Email", "Email already exists");
                await PopulateSelectListsAsync(model);
                return View(model);
            }
            // --- End AppUser Update Logic ---

            // Map properties to the Employee entity
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Address = model.Address;
            employee.DateOfBirth = model.DateOfBirth;
            employee.HireDate = model.HireDate;
            employee.BasicSalary = model.BasicSalary;
            employee.DepartmentID = model.DepartmentID;
            employee.JobTitleID = model.JobTitleID;

            var employeeUpdateResult = await _employeeServices.UpdateAsync(employee);
            // --- Begin AppUser Update Logic ---
            var appUser = employee.ApplicationUser;
            IdentityResult appUserUpdateResult = null; // Initialize to null

            // 3. Move all AppUser logic inside the null check
            if (appUser != null)
            {
                appUser.Email = model.Email;
                appUser.UserName = model.Email;
                appUser.FullName = $"{model.FirstName} {model.LastName}";
                appUser.Address = model.Address;
                appUser.PhoneNumber = model.PhoneNumber;

                appUserUpdateResult = await _userManager.UpdateAsync(appUser);

                if (!appUserUpdateResult.Succeeded)
                {
                    // 4. Add specific Identity errors to ModelState
                    AddIdentityErrorsToModelState(appUserUpdateResult);
                    await PopulateSelectListsAsync(model);
                    return View(model);
                }
            }
            

            //var employee = new Employee
            //{
            //    EmployeeID = model.EmployeeID,
            //    FirstName = model.FirstName,
            //    LastName = model.LastName,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //    Address = model.Address,
            //    DateOfBirth = model.DateOfBirth,
            //    HireDate = model.HireDate,
            //    BasicSalary = model.BasicSalary,
            //    DepartmentID = model.DepartmentID,
            //    JobTitleID = model.JobTitleID,
            //    ApplicationUserId = model.UserId,
            //    IsActive = true
            //};


            // Check only the employee update result.
            // The AppUser result was already checked (or skipped if appUser was null).
            if (!employeeUpdateResult)
            {
                ModelState.AddModelError("", "An error occurred while updating the employee record.");
                await PopulateSelectListsAsync(model);
                return View(model);
            }

            TempData["Success"] = "Employee data has been updated successfully";
           // return RedirectToAction(nameof(Index));
            return RedirectToAction("Details", new { id = model.EmployeeID });
            
        }

        // Helper method to populate dropdowns (DRY principle)
        private async Task PopulateSelectListsAsync(EmployeeFormViewModel model)
        {
            var (departments, jobTitles, users) = await GetSelectListsAsync();
            model.DepartmentList = departments;
            model.JobTitleList = jobTitles;
            model.UserList = users;
        }

        // Helper method to show Identity errors
        private void AddIdentityErrorsToModelState(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [Authorize(Roles = "Employee")]  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBasicInfo(int id, EmployeeEditBasicInfoViewModel model)
        {
            //if (id != model.EmployeeID)
            //    return BadRequest();

            var employee = await _employeeServices.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            var currentUserId = GetCurrentUserId();
            if (employee.ApplicationUserId != currentUserId)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
                return View(model);

            var updated = await _employeeServices.UpdateBasicInfoAsync(id, model);

            if (!updated)
            {
                ModelState.AddModelError("", "An error occurred during the update");
                return View(model);
            }

            TempData["Success"] = "Your data has been updated successfully";
            return RedirectToAction(nameof(MyProfile));
        }

        

        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeServices.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var model = new EmployeeDetailsViewModel
            {
                EmployeeID = employee.EmployeeID,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentName = employee.Department?.DepartmentName ?? "N/A",
                JobTitleName = employee.JobTitle?.TitleName ?? "N/A",
                HireDate = employee.HireDate,
                ApplicationUserId = employee.ApplicationUserId
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string ApplicationUserId)
        {
            var user = await _userManager.FindByIdAsync(ApplicationUserId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);



            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["Error"] = "An error occurred while deleting";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "The employee was successfully deleted";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee,HR,Admin")]
        public async Task<IActionResult> MyProfile()
        {
            var currentUserId = GetCurrentUserId();
            var employee = await _employeeServices.GetByUserIdAsync(currentUserId);

            if (employee == null)
            {
                TempData["Error"] = "Employee Data Not Found";
                return   RedirectToAction("Index", "Home");
            }

            var model = new MyProfileViewModel
            {
                EmployeeID = employee.EmployeeID,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                BasicSalary = employee.BasicSalary,
                DepartmentName = employee.Department?.DepartmentName ?? "N/A",
                JobTitleName = employee.JobTitle?.TitleName ?? "N/A",
                UserName = employee.ApplicationUser?.UserName ?? "N/A",
                TotalLeaveRequests = 0,
                ApprovedLeaves = 0,
                PendingLeaves = 0
            };

            return View(model);
=======
        // GET: api/Employee
        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ResponseViewModel<IEnumerable<EmployeeListDTO>>>> GetAll()
        {
            try
            {
                var employees = await _employeeServices.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeListDTO>>(employees);

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
                var employee = await _employeeServices.GetByIdAsync(id);

                if (employee == null)
                {
                    return NotFound(ResponseViewModel<EmployeeDetailsDTO>.NotFound("Employee not found"));
                }

                // Employee can only view their own details
                if (User.IsInRole("Employee"))
                {
                    var currentUserId = GetCurrentUserId();
                    if (employee.ApplicationUserId != currentUserId)
                    {
                        return StatusCode(403, ResponseViewModel<EmployeeDetailsDTO>.Unauthorized("Access denied"));
                    }
                }

                var dto = _mapper.Map<EmployeeDetailsDTO>(employee);
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

                var result = await _employeeServices.RegisterEmployeeAsync(model);

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
                var employee = await _employeeServices.GetByIdAsync(id);

                if (employee == null)
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

                // Map and update
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.PhoneNumber = model.PhoneNumber;
                employee.Address = model.Address;
                employee.DateOfBirth = model.DateOfBirth;
                employee.HireDate = model.HireDate;
                employee.BasicSalary = model.BasicSalary;
                employee.DepartmentID = model.DepartmentID;
                employee.JobTitleID = model.JobTitleID;

                var updateResult = await _employeeServices.UpdateAsync(employee);

                // Update ApplicationUser
                var appUser = employee.ApplicationUser;
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
                var employee = await _employeeServices.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Employee not found"));
                }

                var currentUserId = GetCurrentUserId();
                if (employee.ApplicationUserId != currentUserId)
                {
                    return StatusCode(403, ResponseViewModel<string>.Unauthorized("Access denied"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<string>.ValidationError("Invalid data"));
                }

                var updated = await _employeeServices.UpdateBasicInfoAsync(id, model);

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
                var employee = await _employeeServices.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Employee not found"));
                }

                var user = await _userManager.FindByIdAsync(employee.ApplicationUserId);
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
                var employee = await _employeeServices.GetByUserIdAsync(currentUserId);

                if (employee == null)
                {
                    return NotFound(ResponseViewModel<MyProfileDTO>.NotFound("Employee data not found"));
                }

                var dto = _mapper.Map<MyProfileDTO>(employee);
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
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }
    }
}