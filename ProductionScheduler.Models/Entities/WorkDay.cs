using ProductionScheduler.Models.Enum;

namespace ProductionScheduler.Models.Entities;

public class WorkDay
{
    public DateTime StartDateTime { get; set; }
    public double WorkingDaysToAdd { get; set; }
    public TimeSpan WorkdayStart { get; set; }
    public TimeSpan WorkdayEnd { get; set; }
    public CalculationType CalculationType { get; set; }
}
