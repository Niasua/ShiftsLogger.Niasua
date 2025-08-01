using ShiftsLogger.ConsoleUI.Models;
using System.Net.Http.Json;

namespace ShiftsLogger.ConsoleUI;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService()
    {
        _client = new();
        _client.BaseAddress = new Uri("http://localhost:5266/api/");
    }

    //Workers
    public async Task<List<Worker>?> GetAllWorkersAsync()
    {
        try
        {
            return await _client.GetFromJsonAsync<List<Worker>>("workers");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while obtaining workers: {ex.Message}");
            return null;
        }
    }
    public async Task<Worker?> GetWorkerByIdAsync(int id)
    {
        try
        {
            return await _client.GetFromJsonAsync<Worker?>($"workers/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting worker by ID: {ex.Message}");
            return null;
        }
    }
    public async Task<Worker?> CreateWorkerAsync(Worker worker)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("workers", worker);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Worker>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating worker: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> UpdateWorkerAsync(int id, Worker worker)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"workers/{id}", worker);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating worker: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> DeleteWorkerAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"workers/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting worker: {ex.Message}");
            return false;
        }
    }

    // Shifts
    public async Task<List<Shift>?> GetAllShiftsAsync()
    {
        try
        {
            return await _client.GetFromJsonAsync<List<Shift>>("shifts");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while obtaining shifts: {ex.Message}");
            return null;
        }
    }
    public async Task<List<Shift>?> GetShiftsByWorkerIdAsync(int workerId)
    {
        try
        {
            return await _client.GetFromJsonAsync<List<Shift>>($"shifts/worker/{workerId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting shifts for worker {workerId}: {ex.Message}");
            return null;
        }
    }
    public async Task<Shift?> GetShiftByIdAsync(int id)
    {
        try
        {
            return await _client.GetFromJsonAsync<Shift?>($"shifts/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while obtaining shift: {ex.Message}");
            return null;
        }
    }
    public async Task<Shift?> CreateShiftAsync(Shift shift)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("shifts", shift);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Shift>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating shift: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> UpdateShiftAsync(int id, Shift shift)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"shifts/{id}", shift);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating shift: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> DeleteShiftAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"shifts/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting shift: {ex.Message}");
            return false;
        }
    }
}
