<<<<<<< HEAD

﻿using HRMS.Interfaces.Services; // <-- التأكد من وجودها

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
// قد تحتاج لإضافة using لنطاق الأسماء (namespace) الخاص بـ DepartmentViewModel و DepartmentFormViewModel إذا لم يكن موجوداً

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin,HR,Employee")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _deptService;
        private readonly IEmployeeServices _empService;

        public DepartmentController(IDepartmentServices deptService, IEmployeeServices empService)
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    //[Authorize(Roles = "Admin,HR,Employee")]
    [Route("api/Department")] 
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentServices _deptService;
        private readonly IEmployeeServices _empService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentServices deptService,
            IEmployeeServices empService,
            IMapper mapper)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        {
            _deptService = deptService;

            _empService = empService;
<<<<<<< HEAD

        }

        private async Task<IEnumerable<SelectListItem>> GetEmployeesSelectListAsync()
        {
            var employees = await _empService.GetAllAsync();

            return employees.Select(e => new SelectListItem
            {
                Value = e.EmployeeID.ToString(),
                Text = e.FirstName + " " + e.LastName
            });
        }

        // GET: /Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _deptService.GetAllAsync();

            var model = departments.Select(d => new DepartmentViewModel
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
                ManagerName = d.Manager != null ? d.Manager.FirstName + " " + d.Manager.LastName : "N/A"
            });

            return View(model);
        }

        // GET: /Departments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var department = await _deptService.GetByIdAsync(id);
            if (department == null)
=======
            _mapper = mapper;
        }

        // GET: /api/department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetDepartments()
        {
            var dtos = await _deptService.GetAllAsync();

            var viewModels = _mapper.Map<IEnumerable<DepartmentViewModel>>(dtos);

            return Ok(viewModels);
        }

        // GET: /api/department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentViewModel>> GetDepartment(int id)
        {
            var dto = await _deptService.GetByIdAsync(id);
            if (dto == null)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            {
                return NotFound();
            }

<<<<<<< HEAD
            var model = new DepartmentViewModel
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName,
                ManagerName = department.Manager != null ?
                department.Manager.FirstName + " " + department.Manager.LastName : "N/A"
            };

            return View(model);
        }

        // GET: /Departments/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new DepartmentFormViewModel
            {
                ManagerList = await GetEmployeesSelectListAsync()
            };
            return View(model);
        }

        // POST: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DepartmentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    DepartmentName = model.DepartmentName,
                    ManagerID = model.ManagerID
                };

                await _deptService.AddAsync(department);
                return RedirectToAction(nameof(Index));
            }

            model.ManagerList = await GetEmployeesSelectListAsync();
            return View(model);
        }

        // GET: /Departments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _deptService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new DepartmentFormViewModel
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName,
                ManagerID = department.ManagerID,
                ManagerList = await GetEmployeesSelectListAsync() // dropdown
            };

            return View(model);
        }

        // POST: /Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DepartmentFormViewModel model)
        {
            if (id != model.DepartmentID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    DepartmentID = model.DepartmentID,
                    DepartmentName = model.DepartmentName,
                    ManagerID = model.ManagerID
                };

                await _deptService.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }

            model.ManagerList = await GetEmployeesSelectListAsync();
            return View(model);
        }

        // GET: /Departments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _deptService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new DepartmentViewModel
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName,
                ManagerName = department.Manager != null ?
                department.Manager.FirstName + " " + department.Manager.LastName : "N/A"
            };

            return View(model);
        }

        // POST: /Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _deptService.DeleteAsync(id);
=======
            var viewModel = _mapper.Map<DepartmentViewModel>(dto);
            return Ok(viewModel);
        }

        // POST: /api/department
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<DepartmentViewModel>> CreateDepartment([FromBody] DepartmentFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<DepartmentFormDto>(viewModel);

            var createdDto = await _deptService.AddAsync(dto);

            var createdViewModel = _mapper.Map<DepartmentViewModel>(createdDto);

            return CreatedAtAction(nameof(GetDepartment), new { id = createdViewModel.DepartmentID }, createdViewModel);
        }

        // PUT: /api/department/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<DepartmentFormDto>(viewModel);

            var success = await _deptService.UpdateAsync(id, dto);

>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            if (!success)
            {
                return NotFound();
            }
<<<<<<< HEAD
            return RedirectToAction(nameof(Index));
=======

            return Content("Department updated successfully");
        }

        // DELETE: /api/department/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var success = await _deptService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return Content("Department deleted successfully");
        }


        [HttpGet("Employee/{EmpId}")]
        public async Task<ActionResult<DepartmentViewModel>> GetDepartmentByEmployeeId(int EmpId)
        {
            var dto = await _deptService.GetByEmpIdAsync(EmpId);
            if (dto == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DepartmentViewModel>(dto);
            return Ok(viewModel);
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }
    }
}