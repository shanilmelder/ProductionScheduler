namespace ProductionScheduler.Models;

public class OneTimeHoliday : BaseAuditableEntity
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
}
