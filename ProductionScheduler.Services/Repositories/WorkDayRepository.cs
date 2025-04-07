using ProductionScheduler.Models.Entities;
using ProductionScheduler.Services.Interfaces;

namespace ProductionScheduler.Services.Repositories;

public class WorkDayRepository : IWorkDayRepository
{
    private readonly IOneTimeHolidayRepository _oneTimeHolidayRepository;
    private readonly IRecurringHolidayRepository _recurringHolidayRepository;
    public WorkDayRepository(IOneTimeHolidayRepository oneTimeHolidayRepository, IRecurringHolidayRepository recurringHolidayRepository)
    {
        _oneTimeHolidayRepository = oneTimeHolidayRepository;
        _recurringHolidayRepository = recurringHolidayRepository;
    }

    public async Task<string> AddWorkingTime(WorkDay workDay)
    {
        // Get holidays
        var oneTimeHolidays = (await _oneTimeHolidayRepository.GetAsync())
            .Select(h => h.DateTime.Date)
            .ToHashSet();

        var recurringHolidays = (await _recurringHolidayRepository.GetAsync())
            .Select(h => (h.Month, h.Date))
            .ToHashSet();

        // Convert working days to total hours, then to TimeSpan
        var workingHoursPerDay = (workDay.WorkdayEnd - workDay.WorkdayStart).TotalHours;
        var totalWorkingHoursToAdd = Math.Abs(workDay.WorkingDaysToAdd) * workingHoursPerDay;
        var timeToAdd = TimeSpan.FromHours(totalWorkingHoursToAdd);

        var current = workDay.StartDateTime;

        while (timeToAdd > TimeSpan.Zero)
        {
            // If before working hours, move to work start
            if (current.TimeOfDay < workDay.WorkdayStart)
            {
                current = current.Date + workDay.WorkdayStart;
            }

            // If after working hours, skip to next day
            if (current.TimeOfDay >= workDay.WorkdayEnd)
            {
                current = current.Date.AddDays(1) + workDay.WorkdayStart;
                continue;
            }

            // Skip non-working days
            if (!IsWorkday(current.Date, oneTimeHolidays, recurringHolidays))
            {
                current = current.Date.AddDays(1) + workDay.WorkdayStart;
                continue;
            }

            var workEnd = current.Date + workDay.WorkdayEnd;
            var availableToday = workEnd - current;

            if (timeToAdd <= availableToday)
            {
                current += timeToAdd;
                break;
            }
            else
            {
                timeToAdd -= availableToday;
                current = current.Date.AddDays(1) + workDay.WorkdayStart;
            }
        }

        return current.ToString("dd-MM-yyyy HH:mm");
    }


    public async Task<string> SubtractWorkingTime(WorkDay workDay)
    {
        // Get holidays
        var oneTimeHolidays = (await _oneTimeHolidayRepository.GetAsync())
            .Select(h => h.DateTime.Date)
            .ToHashSet();

        var recurringHolidays = (await _recurringHolidayRepository.GetAsync())
            .Select(h => (h.Month, h.Date))
            .ToHashSet();

        // Convert working days to total hours, then to TimeSpan
        var workingHoursPerDay = (workDay.WorkdayEnd - workDay.WorkdayStart).TotalHours;
        var totalWorkingHoursToSubtract = Math.Abs(workDay.WorkingDaysToAdd) * workingHoursPerDay;
        var timeToSubtract = TimeSpan.FromHours(totalWorkingHoursToSubtract);

        var current = workDay.StartDateTime;

        // Loop while there's working time to subtract
        while (timeToSubtract > TimeSpan.Zero)
        {
            // If after work hours, snap to work end
            if (current.TimeOfDay > workDay.WorkdayEnd)
            {
                current = current.Date + workDay.WorkdayEnd;
            }

            // If before work hours, skip to previous day
            if (current.TimeOfDay < workDay.WorkdayStart)
            {
                current = current.Date.AddDays(-1) + workDay.WorkdayEnd;
            }

            // Skip weekends and holidays
            if (!IsWorkday(current.Date, oneTimeHolidays, recurringHolidays))
            {
                current = current.Date.AddDays(-1) + workDay.WorkdayEnd;
                continue;
            }

            var workStart = current.Date + workDay.WorkdayStart;
            var workedToday = current - workStart;

            if (timeToSubtract <= workedToday)
            {
                current -= timeToSubtract;
                break;
            }
            else
            {
                timeToSubtract -= workedToday;
                current = current.Date.AddDays(-1) + workDay.WorkdayEnd;
            }
        }

        return current.ToString("dd-MM-yyyy HH:mm");
    }


    private static bool IsHoliday(DateTime date, HashSet<DateTime> oneTime, HashSet<(int, int)> recurring)
    {
        return oneTime.Contains(date.Date) || recurring.Contains((date.Month, date.Day));
    }

    // Check if a date is a valid working day (Mon–Fri and not a holiday)
    private static bool IsWorkday(DateTime date, HashSet<DateTime> oneTime, HashSet<(int, int)> recurring)
    {
        return date.DayOfWeek >= DayOfWeek.Monday &&
               date.DayOfWeek <= DayOfWeek.Friday &&
               !IsHoliday(date, oneTime, recurring);
    }
}
