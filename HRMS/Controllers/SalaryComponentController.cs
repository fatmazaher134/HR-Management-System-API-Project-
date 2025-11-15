using HRMS.DTOs.SalaryComponent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
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
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSalaryComponentDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDto = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.ComponentID }, createdDto);
        }

        [HttpPut("{id}")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryComponentDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, model);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
         [Authorize(Roles = "Admin")]
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

