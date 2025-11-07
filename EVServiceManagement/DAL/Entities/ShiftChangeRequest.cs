using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class ShiftChangeRequest
{
    public int RequestId { get; set; }

    public int RequesterId { get; set; }

    public int ReceiverId { get; set; }

    public int ScheduleId { get; set; }

    public DateTime? RequestedDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? Notes { get; set; }

    public virtual Technician Receiver { get; set; } = null!;

    public virtual Technician Requester { get; set; } = null!;

    public virtual TechnicianSchedule Schedule { get; set; } = null!;
}
