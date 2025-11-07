using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class TechnicianSchedule
{
    public int ScheduleId { get; set; }

    public int TechnicianId { get; set; }

    public int ShiftId { get; set; }

    public DateOnly WorkDate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public virtual WorkShift Shift { get; set; } = null!;

    public virtual ICollection<ShiftChangeRequest> ShiftChangeRequests { get; set; } = new List<ShiftChangeRequest>();

    public virtual Technician Technician { get; set; } = null!;
}
