using HRMS.ViewModels.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Authorize]
    public class PayrollController : Controller
    {
        private readonly IPayrollService _payrollService;
        private readonly IUnitOfWork _unitOfWork; // للجلب المباشر (مثل Index)

        public PayrollController(IPayrollService payrollService, IUnitOfWork unitOfWork)
        {
            _payrollService = payrollService;
        }

        // [GET] /Payroll/
        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> Index()
        {
            // جلب ملخص الرواتب مع اسم الموظف
            IEnumerable<PayslipSummaryViewModel> payslips = await _payrollService.GetAllAsync();

            return View(payslips); 
        }

        // [GET] /Payroll/Generate
        [HttpGet]
        public IActionResult Generate()
        {
            var model = new GeneratePayrollViewModel();
            return View(model);
        }

        // [POST] /Payroll/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(GeneratePayrollViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, errorMessage) = await _payrollService.GeneratePayrollAsync(model.Month, model.Year);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Payslip added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // [GET] /Payroll/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _payrollService.GetPayslipDetailsAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // [POST] /Payroll/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, HR")] 
        public async Task<IActionResult> Delete(int id)
        {
            

            await _payrollService.DeletePayslip(id);
            
            TempData["SuccessMessage"] = "payslip deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        

        [Authorize(Roles = "HR,Employee")]
        public async Task<IActionResult> MyPayslips()
        {
            var userId = GetCurrentUserId();
            var model = await _payrollService.GetMyPayslipsAsync(userId);
            return View(model);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
