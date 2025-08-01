using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public interface IShiftService
{
    Task<List<Shift>> GetAllAsync();
    Task<List<Shift>> GetByWorkerIdAsync(int workerId);
    Task<Shift?> GetByIdAsync(int id);
    Task<Shift> AddAsync(Shift shift);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(Shift shift);
}
