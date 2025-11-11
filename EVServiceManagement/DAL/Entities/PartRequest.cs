using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class PartRequest
{
    public int RequestId { get; set; }
    public int RequestedBy { get; set; }     // FK: Staffs.StaffId
    public int? ApprovedBy { get; set; }     // FK: Managers.ManagerId
    public int? PartId { get; set; }         // FK: Parts.PartId
    public int Quantity { get; set; }
    public DateTime? RequestDate { get; set; }   // default getdate()
    public DateTime? ApprovalDate { get; set; }
    public string Status { get; set; } = "Pending"; // Pending/Approved/Rejected/Received
    public string? Notes { get; set; }

    public virtual Staff RequestedByNavigation { get; set; } = null!;
    public virtual Manager? ApprovedByNavigation { get; set; }
    public virtual Part? Part { get; set; }
}
