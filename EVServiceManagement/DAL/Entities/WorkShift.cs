using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class WorkShift
{
    public int ShiftId { get; set; }

    public string Name { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<TechnicianSchedule> TechnicianSchedules { get; set; } = new List<TechnicianSchedule>();
}
