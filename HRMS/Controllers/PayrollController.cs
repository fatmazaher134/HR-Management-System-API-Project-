using HRMS.ViewModels.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Route("api/Payroll")]
    [ApiController]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        private readonly IMapper _mapper;

        public PayrollController(IPayrollService payrollService, IMapper mapper)
        {
            _payrollService = payrollService;
            _mapper = mapper;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/Payroll
        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        public async Task<ActionResult<IEnumerable<PayslipSummaryViewModel>>> Index()
        {
            var dtos = await _payrollService.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<PayslipSummaryViewModel>>(dtos);
            return Ok(viewModels);
        }

        // GET: api/Payroll/MyPayslips
        [HttpGet("MyPayslips")]
        [Authorize(Roles = "HR,Employee")]
        public async Task<ActionResult<IEnumerable<PayslipViewModel>>> MyPayslips()
        {
            var userId = GetCurrentUserId();
            var dtos = await _payrollService.GetMyPayslipsAsync(userId);
            var viewModels = _mapper.Map<IEnumerable<PayslipViewModel>>(dtos);
            return Ok(viewModels);
        }

        // GET: api/Payroll/Details/5 (للـ Admin/HR)
        [HttpGet("Details/{id}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<ActionResult<PayslipDetailsViewModel>> Details(int id)
        {
            var dto = await _payrollService.GetPayslipDetailsAsync(id);
            if (dto == null) return NotFound();

            var viewModel = _mapper.Map<PayslipDetailsViewModel>(dto);
            return Ok(viewModel);
        }

        // GET: api/Payroll/MyDetails/5 (للـ Employee)
        [HttpGet("MyDetails/{id}")]
        [Authorize(Roles = "HR,Employee")]
        public async Task<ActionResult<PayslipDetailsViewModel>> MyDetails(int id)
        {
            var userId = GetCurrentUserId();
            var dto = await _payrollService.GetMyPayslipDetailsAsync(id, userId);
            if (dto == null) return NotFound(); // Not found or not authorized

            var viewModel = _mapper.Map<PayslipDetailsViewModel>(dto);
            return Ok(viewModel);
        }

        // POST: api/Payroll/Generate
        [HttpPost("Generate")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> Generate([FromBody] GeneratePayrollViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<GeneratePayrollDto>(viewModel);

            var (success, errorMessage) = await _payrollService.GeneratePayrollAsync(dto);

            if (!success)
            {
                return BadRequest(new { message = errorMessage });
            }

            return Ok(new { message = "Payroll generated successfully." });
        }

        // DELETE: api/Payroll/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _payrollService.DeletePayslip(id);
                return Content("Payslip deleted successfully"); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DbUpdateException ex) 
            {
                return BadRequest(new { message = "Cannot delete this payslip. (DbUpdateException)" });
            }
        }
        
    }
}
