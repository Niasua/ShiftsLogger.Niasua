using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {

        private readonly IWorkerService _service;

        public WorkersController(IWorkerService service)
        {
            _service = service;
        }

        // GET: api/Workers
        [HttpGet]
        public async Task<ActionResult<List<Worker>>> GetAll()
        {
            var workers = await _service.GetAllAsync();
            return Ok(workers);
        }

        // GET: api/Workers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> GetById(int id)
        {
            var worker = await _service.GetByIdAsync(id);
            if (worker == null) return NotFound();
            return Ok(worker);
        }

        // POST: api/Workers
        [HttpPost]
        public async Task<ActionResult<Worker>> Create(Worker worker)
        {
            var created = await _service.AddAsync(worker);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Workers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Worker worker)
        {
            if (id != worker.Id) return BadRequest();

            var updated = await _service.UpdateAsync(worker);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/Workers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
