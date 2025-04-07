namespace ProductionScheduler.Services.Repositories;

public interface IUnitOfWork : IDisposable
{
    IOneTimeHolidayRepository OneTimeHolidayRepository { get; }
    Task SaveChangesAsync();
}
