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
        {
            _deptService = deptService;

            _empService = empService;
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
            {
                return NotFound();
            }

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

            if (!success)
            {
                return NotFound();
            }

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
        }
    }
}