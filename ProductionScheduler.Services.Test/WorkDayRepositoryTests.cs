using Moq;
using ProductionScheduler.Models;
using ProductionScheduler.Models.Entities;
using ProductionScheduler.Services.Repositories;

namespace ProductionScheduler.Services.Test;

public class WorkDayRepositoryTests
{
    private readonly Mock<IOneTimeHolidayRepository> _oneTimeHolidayRepoMock;
    private readonly Mock<IRecurringHolidayRepository> _recurringHolidayRepoMock;
    private readonly WorkDayRepository _repository;

    public WorkDayRepositoryTests()
    {
        _oneTimeHolidayRepoMock = new Mock<IOneTimeHolidayRepository>();
        _recurringHolidayRepoMock = new Mock<IRecurringHolidayRepository>();
        _repository = new WorkDayRepository(_oneTimeHolidayRepoMock.Object, _recurringHolidayRepoMock.Object);
    }

    [Fact]
    public async Task AddWorkingTime_BasicQuarterDay_ShouldReturnNextDay0907()
    {
        // Arrange
        var workDay = new WorkDay
        {
            StartDateTime = new DateTime(2004, 5, 24, 15, 7, 0),
            WorkingDaysToAdd = 0.25,
            WorkdayStart = new TimeSpan(8, 0, 0),
            WorkdayEnd = new TimeSpan(16, 0, 0)
        };

        _oneTimeHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<OneTimeHoliday>());
        _recurringHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<RecurringHoliday>());

        // Act
        var result = await _repository.AddWorkingTime(workDay);

        // Assert
        Assert.Equal("25-05-2004 09:07", result);
    }

    [Fact]
    public async Task AddWorkingTime_BeforeWorkHours_ShouldReturnSameDayNoon()
    {
        // Arrange
        var workDay = new WorkDay
        {
            StartDateTime = new DateTime(2004, 5, 24, 4, 0, 0),
            WorkingDaysToAdd = 0.5,
            WorkdayStart = new TimeSpan(8, 0, 0),
            WorkdayEnd = new TimeSpan(16, 0, 0)
        };

        _oneTimeHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<OneTimeHoliday>());
        _recurringHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<RecurringHoliday>());

        // Act
        var result = await _repository.AddWorkingTime(workDay);

        // Assert
        Assert.Equal("24-05-2004 12:00", result);
    }

    [Fact]
    public async Task SubtractWorkingTime_WithHolidays_ShouldReturnExpectedDate()
    {
        // Arrange
        var workDay = new WorkDay
        {
            StartDateTime = new DateTime(2004, 5, 24, 18, 5, 0),
            WorkingDaysToAdd = -5.5,
            WorkdayStart = new TimeSpan(8, 0, 0),
            WorkdayEnd = new TimeSpan(16, 0, 0)
        };

        // One-time holiday on May 27, 2004
        _oneTimeHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<OneTimeHoliday>
        {
            new OneTimeHoliday { DateTime = new DateTime(2004, 5, 27) }
        });

        // Recurring holiday on May 17
        _recurringHolidayRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(new List<RecurringHoliday>
        {
            new RecurringHoliday { Month = 5, Date = 17 }
        });

        // Act
        var result = await _repository.SubtractWorkingTime(workDay);

        // Assert
        Assert.Equal("14-05-2004 12:00", result);
    }
}
