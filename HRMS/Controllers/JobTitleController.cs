using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/JobTitle")]
    [ApiController]
    [Authorize]
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleServices _jobTitleService;
        private readonly IMapper _mapper;

        public JobTitleController(IJobTitleServices jobTitleService, IMapper mapper)
        {
            _jobTitleService = jobTitleService;
            _mapper = mapper;
        }

        // GET: /api/JobTitle
        [HttpGet]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<IEnumerable<JobTitleViewModel>>> GetJobTitles()
        {
            var dtos = await _jobTitleService.GetAllAsync();

            var viewModels = _mapper.Map<IEnumerable<JobTitleViewModel>>(dtos);

            return Ok(viewModels);
        }

        // GET: /api/JobTitle/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<JobTitleViewModel>> GetJobTitle(int id)
        {
            var dto = await _jobTitleService.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<JobTitleViewModel>(dto);
            return Ok(viewModel);
        }

        // POST: /api/JobTitle
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<JobTitleViewModel>> CreateJobTitle([FromBody] JobTitleFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<JobTitleFormDto>(viewModel);

            var createdDto = await _jobTitleService.AddAsync(dto);

            var createdViewModel = _mapper.Map<JobTitleViewModel>(createdDto);

            return CreatedAtAction(nameof(GetJobTitle), new { id = createdViewModel.JobTitleID }, createdViewModel);
        }

        // PUT: /api/JobTitle/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateJobTitle(int id, [FromBody] JobTitleFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<JobTitleFormDto>(viewModel);

            var success = await _jobTitleService.UpdateAsync(id, dto);

            if (!success)
            {
                return NotFound();
            }

            return Content("JobTitle updated successfully"); 
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobTitle(int id)
        {
            var success = await _jobTitleService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return Content("JobTitle deleted successfully");
        }
    }
}
