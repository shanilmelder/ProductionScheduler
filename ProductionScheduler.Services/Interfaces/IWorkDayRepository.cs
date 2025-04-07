using ProductionScheduler.Models.Entities;

namespace ProductionScheduler.Services.Interfaces;

public interface IWorkDayRepository
{
    Task<string> AddWorkingTime(WorkDay workDay);
    Task<string> SubtractWorkingTime(WorkDay workDay);
}