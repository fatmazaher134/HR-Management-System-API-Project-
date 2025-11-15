using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
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
            {
                return NotFound();
            }

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
            {
                return NotFound();
            }

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
            {
                return NotFound();
            }

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
        }
    }
}
