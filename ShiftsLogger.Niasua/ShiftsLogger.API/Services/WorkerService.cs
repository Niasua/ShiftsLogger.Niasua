using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public class WorkerService : IWorkerService
{
    private readonly ShiftsLoggerContext _context;

    public WorkerService(ShiftsLoggerContext context)
    {
        _context = context;
    }

    public async Task<List<Worker>> GetAllAsync()
    {
        return await _context.Workers
            .Include(w => w.Shifts)
            .ToListAsync();
    }

    public async Task<Worker?> GetByIdAsync(int id)
    {
        return await _context.Workers
            .Include(w => w.Shifts)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Worker> AddAsync(Worker worker)
    {
        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();
        return worker;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker == null) return false;

        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(Worker worker)
    {
        if (!await _context.Workers.AnyAsync(w => w.Id == worker.Id))
        {
            return false;
        }

        _context.Workers.Update(worker);
        await _context.SaveChangesAsync();
        return true;
    }
}
