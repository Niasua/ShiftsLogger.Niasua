using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Data;

public class ShiftsLoggerContext : DbContext
{
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    public ShiftsLoggerContext(DbContextOptions<ShiftsLoggerContext> options) : base(options) { }
}
