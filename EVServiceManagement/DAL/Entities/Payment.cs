using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
}
