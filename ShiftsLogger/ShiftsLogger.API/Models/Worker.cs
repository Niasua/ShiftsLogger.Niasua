namespace ShiftsLogger.API.Models;

public class Worker
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Job { get; set; } = null!;
    public List<Shift> Shifts { get; set; } = new();
}
