using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public interface IWorkerService
{
    Task<List<Worker>> GetAllAsync();
    Task<Worker?> GetByIdAsync(int id);
    Task<Worker> AddAsync(Worker worker);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(Worker worker);
}
