using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class TechnicianAssignment
{
    public int AssignmentId { get; set; }

    public int AppointmentId { get; set; }

    public int TechnicianId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public string? Role { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Technician Technician { get; set; } = null!;
}
