using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public int CustomerId { get; set; }

    public string Model { get; set; } = null!;

    public string? Vin { get; set; }

    public string LicensePlate { get; set; } = null!;

    public decimal? CurrentKm { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customer Customer { get; set; } = null!;
}
