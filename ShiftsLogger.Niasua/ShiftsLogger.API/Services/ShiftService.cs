using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public class ShiftService : IShiftService
{
    private readonly ShiftsLoggerContext _context;

    public ShiftService(ShiftsLoggerContext context)
    {
        _context = context;
    }


    public async Task<Shift> AddAsync(Shift shift)
    {
        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift == null) return false;

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Shift>> GetAllAsync()
    {
        return await _context.Shifts.ToListAsync();
    }

    public async Task<Shift?> GetByIdAsync(int id)
    {
        return await _context.Shifts.FindAsync(id);
    }

    public async Task<List<Shift>> GetByWorkerIdAsync(int workerId)
    {
        return await _context.Shifts
            .Where(s => s.WorkerId == workerId)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Shift shift)
    {
        var existing = await _context.Shifts.FindAsync(shift.Id);
        if (existing == null) return false;

        existing.Start = shift.Start;
        existing.End = shift.End;
        existing.Type = shift.Type;
        existing.WorkerId = shift.WorkerId;

        await _context.SaveChangesAsync();
        return true;
    }
}
