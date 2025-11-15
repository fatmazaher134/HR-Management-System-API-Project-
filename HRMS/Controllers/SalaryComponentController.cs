<<<<<<< HEAD
﻿using HRMS.ViewModels.SalaryComponent;
using Microsoft.AspNetCore.Authorization;
=======
﻿using HRMS.DTOs.SalaryComponent;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
<<<<<<< HEAD
        [Authorize(Roles = "Admin,HR")]
    public class SalaryComponentsController : Controller
        {
            private readonly ISalaryComponentServices _service;

            public SalaryComponentsController(ISalaryComponentServices service)
            {
                _service = service;
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var model = await _service.GetAllAsync();
                return View(model);
            }

            [HttpGet]
            [Authorize(Roles = "Admin")]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateSalaryComponentViewModel model)
            {
                if (!ModelState.IsValid) return View(model);

                await _service.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }

            [HttpGet]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Edit(int id)
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null) return NotFound();

                var model = new EditSalaryComponentViewModel
                {
                    ComponentID = existing.ComponentID,
                    ComponentName = existing.ComponentName,
                    ComponentType = existing.ComponentType
                };

                return View(model);
            }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(EditSalaryComponentViewModel model)
            {
                if (!ModelState.IsValid) return View(model);

                var result = await _service.UpdateAsync(model.ComponentID, model); 
                if (!result) return NotFound();

                return RedirectToAction(nameof(Index));
            }
    

                [HttpPost]
                [ValidateAntiForgeryToken]
                [Authorize(Roles = "Admin")]
                public async Task<IActionResult> Delete(int id)
                {
                    var model = await _service.GetByIdAsync(id);
                    if (model == null)
                        return NotFound();

                    await _service.DeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                }


             [HttpGet]
            [Authorize(Roles = "Admin,HR")]
            public async Task<IActionResult> Details(int id)
            {
                var model = await _service.GetByIdAsync(id);
                if (model == null) return NotFound();
                return View(model);
            }
        }
    }
=======
    [ApiController]
    [Route("api/SalaryComponents")] 
    public class SalaryComponentsController : ControllerBase 
    {
        private readonly ISalaryComponentServices _service;

        public SalaryComponentsController(ISalaryComponentServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryComponentDto>>> GetAll()
        {
            var model = await _service.GetAllAsync();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryComponentDto>> GetById(int id)
        {
            var model = await _service.GetByIdAsync(id);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Create([FromBody] CreateSalaryComponentDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDto = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.ComponentID }, createdDto);
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryComponentDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, model);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _service.GetByIdAsync(id);
            if (model == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent(); 
        }
    }
}
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e

