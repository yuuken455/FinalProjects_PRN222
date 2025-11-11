using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class Technician
{
    public int TechnicianId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<TechnicianAssignment> TechnicianAssignments { get; set; } = new List<TechnicianAssignment>();
}
