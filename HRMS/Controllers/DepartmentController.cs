
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
        {
            _deptService = deptService;

            _empService = empService;

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
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}