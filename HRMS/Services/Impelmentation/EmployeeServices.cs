
using AutoMapper;
using HRMS.DTOs.Employee;
using HRMS.Interfaces;
using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Services.Impelmentation
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepository _empRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EmployeeServices(
            IEmployeeRepository empRepo,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _empRepo = empRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeListDTO>> GetAllAsync()
        {
            var includes = new string[] { "Department", "JobTitle", "ApplicationUser" };
            var employees = await _empRepo.FindAllAsync(
                criteria: e => e.IsActive,
                includes: includes
            );

            // Map Model to DTO
            return _mapper.Map<IEnumerable<EmployeeListDTO>>(employees);
        }

        public async Task<EmployeeDetailsDTO?> GetByIdAsync(int id)
        {
            var includes = new string[] { "Department", "JobTitle", "ApplicationUser" };
            var employee = await _empRepo.FindAsync(
                criteria: e => e.EmployeeID == id && e.IsActive,
                includes: includes
            );

            if (employee == null)
                return null;

            // Map Model to DTO
            return _mapper.Map<EmployeeDetailsDTO>(employee);
        }

        public async Task<IdentityResult> RegisterEmployeeAsync(CreateEmployeeDTO dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // Create Application User
                    var appUser = new ApplicationUser
                    {
                        UserName = dto.Email,
                        Email = dto.Email,
                        FullName = $"{dto.FirstName} {dto.LastName}",
                        Address = dto.Address,
                        PhoneNumber = dto.PhoneNumber
                    };

                    var result = await _userManager.CreateAsync(appUser, dto.Password);

                    if (result.Succeeded)
                    {
                        // Add to Employee role
                        await _userManager.AddToRoleAsync(appUser, "Employee");

                        // Map DTO to Model
                        var employee = _mapper.Map<Employee>(dto);
                        employee.ApplicationUserId = appUser.Id;
                        employee.HireDate = DateTime.UtcNow;
                        employee.IsActive = true;

                        await _empRepo.AddAsync(employee);
                        await _unitOfWork.SaveChangesAsync();
                        _unitOfWork.Commit();
                    }

                    return result;
                }
                catch
                {
                    _unitOfWork.Rollback();
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "An error occurred while registering the employee."
                    });
                }
            }
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeDTO dto)
        {
            try
            {
                var existing = await _empRepo.GetByIdAsync(dto.EmployeeID);
                if (existing == null || !existing.IsActive)
                    return false;

                // Map DTO to existing Model
                _mapper.Map(dto, existing);

                await _empRepo.UpdateEmployeeAsync(existing);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBasicInfoAsync(int employeeId, UpdateEmployeeBasicInfoDTO dto)
        {
            try
            {
                var employee = await _empRepo.GetByIdAsync(employeeId);
                if (employee == null || !employee.IsActive)
                    return false;

                // Map DTO to Model (only allowed fields)
                employee.FirstName = dto.FirstName;
                employee.LastName = dto.LastName;
                employee.PhoneNumber = dto.PhoneNumber;
                employee.Address = dto.Address;

                await _empRepo.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _empRepo.SoftDeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<MyProfileDTO?> GetByUserIdAsync(string userId)
        {
            var includes = new string[] { "Department", "JobTitle", "ApplicationUser" };
            var employee = await _empRepo.FindAsync(
                criteria: e => e.ApplicationUserId == userId && e.IsActive,
                includes: includes
            );

            if (employee == null)
                return null;

            // Map Model to DTO
            return _mapper.Map<MyProfileDTO>(employee);
        }

        public async Task<IEnumerable<EmployeeListDTO>> GetByDepartmentIdAsync(int departmentId)
        {
            var employees = await _empRepo.GetEmployeesByDepartmentAsync(departmentId);

            // Map Model to DTO
            return _mapper.Map<IEnumerable<EmployeeListDTO>>(employees);
        }

        public async Task<decimal> GetTotalSalaryAsync(int departmentId)
        {
            var employees = await _empRepo.FindAllAsync(
                criteria: e => e.DepartmentID == departmentId && e.IsActive,
                includes: null
            );

            return employees.Any() ? employees.Sum(e => e.BasicSalary) : 0;
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeEmployeeId = null)
        {
            if (excludeEmployeeId.HasValue)
            {
                return await _empRepo.IsExistAsync(
                    e => e.Email == email && e.EmployeeID != excludeEmployeeId.Value
                );
            }

            return await _empRepo.IsExistAsync(e => e.Email == email);
        }
    }
 }


