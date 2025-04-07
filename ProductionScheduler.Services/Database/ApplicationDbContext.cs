using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Models;

namespace ProductionScheduler.Services.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<OneTimeHoliday> OneTimeHolidays { set; get; }
    public DbSet<RecurringHoliday> RecurringHolidays { set; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
