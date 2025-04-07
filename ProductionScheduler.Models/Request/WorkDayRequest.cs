using ProductionScheduler.Models.Enum;

namespace ProductionScheduler.Models.Request;

public class WorkDayRequest
{
    public string StartDateTime { get; set; }
    public double TimeToAdd { get; set; }
    public string WorkdayStart { get; set; }
    public string WorkdayEnd { get; set; }
}
