using ProductionScheduler.Models;

namespace ProductionScheduler.Services.Repositories;

public class OneTimeHolidayRepository : GenericRepository<OneTimeHoliday>, IOneTimeHolidayRepository
{
    public OneTimeHolidayRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
