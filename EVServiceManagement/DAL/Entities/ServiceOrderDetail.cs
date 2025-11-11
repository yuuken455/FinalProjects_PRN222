using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class ServiceOrderDetail
{
    public int OrderDetailId { get; set; }

    public int? AppointmentId { get; set; }

    public int? ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Service? Service { get; set; }
}
