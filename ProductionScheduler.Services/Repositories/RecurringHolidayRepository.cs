using ProductionScheduler.Models;

namespace ProductionScheduler.Services.Repositories;

public class RecurringHolidayRepository : GenericRepository<RecurringHoliday>, IRecurringHolidayRepository
{
    public RecurringHolidayRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}