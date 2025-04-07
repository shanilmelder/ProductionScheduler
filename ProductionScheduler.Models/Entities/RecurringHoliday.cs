namespace ProductionScheduler.Models;

public class RecurringHoliday : BaseAuditableEntity
{
    public int Id { get; set; }
    public int Month { get; set; }
    public int Date { get; set; }
}
