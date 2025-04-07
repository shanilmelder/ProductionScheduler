using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Models;
using ProductionScheduler.Services.Repositories;

namespace ProductionScheduler.Tests.Repositories
{
    public class RecurringHolidayRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public RecurringHolidayRepositoryTests()
        {
            // Configure an in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ProductionSchedulerTestDb")
                .Options;
        }

        [Fact]
        public async Task AddAsync_AddsNewRecurringHoliday()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new RecurringHolidayRepository(dbContext);
            var recurringHoliday = new RecurringHoliday
            {
                Month = 12,
                Date = 25
            };

            // Act
            await repository.AddAsync(recurringHoliday);
            await dbContext.SaveChangesAsync();

            // Assert
            var result = dbContext.RecurringHolidays.Find(recurringHoliday.Id);
            Assert.NotNull(result);
            Assert.Equal(recurringHoliday.Month, result.Month);
            Assert.Equal(recurringHoliday.Date, result.Date);
        }

        [Fact]
        public async Task Delete_DeletesRecurringHolidayById()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new RecurringHolidayRepository(dbContext);
            var recurringHoliday = new RecurringHoliday { Month = 12, Date = 25 };

            dbContext.Set<RecurringHoliday>().Add(recurringHoliday);
            await dbContext.SaveChangesAsync();

            // Act
            await repository.Delete(recurringHoliday.Id);
            await dbContext.SaveChangesAsync();

            // Assert
            var deletedHoliday = await dbContext.Set<RecurringHoliday>().FindAsync(recurringHoliday.Id);
            Assert.Null(deletedHoliday);
        }

        [Fact]
        public void Update_UpdatesRecurringHoliday()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new RecurringHolidayRepository(dbContext);
            var recurringHoliday = new RecurringHoliday { Month = 12, Date = 25 };
            dbContext.Set<RecurringHoliday>().Add(recurringHoliday);
            dbContext.SaveChanges();

            recurringHoliday.Month = 11;

            // Act
            repository.Update(recurringHoliday);
            dbContext.SaveChanges();

            // Assert
            var updatedHoliday = dbContext.Set<RecurringHoliday>().Find(recurringHoliday.Id);
            Assert.Equal(recurringHoliday.Month, updatedHoliday.Month);
        }

        [Fact]
        public async Task GetAsync_ReturnsAllRecurringHolidays()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new RecurringHolidayRepository(dbContext);
            var recurringHoliday = new RecurringHoliday { Month = 12, Date = 25 };
            dbContext.Set<RecurringHoliday>().Add(recurringHoliday);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
