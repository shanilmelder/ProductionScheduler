namespace ProductionScheduler.Services.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public  IOneTimeHolidayRepository OneTimeHolidayRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext,
                            IOneTimeHolidayRepository oneTimeHolidayRepository)
    {
        _dbContext = dbContext;
        OneTimeHolidayRepository = oneTimeHolidayRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }
}
