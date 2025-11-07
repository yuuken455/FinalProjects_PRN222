using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();
}
