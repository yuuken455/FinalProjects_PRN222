using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class ServiceReview
{
    public int ReviewId { get; set; }

    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
