using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProductionScheduler.Services.Interfaces;
using ProductionScheduler.Services.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDatabaseServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("ProductionSchedulerDb");
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });
    }

    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient<IOneTimeHolidayRepository, OneTimeHolidayRepository>();
        builder.Services.AddTransient<IRecurringHolidayRepository, RecurringHolidayRepository>();
        builder.Services.AddTransient<IWorkDayRepository, WorkDayRepository>();
        builder.Services.AddSingleton(TimeProvider.System);
    }
}
