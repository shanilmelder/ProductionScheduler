using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Models;
using ProductionScheduler.Services.Repositories;

namespace ProductionScheduler.Tests.Repositories
{
    public class OneTimeHolidayRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public OneTimeHolidayRepositoryTests()
        {
            // Configure an in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ProductionSchedulerTestDb")
                .Options;
        }

        [Fact]
        public async Task AddAsync_AddsNewOneTimeHoliday()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new OneTimeHolidayRepository(dbContext);
            var oneTimeHoliday = new OneTimeHoliday
            {
                DateTime = new DateTime(2025, 12, 25, 10, 0, 0)
            };

            // Act
            await repository.AddAsync(oneTimeHoliday);
            await dbContext.SaveChangesAsync();

            // Assert
            var result = dbContext.OneTimeHolidays.Find(oneTimeHoliday.Id);
            Assert.NotNull(result);
            Assert.Equal(oneTimeHoliday.DateTime, result.DateTime);
        }

        [Fact]
        public async Task Delete_DeletesOneTimeHolidayById()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new OneTimeHolidayRepository(dbContext);
            var oneTimeHoliday = new OneTimeHoliday { DateTime = DateTime.Now };

            dbContext.Set<OneTimeHoliday>().Add(oneTimeHoliday);
            await dbContext.SaveChangesAsync();

            // Act
            await repository.Delete(oneTimeHoliday.Id);
            await dbContext.SaveChangesAsync();

            // Assert
            var deletedHoliday = await dbContext.Set<OneTimeHoliday>().FindAsync(oneTimeHoliday.Id);
            Assert.Null(deletedHoliday);
        }

        [Fact]
        public void Update_UpdatesOneTimeHoliday()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new OneTimeHolidayRepository(dbContext);
            var oneTimeHoliday = new OneTimeHoliday { DateTime = DateTime.Now };
            dbContext.Set<OneTimeHoliday>().Add(oneTimeHoliday);
            dbContext.SaveChanges();

            oneTimeHoliday.DateTime = DateTime.Now.AddDays(1);

            // Act
            repository.Update(oneTimeHoliday);
            dbContext.SaveChanges();

            // Assert
            var updatedHoliday = dbContext.Set<OneTimeHoliday>().Find(oneTimeHoliday.Id);
            Assert.Equal(oneTimeHoliday.DateTime, updatedHoliday.DateTime);
        }

        [Fact]
        public async Task GetAsync_ReturnsAllOneTimeHolidays()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_dbContextOptions);
            var repository = new OneTimeHolidayRepository(dbContext);

            dbContext.Set<OneTimeHoliday>().RemoveRange(dbContext.Set<OneTimeHoliday>());
            await dbContext.SaveChangesAsync();

            var oneTimeHoliday = new OneTimeHoliday { DateTime = DateTime.Now };
            dbContext.Set<OneTimeHoliday>().Add(oneTimeHoliday);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(oneTimeHoliday.DateTime, result.First().DateTime);
        }


    }
}
