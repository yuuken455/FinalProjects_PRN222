using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
