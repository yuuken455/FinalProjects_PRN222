using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class ServicePart
{
    public int ServicePartId { get; set; }

    public int? ServiceId { get; set; }

    public int? PartId { get; set; }

    public virtual Part? Part { get; set; }

    public virtual Service? Service { get; set; }
}
