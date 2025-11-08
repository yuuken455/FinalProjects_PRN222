namespace DAL.Entities;

public partial class Technician
{
    public int TechnicianId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<ShiftChangeRequest> ShiftChangeRequestReceivers { get; set; } = new List<ShiftChangeRequest>();

    public virtual ICollection<ShiftChangeRequest> ShiftChangeRequestRequesters { get; set; } = new List<ShiftChangeRequest>();

    public virtual ICollection<TechnicianAssignment> TechnicianAssignments { get; set; } = new List<TechnicianAssignment>();

    public virtual ICollection<TechnicianSchedule> TechnicianSchedules { get; set; } = new List<TechnicianSchedule>();
}
