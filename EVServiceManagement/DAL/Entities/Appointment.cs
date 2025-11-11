namespace DAL.Entities;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int VehicleId { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

    public virtual ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();

    public virtual ICollection<TechnicianAssignment> TechnicianAssignments { get; set; } = new List<TechnicianAssignment>();

    public virtual Vehicle Vehicle { get; set; } = null!;
}
