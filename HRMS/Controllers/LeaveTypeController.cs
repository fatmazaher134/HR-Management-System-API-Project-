using HRMS.ViewModels.LeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{

    [Authorize]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeServices _leaveTypeServices;

        public LeaveTypeController(ILeaveTypeServices leaveTypeServices)
        {
            _leaveTypeServices = leaveTypeServices;
        }

       
        // INDEX - List all leave types
        
        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeServices.GetAllAsync();

            var model = leaveTypes.Select(lt => new LeaveTypeViewModel
            {
                LeaveTypeID = lt.LeaveTypeID,
                TypeName = lt.TypeName,
                DefaultBalance = lt.DefaultBalance
            }).ToList();

            return View(model);
        }

       
        // DETAILS - View leave type details
       
        [Authorize(Roles = "Admin,HR,Employee")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id);

            if (leaveType == null)
                return NotFound();

            var model = new LeaveTypeViewModel
            {
                LeaveTypeID = leaveType.LeaveTypeID,
                TypeName = leaveType.TypeName,
                DefaultBalance = leaveType.DefaultBalance
            };

            return View(model);
        }

       
        // CREATE - Add new leave type (GET)
       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new LeaveTypeFormViewModel());
        }

      
        // CREATE - Add new leave type (POST)
     
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var leaveType = new LeaveType
            {
                TypeName = model.TypeName,
                DefaultBalance = model.DefaultBalance
            };

            try
            {
                await _leaveTypeServices.AddAsync(leaveType);
                TempData["Success"] = "Leave type created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the leave type");
                return View(model);
            }
        }

      
        // EDIT - Edit leave type (GET)
       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id);

            if (leaveType == null)
                return NotFound();

            var model = new LeaveTypeFormViewModel
            {
                LeaveTypeID = leaveType.LeaveTypeID,
                TypeName = leaveType.TypeName,
                DefaultBalance = leaveType.DefaultBalance
            };

            return View(model);
        }

       
        // EDIT - Edit leave type (POST)
      
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeFormViewModel model)
        {
            //if (id != model.LeaveTypeID)
            //    return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var leaveType = new LeaveType
            {
                LeaveTypeID = model.LeaveTypeID,
                TypeName = model.TypeName,
                DefaultBalance = model.DefaultBalance
            };

            try
            {
                var result = await _leaveTypeServices.UpdateAsync(leaveType);

                if (result)
                {
                    TempData["Success"] = "Leave type updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update leave type");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the leave type");
                return View(model);
            }
        }

        
        // DELETE - Delete leave type (GET)
       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id);

            if (leaveType == null)
                return NotFound();

            var model = new LeaveTypeViewModel
            {
                LeaveTypeID = leaveType.LeaveTypeID,
                TypeName = leaveType.TypeName,
                DefaultBalance = leaveType.DefaultBalance
            };

            return View(model);
        }

      
        // DELETE - Delete leave type (POST)
      
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _leaveTypeServices.DeleteAsync(id);

                if (result)
                {
                    TempData["Success"] = "Leave type deleted successfully";
                }
                else
                {
                    TempData["Error"] = "Failed to delete leave type. It may be in use.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the leave type";
            }

            return RedirectToAction(nameof(Index));
        }
    }
    }
