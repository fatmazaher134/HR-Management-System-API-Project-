using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
<<<<<<< HEAD
    [Authorize(Roles = "Admin,HR,Employee")]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleServices _jobTitleService;

        public JobTitleController(IJobTitleServices jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        // GET: /JobTitle
        public async Task<IActionResult> Index()
        {
            // 1. Get models from service
            var jobTitles = await _jobTitleService.GetAllAsync();

            // 2. Map to ViewModels
            var model = jobTitles.Select(jt => new JobTitleViewModel
            {
                JobTitleID = jt.JobTitleID,
                TitleName = jt.TitleName
            });

            return View(model);
        }

        // GET: /JobTitle/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var jobTitle = await _jobTitleService.GetByIdAsync(id);
            if (jobTitle == null)
=======
    [Route("api/JobTitle")]
    [ApiController]
    //[Authorize]
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
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<IEnumerable<JobTitleViewModel>>> GetJobTitles()
        {
            var dtos = await _jobTitleService.GetAllAsync();

            var viewModels = _mapper.Map<IEnumerable<JobTitleViewModel>>(dtos);

            return Ok(viewModels);
        }

        // GET: /api/JobTitle/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<JobTitleViewModel>> GetJobTitle(int id)
        {
            var dto = await _jobTitleService.GetByIdAsync(id);
            if (dto == null)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            {
                return NotFound();
            }

<<<<<<< HEAD
            // Map to ViewModel
            var model = new JobTitleViewModel
            {
                JobTitleID = jobTitle.JobTitleID,
                TitleName = jobTitle.TitleName
            };

            return View(model);
        }

        // GET: /JobTitle/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new JobTitleFormViewModel());
        }

        // POST: /JobTitle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(JobTitleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Map ViewModel to Model
                var jobTitle = new JobTitle
                {
                    TitleName = model.TitleName
                };

                // 2. Send to service
                await _jobTitleService.AddAsync(jobTitle);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /JobTitle/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var jobTitle = await _jobTitleService.GetByIdAsync(id);
            if (jobTitle == null)
=======
            var viewModel = _mapper.Map<JobTitleViewModel>(dto);
            return Ok(viewModel);
        }

        // POST: /api/JobTitle
        [HttpPost]
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateJobTitle(int id, [FromBody] JobTitleFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<JobTitleFormDto>(viewModel);

            var success = await _jobTitleService.UpdateAsync(id, dto);

            if (!success)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            {
                return NotFound();
            }

<<<<<<< HEAD
            // Map Model to ViewModel
            var model = new JobTitleFormViewModel
            {
                JobTitleID = jobTitle.JobTitleID,
                TitleName = jobTitle.TitleName
            };

            return View(model);
        }

        // POST: /JobTitle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, JobTitleFormViewModel model)
        {
            if (id != model.JobTitleID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // 1. Map ViewModel to Model
                var jobTitle = new JobTitle
                {
                    JobTitleID = model.JobTitleID,
                    TitleName = model.TitleName
                };

                // 2. Send to service
                await _jobTitleService.UpdateAsync(jobTitle);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /JobTitle/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobTitle = await _jobTitleService.GetByIdAsync(id);
            if (jobTitle == null)
=======
            return Content("JobTitle updated successfully"); 
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobTitle(int id)
        {
            var success = await _jobTitleService.DeleteAsync(id);
            if (!success)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
            {
                return NotFound();
            }

<<<<<<< HEAD
            // Map to ViewModel for display
            var model = new JobTitleViewModel
            {
                JobTitleID = jobTitle.JobTitleID,
                TitleName = jobTitle.TitleName
            };

            return View(model);
        }

        // POST: /JobTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobTitleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
=======
            return Content("JobTitle deleted successfully");
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }
    }
}
