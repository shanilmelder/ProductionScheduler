﻿namespace ProductionScheduler.Models;
public abstract class BaseAuditableEntity
{
    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
    public bool IsActive { get; set; }
}
