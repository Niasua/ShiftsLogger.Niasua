using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _service;

    public ShiftsController(IShiftService service)
    {
        _service = service;
    }

    // GET: api/Shifts
    [HttpGet]
    public async Task<ActionResult<List<Shift>>> GetAll()
    {
        var shifts = await _service.GetAllAsync();
        return Ok(shifts);
    }

    // GET: api/Shifts/worker/5
    [HttpGet("worker/{workerId}")]
    public async Task<ActionResult<List<Shift>>> GetByWorkerId(int workerId)
    {
        var shifts = await _service.GetByWorkerIdAsync(workerId);
        return Ok(shifts);
    }

    // GET: api/Shifts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> GetById(int id)
    {
        var shift = await _service.GetByIdAsync(id);
        if (shift == null) return NotFound();
        return Ok(shift);
    }

    // POST: api/Shifts
    [HttpPost]
    public async Task<ActionResult<Shift>> Create(Shift shift)
    {
        var created = await _service.AddAsync(shift);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE: api/Shifts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }

    // PUT: api/Shifts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Shift shift)
    {
        if (id != shift.Id) return BadRequest();

        var updated = await _service.UpdateAsync(shift);
        if (!updated) return NotFound();

        return NoContent();
    }

}
